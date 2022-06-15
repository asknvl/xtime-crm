using crm.Models.appcontext;
using crm.ViewModels.tabs.home.screens;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.menu
{
    public abstract class BaseMenu : ViewModelBase
    {        
        #region properties
        public ObservableCollection<BaseMenuItem> Items { get; set; } = new ObservableCollection<BaseMenuItem>();

        bool isMenuExpanded;
        public bool IsMenuExpanded
        {
            get => isMenuExpanded;
            set
            {
                this.RaiseAndSetIfChanged(ref isMenuExpanded, value);
                MenuExpandedEvent?.Invoke(value);
            }
        }

        BaseScreen screen;
        public BaseScreen Screen
        {
            get => screen;
            set
            {
                screen?.OnDeactivate();                
                this.RaiseAndSetIfChanged(ref screen, value);
                value.OnActivate();
            }
        }
        #endregion

        #region protected
        protected void Menu_ExpandedEvent(bool expanded)
        {
            if (expanded)
            {
                foreach (var item in Items)
                {
                    if (item is ComplexMenuItem)
                    {
                        ComplexMenuItem citem = (ComplexMenuItem)item;
                        bool chkd = citem.Screens.Any(o => o.IsChecked);
                        if (chkd)
                            citem.SetExpanderSelected(false);
                    }
                }
                var itemToExpand = Items.FirstOrDefault(o => o is ComplexMenuItem && o.Screens.Any(s => s.IsChecked));
                if (itemToExpand != null)
                    ((ComplexMenuItem)itemToExpand).Expand();

                return;
            }
            foreach (var item in Items)
            {
                if (item is ComplexMenuItem)
                {
                    ComplexMenuItem citem = (ComplexMenuItem)item;
                    citem.Collapse();
                    bool chkd = citem.Screens.Any(o => o.IsChecked);
                    if (chkd)
                        citem.SetExpanderSelected(true);
                }
            }
        }

        protected void Item_ExpandedEvent()
        {
            IsMenuExpanded = true;
        }

        protected void Screen_CheckedEvent(BaseScreen s, bool v)
        {

            foreach (var item in Items)
            {
                if (item is ComplexMenuItem)
                {
                    ((ComplexMenuItem)item).SetExpanderSelected(false);
                }

                foreach (var screen in item.Screens)
                    if (!screen.Equals(s))
                        screen.Uncheck();
            }

            Screen = s;            

        }
        #endregion

        public BaseMenu()
        {
            IsMenuExpanded = true;
            MenuExpandedEvent += Menu_ExpandedEvent;
        }

        #region public
        public void AddItem(BaseMenuItem item)
        {
            item.ScreenCheckedEvent += Screen_CheckedEvent;

            if (item is ComplexMenuItem)
            {
                ((ComplexMenuItem)item).IsItemExpandedEvent += Item_ExpandedEvent;
            }

            Items.Add(item);
        }
        #endregion

        #region callbacks
        public event Action<bool>? MenuExpandedEvent;
        #endregion

    }
}
