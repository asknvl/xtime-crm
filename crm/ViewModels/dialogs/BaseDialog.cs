using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.dialogs
{
    public abstract class BaseDialog : ViewModelBase
    {
        #region properties
        public virtual string Title { get; }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> okCmd { get; }
        #endregion

        public BaseDialog()
        {
            okCmd = ReactiveCommand.Create(() => {
                OnCloseRequest();
            });
        }
    }
}
