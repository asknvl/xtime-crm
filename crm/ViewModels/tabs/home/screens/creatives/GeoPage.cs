using Avalonia.Threading;
using crm.Models.api.server;
using crm.Models.api.socket;
using crm.Models.creatives;
using crm.ViewModels.dialogs;
using crm.WS;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using geo = crm.Models.geoservice;

namespace crm.ViewModels.tabs.home.screens.creatives
{
    public class GeoPage : BaseGeoPage
    {
        #region vars        
        IWindowService ws = WindowService.getInstance();
        public List<CreativeItem> CheckedCreatives = new();
        string SortKey = "+id";
        string token;
        IServerApi server;
        Dictionary<int, List<CreativeItem>> creativeListDictionary = new();

        List<creativeChangedDTO> changeList = new();
        System.Timers.Timer updateTimer;
        #endregion

        #region properties
        CreativeServerDirectory creativeServerDirectory;
        public CreativeServerDirectory CreativeServerDirectory
        {
            get => creativeServerDirectory;
            set => this.RaiseAndSetIfChanged(ref creativeServerDirectory, value);
        }

        public ObservableCollection<CreativeItem> CreativesList { get; set; } = new();

        bool needInvokeAllCheck { get; set; } = true;
        bool isAllChecked;
        public bool IsAllChecked
        {
            get => isAllChecked;
            set
            {
                if (needInvokeAllCheck)
                {
                    foreach (var item in CreativesList)
                        item.IsChecked = value;

                    if (!value)
                    {
                        CheckedCreatives.Clear();
                        CreativesSelectionChangedEvent?.Invoke(0);
                    }
                }
                this.RaiseAndSetIfChanged(ref isAllChecked, value);
            }
        }

        bool needMassUniqalization;
        public bool NeedMassUniqalization
        {
            get => needMassUniqalization;
            set
            {
                foreach (var creative in CreativesList)
                {
                    foreach (var item in CreativesList)
                    {
                        item.IsChecked = value;
                        item.Uniques = (value) ? Uniques : 0;
                    }
                }
                this.RaiseAndSetIfChanged(ref needMassUniqalization, value);
            }
        }

        int uniques = 0;
        public int Uniques
        {
            get => uniques;
            set
            {
                this.RaiseAndSetIfChanged(ref uniques, value);
                if (NeedMassUniqalization)
                {
                    foreach (var creative in CreativesList)
                    {
                        if (creative.IsChecked)
                            creative.Uniques = Uniques;
                    }
                }
            }
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> nextPageCmd { get; }
        public ReactiveCommand<Unit, Unit> prevPageCmd { get; }
        #endregion

        public GeoPage(CreativeServerDirectory dir) : base()
        {

            updateTimer = new System.Timers.Timer();
            updateTimer.Elapsed += UpdateTimer_Elapsed;
            updateTimer.Interval = 5000;
            updateTimer.AutoReset = true;
            updateTimer.Start();

            CreativeServerDirectory = dir;
            Title = dir.dir;

            server = AppContext.ServerApi;
            token = AppContext.User.Token;
            AppContext.Settings.SettingChangedEvent += Settings_SettingChangedEvent;

            SelectedPage = 1;

            #region commands
            prevPageCmd = ReactiveCommand.CreateFromTask(async () =>
            {
                SelectedPage--;
                try
                {
                    //Users.Clear();
                    await updatePageInfo(SelectedPage, PageSize, SortKey);
                }
                catch (Exception ex)
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
                    await updatePageInfo(SelectedPage, PageSize, SortKey);
                }
                catch (Exception ex)
                {
                    ws.ShowDialog(new errMsgVM(ex.Message));
                }
            });

            AppContext.SocketApi.ReceivedCreativeChangedEvent += SocketApi_ReceivedCreativeChangedEvent;
            #endregion

        }

        private async void Settings_SettingChangedEvent(string property, object value)
        {
            if (property.Equals("CreativesPerPage"))
            {
                try
                {
                    await updatePageInfo(SelectedPage, PageSize, SortKey);
                }
                catch (Exception ex)
                {
                    ws.ShowDialog(new errMsgVM(ex.Message));
                }
            }
        }

        private async void UpdateTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            ToogleUpdate(false);
            Debug.WriteLine($"{creativeServerDirectory.dir} TIMER");
            if (changeList.Count > 0)
            {
                Debug.WriteLine($"{creativeServerDirectory.dir} IS NOTIFY");
                changeList.Clear();
                await updatePageInfo(SelectedPage, PageSize, SortKey);
            }
            ToogleUpdate(true);
        }

        private async void SocketApi_ReceivedCreativeChangedEvent(Models.api.socket.creativeChangedDTO chdto)
        {
            //TODO фильтр по айди директории

            //await updatePageInfo(SelectedPage, PageSize, SortKey);

            if (chdto.filepath.Contains(CreativeServerDirectory.dir))
            {
                Debug.WriteLine($"{creativeServerDirectory.dir} SOCKET");

                if (changeList.Count == 0)
                {
                    changeList.Add(chdto);
                    Debug.WriteLine($"{creativeServerDirectory.dir} ADD");
                }
            }

        }

        int prevPageSize = 1;
        Task getT(int page, int pagesize, string sortkey)
        {
            return new Task(() =>
             {

                 prevPageSize = PageSize;

                 Dispatcher.UIThread.InvokeAsync(() =>
                 {
                     CreativesList.Clear();
                 });

#if ONLINE
                 int total_pages = 0;
                 int total_creatives = 0;
                 List<CreativeDTO> crdtos;

                 var roles = AppContext.User.Roles;
                 bool? showinvisible = roles.Any(x => x.Type == Models.user.RoleType.admin || x.Type == Models.user.RoleType.creative) ? null : true;

                 (crdtos, TotalPages, total_creatives) = AppContext.ServerApi.GetAvaliableCreatives(token, page - 1, pagesize, CreativeServerDirectory, (int)CreativeType.video, showinvisible);

                 PageInfo = getPageInfo(SelectedPage, crdtos.Count, total_creatives);

                 //int total_in_dictionary = 0;
                 //foreach (var item in creativeListDictionary)
                 //    total_in_dictionary += item.Value.Count;

                 //if (total_in_dictionary < total_creatives)
                 //    creativeListDictionary.Clear();

                 //if (!creativeListDictionary.ContainsKey(SelectedPage))
                 //    creativeListDictionary.Add(SelectedPage, new List<CreativeItem>());

                 IsPrevActive = false;
                 IsNextActive = false;


                 foreach (var cdt in crdtos)
                 {
                     var found = CreativesList.FirstOrDefault(o => o.Id == cdt.id);

                     //CreativeItem found = null;
                     //if (creativeListDictionary.ContainsKey(SelectedPage))
                     //{
                     //    var list = creativeListDictionary[SelectedPage];
                     //    found = list.FirstOrDefault(o => o.Id == cdt.id);
                     //}

                     if (found == null)
                     {

                         CreativeItem creative = new CreativeItem(cdt, CreativeServerDirectory);

                         if (creative.IsUploaded)
                         {

                             Dispatcher.UIThread.InvokeAsync(() =>
                             {
                                 creative.CheckedEvent -= Creative_CheckedEvent;
                                 creative.CheckedEvent += Creative_CheckedEvent;
                                 creative.IsChecked = CheckedCreatives.Any(u => u.Id.Equals(creative.Id)) || IsAllChecked;
                                 CreativesList.Add(creative);
                                 //creativeListDictionary[SelectedPage].Add(creative);

                             });

                             //await Task.Run(() => { creative.Synchronize(); });
                             creative.SynchronizeAsync().Wait();

                         }
                         else
                         {

                         }


                     }
                 }

                 IsPrevActive = true;
                 IsNextActive = true;

                 //foreach (var creative in creativeListDictionary[SelectedPage])
                 //{
                 //    await Dispatcher.UIThread.InvokeAsync(() =>
                 //    {
                 //        CreativesList.Add(creative);
                 //    });
                 //}
#else
#endif

             });

            //return new Task(async () => {

            //    int cntr = 0;

            //    await Task.Run(() => { 

            //        while (cntr < 100)
            //        {
            //            Debug.WriteLine(CreativeServerDirectory.dir + " " + cntr);
            //            Thread.Sleep(100);
            //            cntr++;
            //        }
            //    });

            //});

        }


        Dictionary<int, Task> tasks = new Dictionary<int, Task>();


        #region helpers
        async Task updatePageInfo(int page, int pagesize, string sortkey)
        {

            //            Task t = new Task(async () =>
            //            {

            //                await Dispatcher.UIThread.InvokeAsync(() =>
            //                {
            //                    CreativesList.Clear();
            //                });

            //#if ONLINE
            //                int total_pages = 0;
            //                int total_creatives = 0;
            //                List<CreativeDTO> crdtos;

            //                var roles = AppContext.User.Roles;
            //                bool? showinvisible = roles.Any(x => x.Type == Models.user.RoleType.admin || x.Type == Models.user.RoleType.creative) ? null : true;

            //                (crdtos, TotalPages, total_creatives) = await AppContext.ServerApi.GetAvaliableCreatives(token, page - 1, pagesize, CreativeServerDirectory, (int)CreativeType.video, showinvisible);

            //                PageInfo = getPageInfo(SelectedPage, crdtos.Count, total_creatives);

            //                //int total_in_dictionary = 0;
            //                //foreach (var item in creativeListDictionary)
            //                //    total_in_dictionary += item.Value.Count;

            //                //if (total_in_dictionary < total_creatives)
            //                //    creativeListDictionary.Clear();

            //                //if (!creativeListDictionary.ContainsKey(SelectedPage))
            //                //    creativeListDictionary.Add(SelectedPage, new List<CreativeItem>());

            //                IsPrevActive = false;
            //                IsNextActive = false;


            //                foreach (var cdt in crdtos)
            //                {
            //                    var found = CreativesList.FirstOrDefault(o => o.Id == cdt.id);

            //                    //CreativeItem found = null;
            //                    //if (creativeListDictionary.ContainsKey(SelectedPage))
            //                    //{
            //                    //    var list = creativeListDictionary[SelectedPage];
            //                    //    found = list.FirstOrDefault(o => o.Id == cdt.id);
            //                    //}

            //                    if (found == null)
            //                    {

            //                        CreativeItem creative = new CreativeItem(cdt, CreativeServerDirectory);

            //                        if (creative.IsUploaded)
            //                        {

            //                            await Dispatcher.UIThread.InvokeAsync(() =>
            //                            {
            //                                creative.CheckedEvent -= Creative_CheckedEvent;
            //                                creative.CheckedEvent += Creative_CheckedEvent;
            //                                creative.IsChecked = CheckedCreatives.Any(u => u.Id.Equals(creative.Id)) || IsAllChecked;
            //                                CreativesList.Add(creative);
            //                                //creativeListDictionary[SelectedPage].Add(creative);

            //                            });

            //                            //await Task.Run(() => { creative.Synchronize(); });
            //                            await creative.SynchronizeAsync();

            //                        } else
            //                        {

            //                        }


            //                    }
            //                }

            //                IsPrevActive = true;
            //                IsNextActive = true;

            //                //foreach (var creative in creativeListDictionary[SelectedPage])
            //                //{
            //                //    await Dispatcher.UIThread.InvokeAsync(() =>
            //                //    {
            //                //        CreativesList.Add(creative);
            //                //    });
            //                //}
            //#else
            //#endif

            //            });

            if (!tasks.ContainsKey(page))
            {
                var t = getT(page, pagesize, sortkey);
                tasks.Add(page, t);
                t.Start();
            }
            else
            {
                if (!tasks[page].IsCompleted)
                {

                    Debug.WriteLine("IsRUnning");

                }
                else
                {
                    var t = getT(page, pagesize, sortkey);
                    tasks[page] = t;
                    t.Start();
                }
            }

        }
        #endregion

        #region public  
        public void ToogleUpdate(bool state)
        {
            updateTimer.Enabled = state;
        }
        #endregion

        #region callbacks
        private void Creative_CheckedEvent(CreativeItem creative, bool ischecked)
        {

            if (!ischecked && IsAllChecked)
            {
                needInvokeAllCheck = false;
                IsAllChecked = false;
                needInvokeAllCheck = true;
            }

            var found = CheckedCreatives.FirstOrDefault(o => o.Id.Equals(creative.Id));

            if (ischecked)
            {
                if (found == null)
                    CheckedCreatives.Add(creative);
            }
            else
            {
                if (found != null)
                    CheckedCreatives.Remove(found);
            }

            CreativesSelectionChangedEvent?.Invoke(CheckedCreatives.Count);
        }
        #endregion

        #region events
        public event Action<int> CreativesSelectionChangedEvent;
        #endregion

        #region override    
        public override async void OnActivate()
        {
            base.OnActivate();

            CreativesSelectionChangedEvent?.Invoke(CheckedCreatives.Count);

            try
            {
                await updatePageInfo(SelectedPage, PageSize, SortKey);
            }
            catch (Exception ex)
            {
                ws.ShowDialog(new errMsgVM(ex.Message));
            }
        }
        #endregion
    }
}
