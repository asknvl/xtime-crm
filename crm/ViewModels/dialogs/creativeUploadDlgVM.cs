using crm.Models.api.server;
using crm.Models.storage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using geo = crm.Models.geoservice;

namespace crm.ViewModels.dialogs
{
    public class creativeUploadDlgVM : ViewModelBase
    {
        #region vars
        IServerApi server;
        string token;
        CancellationTokenSource cts;
        IPaths paths = Paths.getInstance();
        WebClient client;
        long TotalBytes = 0;
        #endregion

        #region properties
        public string[] Files { get; set; }
        public geo.GEO GEO { get; set; }

        int progress;
        public int Progress
        {
            get => progress;
            set => this.RaiseAndSetIfChanged(ref progress, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> cancelCmd { get; }
        #endregion
        public creativeUploadDlgVM()
        {
            server = AppContext.ServerApi;
            token = AppContext.User.Token;
            client = new WebClient();            
            NetworkCredential credential = new NetworkCredential(
                "user287498742876",
                "TK&9HhALSv3utvd58px3#tGgQ"
                );

            client.Credentials = credential;
            client.UploadProgressChanged += Client_UploadProgressChanged;
            client.UploadFileCompleted += Client_UploadFileCompleted;

            #region commands
            cancelCmd = ReactiveCommand.Create(() => {
                cts?.Cancel();
            });
            #endregion
        }

        private void Client_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            Progress = 0;
        }

        private void Client_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            Progress = (int)(e.BytesSent / TotalBytes * 100);            
        }

        public async Task RunFilesUploadAsync()
        {
            cts = new CancellationTokenSource();

            try
            {
                await Task.Run(async () =>
                {
                    foreach (var file in Files)
                    {
                        string filename = Path.GetFileName(file);
                        string[] splt = filename.Split(".");
                        string name = splt[0];
                        string extension = splt[1];

                        string creative_name = null;
                        string filepath = null;
                        (creative_name, filepath) = await server.AddCreative(token, name, extension, GEO);   

                        if (!string.IsNullOrEmpty(creative_name) && !string.IsNullOrEmpty(filepath))
                        {
                            TotalBytes = new System.IO.FileInfo(file).Length;                            
                            string url = $"{paths.CreativesRootURL}{filepath}.{extension}";                            
                            await client.UploadFileTaskAsync(new Uri(url), "PUT", file);
                        }
                        cts.Token.ThrowIfCancellationRequested();
                    }
                });

            } catch (OperationCanceledException)
            {
                
            } finally
            {
                OnCloseRequest();
            }
        }
    }
}
