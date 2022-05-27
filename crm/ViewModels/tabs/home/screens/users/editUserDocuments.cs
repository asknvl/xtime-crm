using crm.Models.appcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens.users
{
    public class editUserDocuments : BaseScreen
    {
        public override string Title => "Документы";
        public editUserDocuments(ApplicationContext context) : base(context)
        {
        }
    }
}
