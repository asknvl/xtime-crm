using crm.Models.api.server;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        #endregion

        #region properties
        public string[] Files { get; set; }
        public geo.GEO GEO { get; set; }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> cancelCmd { get; }
        #endregion
        public creativeUploadDlgVM()
        {
            server = AppContext.ServerApi;
            token = AppContext.User.Token;

            #region commands
            cancelCmd = ReactiveCommand.Create(() => {
                cts?.Cancel();
            });
            #endregion
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
                        await server.AddCreative(token, file, GEO);    !
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
