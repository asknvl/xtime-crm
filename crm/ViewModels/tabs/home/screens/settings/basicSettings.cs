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
        #endregion
        public basicSettings() : base()
        {
            RememberMe = AppContext.Settings.RememberMe;
        }

        public override void OnDeactivate()
        {
            //AppContext.Settings.Save();
        }
    }
}
