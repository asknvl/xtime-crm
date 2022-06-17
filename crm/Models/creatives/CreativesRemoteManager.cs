using crm.Models.api.server;
using crm.Models.api.socket;
using crm.Models.geoservice;
using crm.ViewModels.tabs.home.screens.creatives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.creatives
{
    public class CreativesRemoteManager : ICreativesRemoteManager
    {
        #region vars                
        WebClient client;
        Uri host;
        IServerApi server;
        ISocketApi socket;
        #endregion

        public CreativesRemoteManager(string host, IServerApi server, ISocketApi socket)
        {
            this.host = new Uri(host);
            this.server = server;
            this.socket = socket;

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
            UploadProgressUpdateEvent?.Invoke(e.ProgressPercentage);
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
            string fname = Path.GetFileName(fullname);
            //Uri uri = new Uri(host, $"{geo.Name}/test.mp4");
            Uri uri = host;
            await client.UploadFileTaskAsync(uri, "POST", fullname);
        }

        #region callbacks
        public event Action<float> UploadProgressUpdateEvent;
        #endregion
    }
}
