using crm.Models.appcontext;
using crm.ViewModels.tabs.home.screens;
using crm.ViewModels.tabs.tabservice;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs
{
    public class ScreenTab : Tab
    {

        #region properties
        Object screen;
        public Object Screen {
            get => screen;
            set => this.RaiseAndSetIfChanged(ref screen, value);
        }
        #endregion

        public ScreenTab(ITabService ts, BaseScreen screen) : base(ts)
        {
            Title = screen.Title;
            Screen = screen;
        }

        public override void Show()
        {
            base.Show();
        }

        public override void Close()
        {
            base.Close();
        }
    }
}
