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

        List<int> creosPerDragDropList = new List<int> { 1, 5, 10, 15, 20, 25, 30, 40, 50 };
        public List<int> CreosPerDragDropList
        {
            get => creosPerDragDropList;
            set => this.RaiseAndSetIfChanged(ref creosPerDragDropList, value);
        }

        bool isCreosPerDragDropVisible;
        public bool IsCreosPerDragDropVisible
        {
            get => isCreosPerDragDropVisible;
            set => this.RaiseAndSetIfChanged(ref isCreosPerDragDropVisible, value);
        }

        int creosPerDragDrop;
        public int CreosPerDragDrop
        {
            get => creosPerDragDrop;
            set
            {
                AppContext.Settings.CreativesPerDragDrop = value;
                this.RaiseAndSetIfChanged(ref creosPerDragDrop, value);
                AppContext.Settings.Save();
                IsCreosPerDragDropVisible = false;
            }
        }

        #endregion
        public basicSettings() : base()
        {
            RememberMe = AppContext.Settings.RememberMe;            
            CreosPerPage = AppContext.Settings.CreativesPerPage;
            CreosPerDragDrop = AppContext.Settings.CreativesPerDragDrop;
        }

        public override void OnDeactivate()
        {            
        }
    }
}
