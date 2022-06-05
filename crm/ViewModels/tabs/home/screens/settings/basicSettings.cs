using crm.Models.appcontext;
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
        #endregion

        public basicSettings() : base(new ApplicationContext())
        {
        }

        public basicSettings(ApplicationContext context) : base(context)
        {
        }
    }
}
