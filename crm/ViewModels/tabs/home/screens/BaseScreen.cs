using crm.Models.appcontext;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens
{
    public abstract class BaseScreen : ViewModelBase
    {
        #region vars
        protected ApplicationContext AppContext;
        #endregion

        #region properties
        bool NeedInvoke { get; set; } = true;

        bool isChecked;
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                if (NeedInvoke && !value)
                    return;
                   
                this.RaiseAndSetIfChanged(ref isChecked, value);
                if (NeedInvoke)
                    ScreenCheckedEvent?.Invoke(this, value);
            }
        }
        public virtual string Title { get; set; }        
        #endregion

        public BaseScreen()
        {
            AppContext = ApplicationContext.getInstance();
        }

        #region public
        public void Uncheck()
        {
            NeedInvoke = false;
            IsChecked = false;
            NeedInvoke = true;
        }
        public virtual void OnActivate() {
            Debug.WriteLine($"Activated:{Title}");
        }
        public virtual void OnDeactivate() {
            Debug.WriteLine($"Deactivated:{Title}");
        }
        #endregion

        #region callbacks
        public event Action<BaseScreen, bool>? ScreenCheckedEvent;
        #endregion

    }
}
