using crm.Models.appcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens
{
    public class AccountsImport : BaseScreen
    {
        public AccountsImport(ApplicationContext appcontext) : base(appcontext)
        {
        }

        public override string Title => "Импорт аккаунтов";
    }
}
