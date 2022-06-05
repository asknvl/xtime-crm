using crm.Models.appcontext;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens.settings
{
    public class Settings : BaseScreen
    {
        #region properties
        public override string Title => "Настройки";
        public ObservableCollection<BaseScreen> SettingsPages { get; }

        Object content;
        public Object Content
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }
        #endregion
        public Settings() : base(new ApplicationContext())
        {
            SettingsPages = new ObservableCollection<BaseScreen>();
            SettingsPages.Add(new basicSettings(AppContext));
        }
        public Settings(ApplicationContext context) : base(context)
        {
            SettingsPages = new ObservableCollection<BaseScreen>();
            SettingsPages.Add(new basicSettings(AppContext));
        }
    }
}
