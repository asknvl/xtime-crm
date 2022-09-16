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
    public class progressDlgVM : ViewModelBase
    {
        #region vars                
        CancellationTokenSource cts;        
        #endregion

        #region properties        
        int progress;
        public int Progress
        {
            get => progress;
            set => this.RaiseAndSetIfChanged(ref progress, value);
        }

        string progressText;
        public string ProgressText
        {
            get => progressText;
            set => this.RaiseAndSetIfChanged(ref progressText, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> cancelCmd { get; }
        #endregion
        public progressDlgVM()
        {            

            #region commands
            cancelCmd = ReactiveCommand.Create(() => {
                cts?.Cancel();
            });
            #endregion
        }        
    }
}
