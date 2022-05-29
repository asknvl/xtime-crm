using Avalonia.Controls;
using crm.Models.api.server;
using crm.Models.api.socket;
using crm.Models.appcontext;
using crm.Models.user;
using crm.ViewModels.tabs;
using crm.ViewModels.tabs.home.screens.users;
using crm.ViewModels.tabs.tabservice;
using ReactiveUI;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace crm.ViewModels
{
    public class mainVM : ViewModelBase, ITabService
    {

        #region vars
        homeVM homeTab;
        loginVM loginTab;
        tokenVM tokenTab;
        registrationVM registrationTab;
        //BaseServerApi api;
        #endregion

        #region properties             
        WindowState windowState;
        public WindowState WindowState
        {
            get => windowState;
            set => this.RaiseAndSetIfChanged(ref windowState, value);
        }

        BaseUser user;
        BaseUser User
        {
            get => user;
            set => this.RaiseAndSetIfChanged(ref user, value);

        }

        bool isUserMenuVisible;
        public bool IsUserMenuVisible
        {
            get => isUserMenuVisible;
            set => this.RaiseAndSetIfChanged(ref isUserMenuVisible, value);
        }

        bool isProfileMenuOpen;
        public bool IsProfileMenuOpen
        {
            get => isProfileMenuOpen;
            set
            {
                isProfileMenuOpen = value;
                this.RaisePropertyChanged("IsProfileMenuOpen");
            }
        }

        bool isStripVisible;
        public bool IsStripVisible
        {
            get => isStripVisible;
            set => this.RaiseAndSetIfChanged(ref isStripVisible, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> closeCmd { get; }
        public ReactiveCommand<Unit, Unit> maximizeCmd { get; }
        public ReactiveCommand<Unit, Unit> minimizeCmd { get; }
        public ReactiveCommand<Unit, Unit> profileMenuOpenCmd { get; }
        public ReactiveCommand<Unit, Unit> editUserCmd { get; }
        public ReactiveCommand<Unit, Unit> quitCmd { get; }
        #endregion
        public mainVM()
        {

            #region dependencies
            ApplicationContext AppContext = new ApplicationContext();
#if DEBUG
            AppContext.ServerApi = new ServerApi("http://136.243.74.153:4000");
            AppContext.SocketApi = new SocketApi("http://136.243.74.153:4000");
            //AppContext.ServerApi = new ServerApi("http://185.46.9.229:4000");
            //AppContext.SocketApi = new SocketApi("http://185.46.9.229:4000");

#elif RELEASE
            AppContext.ServerApi = new ServerApi("http://136.243.74.153:4000");
            AppContext.SocketApi = new SocketApi("http://136.243.74.153:4000");
#endif

            AppContext.TabService = this;
            #endregion

            #region init
            WindowState = WindowState.Normal;
            #endregion

            #region commands           
            maximizeCmd = ReactiveCommand.Create(() =>
            {
                if (WindowState == WindowState.Maximized)
                    WindowState = WindowState.Normal;
                else
                if (WindowState == WindowState.Normal)
                    WindowState = WindowState.Maximized;
                else
                if (WindowState == WindowState.Minimized)
                    WindowState = WindowState.Maximized;
            });

            closeCmd = ReactiveCommand.Create(() =>
            {
                OnCloseRequest();
            });

            minimizeCmd = ReactiveCommand.Create(() =>
            {
                WindowState = WindowState.Normal;
                WindowState = WindowState.Minimized;
            });

            profileMenuOpenCmd = ReactiveCommand.Create(() =>
            {
                IsProfileMenuOpen = true;
            });

            editUserCmd = ReactiveCommand.Create(() =>
            {
                Tab edit = new ScreenTab(this, new UserEdit(AppContext, AppContext.User));
                edit.Show();
                IsProfileMenuOpen = false;
            });

            quitCmd = ReactiveCommand.Create(() =>
            {                
                int index = TabsList.Count;
                while (index > 0)
                {
                    index = TabsList.Count - 1;
                    TabsList[index].Close();
                }

                AppContext.SocketApi.Disconnect();

                loginTab.Show();
                IsUserMenuVisible = false;
                IsProfileMenuOpen = false;
            });

            TabShownEvent += MainVM_TabShownEvent;
            #endregion

            #region registrationTab
            registrationTab = new registrationVM(this, AppContext);
            registrationTab.onUserRegistered += () =>
            {
                registrationTab.Close();
                //loginTab.Clear();
                loginTab.Show();
            };
            #endregion

            #region loginTab
            loginTab = new loginVM(this, AppContext);
            loginTab.onLoginDone += async (user) =>
            {

                User = user;
                IsUserMenuVisible = true;

                AppContext.User = user;
#if ONLINE
                AppContext.SocketApi.Connect(user.Token);
#endif

                homeVM homeTab = new homeVM(this, AppContext);
                homeTab.Menu.MenuExpandedEvent += Menu_MenuExpandedEvent;
                //homeTab.TabClosedEvent += (tab) =>
                //{
                //    loginTab.Password = "";
                //    loginTab.Show();
                //};
                loginTab.Close();
                homeTab.Show();
            };
            loginTab.onCreateUserAction += () =>
            {
                tokenTab.Show();
            };
            #endregion

            #region tokenTab
            tokenTab = new tokenVM(this, AppContext);
            //tokenTab.CloseTabEvent += CloseTab;
            tokenTab.onTokenCheckResult += (result, token) =>
            {

                if (result)
                {
                    CloseTab(tokenTab);
                    registrationTab.Token = token;
                    ShowTab(registrationTab);
                }
            };
            #endregion

            loginTab.Show();
        }

        #region callbacks
        private void MainVM_TabShownEvent(Tab tab)
        {
            IsStripVisible = tab is homeVM;
        }
        private void Menu_MenuExpandedEvent(bool expanded)
        {            
            IsStripVisible = expanded;
        }
        #endregion

        #region tabservice
        public ObservableCollection<Tab> TabsList { get; set; } = new ObservableCollection<Tab>();
        bool needRefresh { get; set; } = true;        

        object? content;
        public object? Content
        {
            get => content;
            set
            {
                ((Tab)content)?.OnDeactivate();
                this.RaiseAndSetIfChanged(ref content, value);
                ((Tab)content)?.OnActivate();

                TabShownEvent?.Invoke((Tab)content);
                //if (needRefresh)
                //{
                //    ((Tab)content)?.Refresh();
                //}
            }
        }

        int itemwidth;
        public int ItemWidth
        {
            get => itemwidth;
            set => this.RaiseAndSetIfChanged(ref itemwidth, value);
        }

        public void ShowTab(Tab tab)
        {
            var fTab = TabsList.FirstOrDefault(t => t.Title.Equals(tab.Title));
            needRefresh = false;

            if (fTab == null)
            {
                if (tab is homeVM)
                {

                    TabsList.Insert(0, tab);
                } else
                    TabsList.Add(tab);
             
                Content = tab;
            } else
            {
                Content = fTab;
            }

            needRefresh = true;

            //if (tab is homeVM)
            //    TabsList.Insert(0, tab);
            //else
            //    TabsList.Add(tab);

            //Content = tab;

        }

        public void AddTab(Tab tab)
        {
            var fTab = TabsList.FirstOrDefault(t => t.Title.Equals(tab.Title));
            if (fTab == null)
            {
                //tab.TabClosedEvent += CloseTab;                
                TabsList.Add(tab);
            }
        }

        public void CloseTab(Tab tab)
        {
            int index = TabsList.IndexOf(tab);
            if (index >= 1)
            {
                var prev = TabsList[index - 1];
                if (prev != null)
                    prev.Show();
            }
            TabsList.Remove(tab);
        }

        public event Action<Tab> TabShownEvent;
        #endregion

    }
}
