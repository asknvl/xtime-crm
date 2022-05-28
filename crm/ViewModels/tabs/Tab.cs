using crm.ViewModels.tabs.tabservice;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;

namespace crm.ViewModels.tabs
{
    public abstract class Tab : ViewModelBase
    {
        #region vars
        ITabService tabService;
        #endregion

        #region properties       
        string title;
        public string Title
        {
            get => title;
            set => this.RaiseAndSetIfChanged(ref title, value);
        }

        bool isInputValid;
        protected bool IsInputValid
        {
            get => isInputValid;
            set => this.RaiseAndSetIfChanged(ref isInputValid, value);
        }
        protected bool needValidate { get; set; }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> closeCmd { get; }
        #endregion

        public Tab(ITabService ts)
        {
            tabService = ts;            

            closeCmd = ReactiveCommand.Create(() => {
                Close();
            });
        }

        #region protected
        protected bool CheckValidity(bool[] fields)
        {
            return !fields.Any(p => p == false);
        }
        //protected virtual void CloseRequest()
        //{
        //    TabClosedEvent?.Invoke(this);
        //}
        #endregion

        #region public                
        public virtual void Show()
        {            
            tabService.ShowTab(this);            
        }

        public virtual void Close()
        {   
            tabService.CloseTab(this);            
        }
        #endregion

        #region virtual
        public virtual void Clear() { }

        public virtual void Refresh() { }
        #endregion

    }
}
