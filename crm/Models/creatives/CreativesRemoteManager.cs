using crm.Models.api.server;
using crm.Models.api.socket;
using crm.Models.appcontext;
using crm.Models.geoservice;
using crm.Models.storage;
using crm.ViewModels.tabs.home.screens.creatives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static crm.Models.api.server.BaseServerApi;

namespace crm.Models.creatives
{
    public class CreativesRemoteManager : ICreativesRemoteManager
    {

        #region const        
        #endregion

        #region vars                
        IPaths paths = Paths.getInstance();
        ApplicationContext AppContext = ApplicationContext.getInstance();
        WebClient client;
        Uri host;        
        long TotalBytes = 0;        
        IServerApi serverApi;
        string token;
        #endregion

        public CreativesRemoteManager()
        {
            serverApi = AppContext.ServerApi;
            token = AppContext.User.Token;
            client = new WebClient();
            NetworkCredential credential = new NetworkCredential(
                 "user287498742876",
                 "TK&9HhALSv3utvd58px3#tGgQ"
                 );
            client.Credentials = credential;
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.UploadProgressChanged += Client_UploadProgressChanged;            
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

        public async Task Download(string source, string destination)
        {
            //string url = $"{paths.CreativesRootURL}{source}";

            //await client.DownloadFileTaskAsync()
        }          

        public async Task Upload(GEO geo, string fullname)
        {
            string filename = Path.GetFileName(fullname);

            string[] splt = filename.Split(".");
            string name = splt[0];
            string extension = splt[1];

            int creative_id;
            string creative_name = null;
            string filepath = null;

            (creative_id, creative_name, filepath) = await serverApi.AddCreative(token, name, extension, geo);

            if (!string.IsNullOrEmpty(creative_name) && !string.IsNullOrEmpty(filepath))
            {
                TotalBytes = new System.IO.FileInfo(fullname).Length;
                string url = $"{paths.CreativesRootURL}{filepath}.{extension}";
                await client.UploadFileTaskAsync(new Uri(url), "PUT", fullname);
                await serverApi.SetCreativeStatus(token, creative_id, true, true);

            }
        }

        #region callbacks
        public event Action<int> UploadProgressUpdateEvent;
        public event Action<int> DownloadProgessUpdateEvent;
        #endregion
    }
}
