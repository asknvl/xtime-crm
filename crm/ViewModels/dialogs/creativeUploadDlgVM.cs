using crm.Models.api.server;
using crm.Models.creatives;
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
        ICreativesRemoteManager creativesRemoteManager;
        CancellationTokenSource cts;
        IPaths paths = Paths.getInstance();
        int filesNumber = 1;
        #endregion

        #region properties
        public string[] Files { get; set; }
        public CreativeServerDirectory CreativeServerDirectory { get; set; }

        int progress;
        public int Progress
        {
            get => progress;
            set => this.RaiseAndSetIfChanged(ref progress, value);
        }

        string filesCounter;
        public string FilesCounter
        {
            get => filesCounter;
            set => this.RaiseAndSetIfChanged(ref filesCounter, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> cancelCmd { get; }
        #endregion
        public creativeUploadDlgVM()
        {
            creativesRemoteManager = new CreativesRemoteManager();
            //creativesRemoteManager.UploadProgressUpdateEvent += (progress) =>
            //{
            //    Progress = progress;                
            //};

            #region commands
            cancelCmd = ReactiveCommand.Create(() => {
                cts?.Cancel();
            });
            #endregion
        }

        private void CreativesRemoteManager_UploadProgressUpdateEvent(int progress)
        {
            Progress = progress;
            Thread.Sleep(10);
            //Debug.WriteLine(progress);
        }

        public async Task RunFilesUploadAsync()
        {
            cts = new CancellationTokenSource();

            try
            {
                AppContext.SocketApi.NeedNotifyCreativeAction = false;

                await Task.Run(async () =>
                {
                    int fcounter = 0;
                    int totalFiles = Files.Length;                    

                    if (totalFiles == 1)
                    {
                        creativesRemoteManager.UploadProgressUpdateEvent -= CreativesRemoteManager_UploadProgressUpdateEvent;
                        creativesRemoteManager.UploadProgressUpdateEvent += CreativesRemoteManager_UploadProgressUpdateEvent;
                    }

                    foreach (var file in Files)
                    {

                        if (totalFiles - fcounter == 1)
                            AppContext.SocketApi.NeedNotifyCreativeAction = true;    
                            
                        await creativesRemoteManager.Upload(CreativeServerDirectory, file);

                        FilesCounter = $"Загружено {++fcounter} из {Files.Length}";

                        if (totalFiles > 1)
                            Progress = fcounter * 100 / totalFiles;

                        cts.Token.ThrowIfCancellationRequested();
                    }
                });

            } catch (OperationCanceledException)
            {
                
            } finally
            {
                AppContext.SocketApi.NeedNotifyCreativeAction = true;
                OnCloseRequest();
            }
        }
    }
}
