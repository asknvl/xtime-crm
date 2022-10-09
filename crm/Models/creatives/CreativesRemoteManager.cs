using crm.Models.api.server;
using crm.Models.api.socket;
using crm.Models.appcontext;
using crm.Models.geoservice;
using crm.Models.storage;
using crm.ViewModels.tabs.home.screens.creatives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static crm.Models.api.server.BaseServerApi;
using WebDav;

namespace crm.Models.creatives
{
    public class CreativesRemoteManager : ICreativesRemoteManager
    {

        #region const        
        NetworkCredential credential = new NetworkCredential(
                 "user287498742876",
                 "TK&9HhALSv3utvd58px3#tGgQ"
                 );
        #endregion

        #region vars                
        IPaths paths = Paths.getInstance();
        ApplicationContext AppContext = ApplicationContext.getInstance();
        //ExtendedWebClient client;
        Uri host;        
        long TotalBytes = 0;        
        IServerApi serverApi;
        string token;
        List<ICreative> downloadingList = new();


        IWebDavClient webdav;
        #endregion

        public CreativesRemoteManager()
        {
            serverApi = AppContext.ServerApi;
            token = AppContext.User.Token;
            //client = new ExtendedWebClient();
            
            //client.Timeout = 100000;
            //NetworkCredential credential = new NetworkCredential(
            //     "user287498742876",
            //     "TK&9HhALSv3utvd58px3#tGgQ"
            //     );
            //client.Credentials = credential;
            //client.DownloadProgressChanged += Client_DownloadProgressChanged;
            //client.UploadProgressChanged += Client_UploadProgressChanged;
            //client.DownloadFileCompleted += Client_DownloadFileCompleted;
            

            // / webdav / uniq
            var clientParams = new WebDavClientParams
            {
                BaseAddress = new Uri("http://136.243.74.153:4080"),
                Credentials = new NetworkCredential("user287498742876", "TK&9HhALSv3utvd58px3#tGgQ"),
            };
            webdav = new WebDavClient(clientParams);           
            

        }

        private void Client_DownloadFileCompleted(object? sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            DownloadCompleted?.Invoke();
        }

        #region private        
        private void Client_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            int progress = (int)(e.BytesSent * 100.0d / TotalBytes );
            Debug.WriteLine(progress);
            UploadProgressUpdateEvent?.Invoke(progress);
        }
        
        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            int progress = (int)(e.BytesReceived * 100.0d / e.TotalBytesToReceive);
            DownloadProgessUpdateEvent?.Invoke(progress);            
        }
        #endregion

       

        public async Task Download(ICreative creative)
        {
            var client = new ExtendedWebClient();

            client.Timeout = 100000;
            NetworkCredential credential = new NetworkCredential(
                 "user287498742876",
                 "TK&9HhALSv3utvd58px3#tGgQ"
                 );
            client.Credentials = credential;
            client.DownloadProgressChanged += Client_DownloadProgressChanged;            
            client.DownloadFileCompleted += Client_DownloadFileCompleted;

            try
            {

                if (File.Exists(creative.LocalPath))
                    File.Delete(creative.LocalPath);

                var found = downloadingList.FirstOrDefault(c => c.Id == creative.Id);
                if (found == null)
                {
                    downloadingList.Add(creative);
                    await client.DownloadFileTaskAsync(new Uri(creative.UrlPath), creative.LocalPath);                    

                    downloadingList.Remove(creative);
                } else
                {
                    Debug.WriteLine("Already downloading");
                }

                 
                //await Task.Run(() =>
                //{
                //    DownloadFile(new Uri(creative.UrlPath), creative.LocalPath);
                //});

            } catch (Exception ex)
            {
                Debug.WriteLine(ex);
            } finally
            {
                client.Dispose();
            }
        }          

        public async Task Upload(CreativeServerDirectory dir, string fullname)
        {
            string filename = Path.GetFileName(fullname);

            string[] splt = filename.Split(".");
            string name = splt[0];
            string extension = splt[1];

            int creative_id;
            string creative_name = null;
            string filepath = null;

            (creative_id, creative_name, filepath) = await serverApi.AddCreative(token, name, extension, dir);

            if (!string.IsNullOrEmpty(creative_name) && !string.IsNullOrEmpty(filepath))
            {
                TotalBytes = new System.IO.FileInfo(fullname).Length;
                string url = $"{paths.CreativesRootURL}{filepath}.{extension}";


                var client = new ExtendedWebClient();

                client.Timeout = 100000;
                NetworkCredential credential = new NetworkCredential(
                     "user287498742876",
                     "TK&9HhALSv3utvd58px3#tGgQ"
                     );
                client.Credentials = credential;
                //client.DownloadProgressChanged += Client_DownloadProgressChanged;
                client.UploadProgressChanged += Client_UploadProgressChanged;
                //client.DownloadFileCompleted += Client_DownloadFileCompleted;

                await client.UploadFileTaskAsync(new Uri(url), "PUT", fullname);

                client.Dispose();

                //await webdav.PutFile(new Uri(url), File.OpenRead(fullname));             
                
                await serverApi.SetCreativeStatus(token, creative_id, true, true);

            }
        }

        public async Task<long> GetFileSize(ICreative creative)
        {
            long res = 0;

            System.Net.WebRequest req = System.Net.WebRequest.Create(creative.UrlPath);
            req.Credentials = credential;
            req.Method = "HEAD";

            await Task.Run(() => {
                using (System.Net.WebResponse resp = req.GetResponse())
                {
                    if (long.TryParse(resp.Headers.Get("Content-Length"), out long ContentLength))
                    {
                        res = ContentLength;
                    }
                }
            });

            return res;
        }

        #region callbacks
        public event Action<int> UploadProgressUpdateEvent;
        public event Action<int> DownloadProgessUpdateEvent;
        public event Action DownloadCompleted;
        #endregion
    }
}
