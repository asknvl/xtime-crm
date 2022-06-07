using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.dialogs
{
    public class msgDlgVM : ViewModelBase
    {
        #region properties
        string message;
        public string Message
        {
            get => message;
            set => this.RaiseAndSetIfChanged(ref message, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> nextCmd { get; }
        #endregion

        public msgDlgVM(string message)
        {

            Message = message;

            nextCmd = ReactiveCommand.Create(() => {
                OnCloseRequest();
            });
        }

    }
}
