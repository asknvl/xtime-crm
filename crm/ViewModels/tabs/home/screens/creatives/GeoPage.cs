using Avalonia.Threading;
using crm.Models.api.server;
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
using System.Threading.Tasks;
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
            set {
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
            set { 
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
            CreativeServerDirectory = dir;
            Title = dir.dir;

            server = AppContext.ServerApi;
            token = AppContext.User.Token;

            SelectedPage = 1;

            #region commands
            prevPageCmd = ReactiveCommand.CreateFromTask(async () =>
            {
                SelectedPage--;
                try
                {
                    //Users.Clear();
                    await updatePageInfo(SelectedPage, PageSize, SortKey);
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
                    await updatePageInfo(SelectedPage, PageSize, SortKey);
                } catch (Exception ex)
                {
                    ws.ShowDialog(new errMsgVM(ex.Message));
                }
            });

            AppContext.SocketApi.ReceivedCreativeChangedEvent += SocketApi_ReceivedCreativeChangedEvent;
            #endregion

        }

        private async void SocketApi_ReceivedCreativeChangedEvent(Models.api.socket.creativeChangedDTO obj)
        {
            await updatePageInfo(SelectedPage, PageSize, SortKey);
        }

        #region helpers
        async Task updatePageInfo(int page, int pagesize, string sortkey)
        {
           
            await Task.Run(async () =>
            {

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    CreativesList.Clear();
                });

#if ONLINE
                int total_pages = 0;
                int total_creatives = 0;
                List<CreativeDTO> crdtos;

                //var roles = AppContext.User.Roles;
                //bool? showinvisible = roles.Any(x => x.Type == Models.user.RoleType.admin || x.Type == Models.user.RoleType.creative) ? null : true;

                (crdtos, TotalPages, total_creatives) = await AppContext.ServerApi.GetAvaliableCreatives(token, page - 1, pagesize, CreativeServerDirectory, (int)CreativeType.video, null);

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

                            await Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                creative.CheckedEvent -= Creative_CheckedEvent;
                                creative.CheckedEvent += Creative_CheckedEvent;
                                creative.IsChecked = CheckedCreatives.Any(u => u.Id.Equals(creative.Id)) || IsAllChecked;
                                CreativesList.Add(creative);
                                //creativeListDictionary[SelectedPage].Add(creative);

                            });

                            //await Task.Run(() => { creative.Synchronize(); });

                            await creative.SynchronizeAsync();
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

          
        }
        #endregion

        #region public  
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
            } else
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
            } catch (Exception ex)
            {
                ws.ShowDialog(new errMsgVM(ex.Message));
            }
        }
        #endregion
    }
}
