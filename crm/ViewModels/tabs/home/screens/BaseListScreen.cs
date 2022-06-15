using crm.Models.appcontext;
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

namespace crm.ViewModels.tabs.home.screens
{
    public abstract class BaseListScreen<T> : BaseScreen
    {
        #region const        
        const int displayed_lines_num = 20;
        #endregion

        #region vars
        IWindowService ws = WindowService.getInstance();
        #endregion

        #region properties
        public ObservableCollection<ICheckable<T>> ListOfItems { get; } = new();

        bool isAllChecked;
        public bool IsAllChecked
        {
            get => isAllChecked;
            set
            {
                foreach (var item in ListOfItems)
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
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> nextPageCmd { get; }
        public ReactiveCommand<Unit, Unit> prevPageCmd { get; }
        public ReactiveCommand<object, Unit> sortParameterCmd { get; }
        #endregion

        public BaseListScreen() : base()
        {
            #region commands
            prevPageCmd = ReactiveCommand.CreateFromTask(async () =>
            {
                SelectedPage--;
                try
                {
                    //Users.Clear();
                    await updatePageInfo(SelectedPage, PageSize);
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
                    await updatePageInfo(SelectedPage, PageSize);
                }
                catch (Exception ex)
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
            #endregion
        }

        #region helpers
        protected string getPageInfo(int page, int items_count, int total_items)
        {
            int p = (items_count < displayed_lines_num) ? (page - 1) * displayed_lines_num + items_count : page * displayed_lines_num;
            return $"{(page - 1) * displayed_lines_num + 1}-{p} из {total_items}";
        }
        #endregion

        #region protected
        /// <summary>
        /// Update list items routine, nust implement PageInfo = getPageInfo
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        protected abstract Task updatePageInfo(int page, int pagesize);
        #endregion
    }
}
