using crm.Models.api.server;
using crm.Models.appcontext;
using crm.Models.user;
using crm.ViewModels.tabs.home.menu;
using crm.ViewModels.tabs.home;
using crm.ViewModels.tabs.tabservice;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using scrn = crm.ViewModels.tabs.home.screens;
using itms = crm.ViewModels.tabs.home.menu.items;

namespace crm.ViewModels.tabs
{
    public class homeVM : Tab
    {
        #region vars
        BaseServerApi api;
        ApplicationContext AppContext;
        #endregion

        #region properties     
        BaseMenu menu;
        public BaseMenu Menu
        {
            get => menu;
            set => this.RaiseAndSetIfChanged(ref menu, value);
        }
        //object screen;
        //public object Screen
        //{
        //    get => screen;
        //    set => this.RaiseAndSetIfChanged(ref screen, value);
        //}
        string initialLetter;
        public string InitialLetter
        {
            get => initialLetter;
            set => this.RaiseAndSetIfChanged(ref initialLetter, value);
        }
        #endregion

        #region commands        
        public ReactiveCommand<Unit, Unit> profileMenuOpenCmd { get; }
        public ReactiveCommand<Unit, Unit> editUserCmd { get; }
        public ReactiveCommand<Unit, Unit> quitCmd { get; }
        #endregion

        public homeVM() : base((new ApplicationContext()).TabService)
        {
            Menu = new admin_menu();
        }

        public homeVM(ITabService ts, ApplicationContext appcontext) : base(ts)
        {

            Title = "Домой";
            //List<Role> roles = appcontext.User.Roles;
            ////Menu = new admin_menu(appcontext);
            //BaseMenu menu = new menu(appcontext);

            //SimpleMenuItem dashboard = new itms.Dashboard();
            //dashboard.AddScreen(new scrn.Dashboard(appcontext));
            //menu.AddItem(dashboard);

            //bool needUsers = roles.Any(r => r.Type == RoleType.admin ||
            //                           r.Type == RoleType.team_lead_comment ||
            //                           r.Type == RoleType.team_lead_farm ||
            //                           r.Type == RoleType.team_lead_link ||
            //                           r.Type == RoleType.team_lead_media
            //                           );


            //bool needAccImport = roles.Any(r => r.Type == RoleType.admin ||
            //                                    r.Type == RoleType.team_lead_farm ||
            //                                    r.Type == RoleType.buyer_farm
            //                                    );

            //bool needCreatives = roles.Any(r => r.Type == RoleType.admin ||
            //                                    r.Type == RoleType.team_lead_media ||
            //                                    r.Type == RoleType.buyer_media ||
            //                                    r.Type == RoleType.creative                                                
            //                                    );

            //if (needUsers)
            //{
            //    ComplexMenuItem users = new itms.Users();
            //    users.AddScreen(new scrn.UserList(appcontext));
            //    users.AddScreen(new scrn.UserActions(appcontext));
            //    menu.AddItem(users);
            //}


            //Menu = menu;
            Menu = new admin_menu(appcontext);
        }

        public override void Show()
        {
            foreach (var item in Menu.Items)
                foreach (var screen in item.Screens)
                    screen.OnActivate();

            base.Show();
        }

        public override void Close()
        {

            foreach (var item in Menu.Items)
                foreach (var screen in item.Screens)
                    screen.OnDeactivate();


            base.Close();
        }
    }
}
