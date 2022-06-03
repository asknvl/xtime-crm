using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens
{
    public interface ICheckable<T>
    {
        bool IsChecked { get; set; }
        event Action<T, bool> CheckedEvent;        
    }
}
