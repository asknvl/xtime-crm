using Avalonia.Threading;
using crm.Models.api.server;
using crm.Models.api.socket;
using crm.Models.appcontext;
using crm.Models.user;
using crm.ViewModels.dialogs;
using crm.ViewModels.tabs.home.screens.users;
using crm.WS;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace crm.ViewModels.tabs.home.screens
{
    public class UserList : BaseScreen
    {
        #region const        
        const int displayed_lines_num = 20;
        #endregion

        #region vars        
        IWindowService ws = WindowService.getInstance();
        IServerApi srvApi;
        ISocketApi sckApi;
        string token;
        #endregion

        #region properties       
        public override string Title => "Список сотрудников";

        //ObservableCollection<UserListItem> users = new();
        //public ObservableCollection<UserListItem> Users
        //{
        //    get => users;
        //    set => this.RaiseAndSetIfChanged(ref users, value);
        //}

        public ObservableCollection<UserListItem> Users { get; } = new();

        bool isAllChecked;
        public bool IsAllChecked
        {
            get => isAllChecked;
            set
            {
                foreach (var item in Users)
                    item.IsChecked = value;

                this.RaiseAndSetIfChanged(ref isAllChecked, value);
            }
        }

        int page = 1;
        public int SelectedPage
        {
            get => page;
            set
            {
                IsPrevActive = (value > 1);
                IsNextActive = (value < TotalPages);
                this.RaiseAndSetIfChanged(ref page, value);
            }
        }

        int totalPages = 1;
        public int TotalPages
        {
            get => totalPages;
            set
            {
                IsPrevActive = (SelectedPage > 1);
                IsNextActive = (SelectedPage < value || TotalPages == 0);
                this.RaiseAndSetIfChanged(ref totalPages, value);
            }
        }

        int pageSize = displayed_lines_num;
        public int PageSize
        {
            get => pageSize;
            set => this.RaiseAndSetIfChanged(ref pageSize, value);
        }

        bool isNextActive = true;
        public bool IsNextActive
        {
            get => isNextActive;
            set => this.RaiseAndSetIfChanged(ref isNextActive, value);
        }

        bool isPrevActive = true;
        public bool IsPrevActive
        {
            get => isPrevActive;
            set => this.RaiseAndSetIfChanged(ref isPrevActive, value);
        }

        string pageInfo;
        public string PageInfo
        {
            get => pageInfo;
            set => this.RaiseAndSetIfChanged(ref pageInfo, value);
        }

        bool allowUpdate { get; set; } = true;
        #endregion

        #region commands        
        public ReactiveCommand<Unit, Unit> addUserCmd { get; }
        public ReactiveCommand<Unit, Unit> nextPageCmd { get; }
        public ReactiveCommand<Unit, Unit> prevPageCmd { get; }
        public ReactiveCommand<object, Unit> sortParameterCmd { get; }
        #endregion

        public UserList() : base(new ApplicationContext())
        {
            Users = new();
            Users.Add(new UserItemTest(new ApplicationContext()));
        }

        public UserList(ApplicationContext context) : base(context)
        {

            srvApi = AppContext.ServerApi;
            token = AppContext.User.Token;
            sckApi = AppContext.SocketApi;
            sckApi.ReceivedConnectedUsersEvent += SckApi_ReceivedConnectedUsersEvent;
            sckApi.ReceivedUsersDatesEvent += SckApi_ReceivedUsersDatesEvent;


            SelectedPage = 1;

            #region commands
            addUserCmd = ReactiveCommand.Create(() =>
            {
                ws.ShowDialog(new rolesDlgVM(AppContext));
            });

            prevPageCmd = ReactiveCommand.CreateFromTask(async () =>
            {                   
                SelectedPage--;
                try
                {
                    //Users.Clear();
                    await updatePageInfo(SelectedPage, PageSize);
                } catch (Exception ex)
                {
                    ws.ShowDialog(new errMsgVM(ex.Message));
                }
            });

            nextPageCmd = ReactiveCommand.CreateFromTask(async () =>
            {                
                SelectedPage++;
                try
                {
                    //Users.Clear();
                    await updatePageInfo(SelectedPage, PageSize);
                } catch (Exception ex)
                {
                    ws.ShowDialog(new errMsgVM(ex.Message));
                }
            });

            sortParameterCmd = ReactiveCommand.Create<object>((o) => {

                ReadOnlyCollection<Object> c = o as ReadOnlyCollection<Object>;
                string name = (string)c[0];
                bool value = (bool)c[1];

                Debug.WriteLine($"{name}={value}");

            });

            //Task.Run(async () =>
            //{
            //    while (true)
            //    {
            //        await updatePageInfo(SelectedPage, PageSize);
            //        Thread.Sleep(10000);
                

            //    }
            //});
            #endregion
        }

        #region helpers
        string getPageInfo(int page, int users_count, int total_users)
        {
            int p = (users_count < displayed_lines_num) ? (page - 1) * displayed_lines_num + users_count : page * displayed_lines_num;
            return $"{(page - 1) * displayed_lines_num + 1}-{p} из {total_users}";
        }

        bool isRunning { get; set; }
        int storePage;

        async Task updatePageInfo(int page, int pagesize)
        {
#if OFFLINE
            Users.Clear();
            Users.Add(new UserItemTest(AppContext) { FullName = "Иванов Иван Иванович" });
            Users.Add(new UserItemTest(AppContext) { FullName = "Петров Петр Петрович" });
#elif ONLINE

            await Task.Run(async () =>
            {            
                
                List<User> users;
                int total_users;

                (users, TotalPages, total_users) = await srvApi.GetUsers(page - 1, pagesize, token);

                PageInfo = getPageInfo(page, users.Count, total_users);

                await Dispatcher.UIThread.InvokeAsync(() =>
                {

                    if (storePage != page)
                        Users.Clear();

                    storePage = page;

                    foreach (var user in users)
                    {                 
                        var found = Users.FirstOrDefault(u => u.Id.Equals(user.Id));
                        if (found != null)
                        {
                            found.Copy(user);
                        } else
                        {
                            var tmp = new UserListItem(AppContext);
                            tmp.Copy(user);
                            Users.Add(tmp);
                        }
                    }
                });

            });

            sckApi.RequestConnectedUsers();

#endif
        }
        #endregion

        #region override
        public override async void OnActivate()
        {
            base.OnActivate();

            try
            {
                await updatePageInfo(SelectedPage, PageSize);
            } catch (OperationCanceledException ex)
            {
            } catch (Exception ex)
            {
                ws.ShowDialog(new errMsgVM(ex.Message));
            }

        }       
        #endregion
        
        #region callbacks
        private void SckApi_ReceivedConnectedUsersEvent(List<usersOnlineDTO> connectedUsers)
        {
            try
            {
                foreach (var connected in connectedUsers)
                {
                    var user = Users.FirstOrDefault(u => u.Id.Equals(connected.user_id));
                    if (user != null)
                        user.Status = connected.connected;
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
        }

        private void SckApi_ReceivedUsersDatesEvent(usersDatesDTO dates)
        {
            BaseUser user = Users?.FirstOrDefault(u => u.Id.Equals(dates.user_id));
            if (user != null)
            {
                user.LastLoginDate = dates.last_login_date;
                user.LastEventDate = dates.last_event_date;
            }
        }

        public override void OnDeactivate()
        {
            //sckApi.ReceivedConnectedUsersEvent -= SckApi_ReceivedConnectedUsersEvent;
            //sckApi.ReceivedUsersDatesEvent -= SckApi_ReceivedUsersDatesEvent;
            base.OnDeactivate();            
        }
        #endregion
    }
}
