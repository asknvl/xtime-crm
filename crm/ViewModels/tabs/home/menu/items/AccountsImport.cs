using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.menu.items
{
    public class AccountsImport : SimpleMenuItem
    {
        public override string Title => "Импортировать аккаунты";
        public override string IconPath => "/Assets/svgs/leftslidemenu/acc_import.svg";
    }
}
