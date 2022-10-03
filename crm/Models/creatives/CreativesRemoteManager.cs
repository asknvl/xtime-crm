﻿using crm.Models.api.server;
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
using WinSCP;

namespace crm.Models.creatives
{
    public class CreativesRemoteManager : ICreativesRemoteManager
    {

        #region const        
        #endregion

        #region vars                
        IPaths paths = Paths.getInstance();
        ApplicationContext AppContext = ApplicationContext.getInstance();
        ExtendedWebClient client;
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
            client = new ExtendedWebClient();
            client.Timeout = 100000;
            NetworkCredential credential = new NetworkCredential(
                 "user287498742876",
                 "TK&9HhALSv3utvd58px3#tGgQ"
                 );
            client.Credentials = credential;
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.UploadProgressChanged += Client_UploadProgressChanged;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;


            // / webdav / uniq
            var clientParams = new WebDavClientParams
            {
                BaseAddress = new Uri("http://136.243.74.153:4080"),
                Credentials = new NetworkCredential("user287498742876", "TK&9HhALSv3utvd58px3#tGgQ")
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
            UploadProgressUpdateEvent?.Invoke(progress);
        }
        
        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            int progress = (int)(e.BytesReceived * 100.0d / e.TotalBytesToReceive);
            DownloadProgessUpdateEvent?.Invoke(progress);            
        }
        #endregion

        private async void DownloadFile(Uri remoteUri, string localPath)
        {
            var request = (HttpWebRequest)WebRequest.Create(remoteUri);
            request.Timeout = 30000;
            request.AllowWriteStreamBuffering = false;

            NetworkCredential credential = new NetworkCredential(
               "user287498742876",
               "TK&9HhALSv3utvd58px3#tGgQ"
               );

            request.Credentials = credential;

            using (var response = await request.GetResponseAsync())
            using (var s = response.GetResponseStream())
            using (var fs = new FileStream(localPath, FileMode.Create))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                int total = 0;
                while ((bytesRead = s.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, bytesRead);
                    bytesRead = s.Read(buffer, 0, buffer.Length);
                    total += bytesRead;

                }

                Debug.WriteLine("" + total);
            }
        }

        public async Task Download(ICreative creative)
        {
            try
            {
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

                //await client.UploadFileTaskAsync(new Uri(url), "PUT", fullname);                

                await webdav.PutFile(new Uri(url), File.OpenRead(fullname));

                try
                {
                    // Setup session options
                    SessionOptions sessionOptions = new SessionOptions
                    {
                        Protocol = Protocol.Webdav,
                        HostName = "136.243.74.153",
                        UserName = "user287498742876",
                        Password = "TK&9HhALSv3utvd58px3#tGgQ",
                        PortNumber = 4080
                        //SshHostKeyFingerprint = "ssh-rsa 2048 xxxxxxxxxxx..."
                    };

                    using (Session session = new Session())
                    {
                        // Connect
                        session.Open(sessionOptions);
                        session.FileTransferProgress += (s, e) => {
                            UploadProgressUpdateEvent?.Invoke((int)e.FileProgress);
                        };

                        // Upload files
                        TransferOptions transferOptions = new TransferOptions();
                        transferOptions.TransferMode = TransferMode.Automatic;                        

                        TransferOperationResult transferResult;

                        //transferResult =
                        //    session.PutFiles(fullname, url, false, transferOptions);

                        await Task.Run(() => { 
                            session.PutFile(File.OpenRead(fullname), url);
                        });

                        // Throw on any error


                        // Print results

                    }

                    
                } catch (Exception e)
                {
                    Debug.WriteLine("Error: {0}", e);
                    
                }



                await serverApi.SetCreativeStatus(token, creative_id, true, true);

            }
        }

        private void Session_FileTransferProgress(object sender, FileTransferProgressEventArgs e)
        {
            throw new NotImplementedException();
        }

        #region callbacks
        public event Action<int> UploadProgressUpdateEvent;
        public event Action<int> DownloadProgessUpdateEvent;
        public event Action DownloadCompleted;
        #endregion
    }
}
