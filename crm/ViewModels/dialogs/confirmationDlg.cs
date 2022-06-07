using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.dialogs
{
    public class confirmationDlg : BaseDialog
    {
        #region properties
        string title;
        public string Title
        {
            get => title;
            set => this.RaiseAndSetIfChanged(ref title, value);
        }

        string message;
        public string Message
        {
            get => message;
            set => this.RaiseAndSetIfChanged(ref message, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> cancelCmd { get; }
        public ReactiveCommand<Unit, Unit> confirmCmd { get; }
        #endregion
        public confirmationDlg()
        {
            #region commands
            cancelCmd = ReactiveCommand.Create(() => {
                DialogResultEvent?.Invoke(false);
            });
            confirmCmd = ReactiveCommand.Create(() => {
                DialogResultEvent?.Invoke(true);
            });
            #endregion
        }

        #region public
        public event Action<bool> DialogResultEvent;
        #endregion
    }
}
