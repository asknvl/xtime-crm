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

namespace crm.Models.creatives
{
    public class CreativesRemoteManager : ICreativesRemoteManager
    {
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
            client.UploadDataCompleted += Client_UploadDataCompleted;
        }
        
        #region private        
        private void Client_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            int progress = (int)(e.BytesSent / TotalBytes * 100);
            UploadProgressUpdateEvent?.Invoke(progress);
        }
        private void Client_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
        {
            UploadProgressUpdateEvent?.Invoke(0);
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        public Task<List<BaseCreative>> GetAvaliableAsync(GEO geo, CreativeType type, bool showInvisible)
        {
            throw new NotImplementedException();
        }

        public async Task Upload(GEO geo, string fullname)
        {
            string filename = Path.GetFileName(fullname);

            string[] splt = filename.Split(".");
            string name = splt[0];
            string extension = splt[1];

            string creative_name = null;
            string filepath = null;
            (creative_name, filepath) = await serverApi.AddCreative(token, name, extension, geo);

            if (!string.IsNullOrEmpty(creative_name) && !string.IsNullOrEmpty(filepath))
            {
                TotalBytes = new System.IO.FileInfo(fullname).Length;
                string url = $"{paths.CreativesRootURL}{filepath}.{extension}";
                await client.UploadFileTaskAsync(new Uri(url), "PUT", fullname);
            }
        }

        #region callbacks
        public event Action<int> UploadProgressUpdateEvent;
        #endregion
    }
}
