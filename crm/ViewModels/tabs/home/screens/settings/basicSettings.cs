using crm.Models.appcontext;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens.settings
{
    public class basicSettings : BaseScreen
    {
        #region properties
        public override string Title => "Основные";

        bool rememberMe;
        public bool RememberMe
        {
            get => rememberMe;
            set
            {
                AppContext.Settings.RememberMe = value;
                this.RaiseAndSetIfChanged(ref rememberMe, value);
                AppContext.Settings.Save();
            }
        }

        List<int> creosPerPageList = new List<int> { 25, 50, 100, 200 };
        public List<int> CreosPerPageList
        {
            get => creosPerPageList;
            set => this.RaiseAndSetIfChanged(ref creosPerPageList, value);
        }

        bool isCreosPerPageVisible;
        public bool IsCreosPerPageVisible
        {
            get => isCreosPerPageVisible;
            set => this.RaiseAndSetIfChanged(ref isCreosPerPageVisible, value);
        }

        int creosPerPage;
        public int CreosPerPage
        {
            get => creosPerPage;
            set {                
                AppContext.Settings.CreativesPerPage = value;                
                this.RaiseAndSetIfChanged(ref creosPerPage, value);                
                AppContext.Settings.Save();
                IsCreosPerPageVisible = false;
            }
        }

        #endregion
        public basicSettings() : base()
        {
            RememberMe = AppContext.Settings.RememberMe;
            CreosPerPage = AppContext.Settings.CreativesPerPage;
        }

        public override void OnDeactivate()
        {
            //AppContext.Settings.Save();
        }
    }
}
