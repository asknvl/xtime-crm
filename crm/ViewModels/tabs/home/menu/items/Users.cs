using crm.ViewModels.tabs.home.screens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.menu.items
{
    public class Users : ComplexMenuItem
    {       
        public override string Title => "Сотрудники";
        public override string IconPath => "/Assets/svgs/leftslidemenu/user.svg";
        
    }
}
