using crm.Models.appcontext;
using crm.ViewModels.tabs.home.screens.creatives;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using WinSCP;

namespace crm.ViewModels.tabs.home.screens
{
    public class Creatives : BaseScreen
    {
        #region properties
        public override string Title => "Креативы";
        public ObservableCollection<BaseCreative> CreativesList { get; }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> testCmd { get; }
        #endregion

        public Creatives() : base()
        {
            testCmd = ReactiveCommand.Create(() => {

                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Webdav,
                    HostName = "136.243.74.153",
                    PortNumber = 4080,
                    UserName = "user287498742876",
                    Password = "TK&9HhALSv3utvd58px3#tGgQ",
                };

                using (Session session = new Session())
                {
                    session.FileTransferProgress += Session_FileTransferProgress;
                    session.Open(sessionOptions);
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult =
                        session.PutFiles(@"d:\out\*", "/webdav/uniq/", false, transferOptions);

                    // Throw on any error
                    transferResult.Check();

                    // Print results
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                    }
                }

            });
        }

        private void Session_FileTransferProgress(object sender, FileTransferProgressEventArgs e)
        {
            Debug.WriteLine("\r{0} ({1:P0})", e.FileName, e.FileProgress);
        }
    }
}
