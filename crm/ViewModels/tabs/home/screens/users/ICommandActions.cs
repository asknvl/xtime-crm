using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens.users
{
    public interface ICommandActions
    {
        Task<bool> Save();
        void Cancel();        
        bool IsEditable { get; set; }
    }
}
