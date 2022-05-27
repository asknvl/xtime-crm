using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.tabservice
{
    public interface ITabService
    {
        void ShowTab(Tab tab);
        void AddTab(Tab tab);
        void CloseTab(Tab tab);
    }
}
