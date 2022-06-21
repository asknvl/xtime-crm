using Avalonia.Threading;
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
        List<CreativeItem> checkedCreatives = new();
        string SortKey = "+id";
        #endregion

        #region properties
        geo.GEO geo;
        public geo.GEO GEO
        {
            get => geo;
            set => this.RaiseAndSetIfChanged(ref geo, value);
        }
        public ObservableCollection<CreativeItem> CreativesList { get; } = new();

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
                        checkedCreatives.Clear();
                        CreativesSelectionChangedEvent?.Invoke(0);
                    }
                }
                this.RaiseAndSetIfChanged(ref isAllChecked, value);
            }
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> nextPageCmd { get; }
        public ReactiveCommand<Unit, Unit> prevPageCmd { get; }
        #endregion

        public GeoPage(geo.GEO g)
        {
            GEO = g;
            Title = GEO.Code;

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
            #endregion

        }

        #region helpers
        async Task updatePageInfo(int page, int pagesize, string sortkey)
        {
            await Task.Run(async () => {

                await Dispatcher.UIThread.InvokeAsync(() => {
                    CreativesList.Clear();
                });

                for (int i = 0; i < 10; i++)
                {
                    var c = new CreativeItem() { GEO = GEO, Id = i, Name = $"{GEO.Code}{i}" };
                    c.CheckedEvent += Creative_CheckedEvent;
                    c.IsChecked = checkedCreatives.Any(u => u.Id.Equals(c.Id)) || IsAllChecked;

                    await Dispatcher.UIThread.InvokeAsync(() => {
                        CreativesList.Add(c);
                    });
                }

                PageInfo = getPageInfo(SelectedPage, 10, 10);

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

            var found = checkedCreatives.FirstOrDefault(o => o.Id.Equals(creative.Id));

            if (ischecked)
            {
                if (found == null)
                    checkedCreatives.Add(creative);
            }
            else
            {
                if (found != null)
                    checkedCreatives.Remove(found);
            }

            CreativesSelectionChangedEvent?.Invoke(checkedCreatives.Count);
        }
        #endregion

        #region events
        public event Action<int> CreativesSelectionChangedEvent;
        #endregion

        #region override    
        public override async void OnActivate()
        {
            base.OnActivate();

            CreativesSelectionChangedEvent?.Invoke(checkedCreatives.Count);

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
