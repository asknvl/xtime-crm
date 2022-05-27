using crm.ViewModels.tabs.home.screens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.menu.items
{
    public class Dashboard : SimpleMenuItem
    {
        //public override ObservableCollection<BaseScreen> Screens => new ObservableCollection<BaseScreen>()
        //{
        //    new screens.Dashboard()
        //};
        public override string Title => "Dashboard";
        public override string IconPath => "/Assets/svgs/leftslidemenu/dashboard.svg";        
    }
}
