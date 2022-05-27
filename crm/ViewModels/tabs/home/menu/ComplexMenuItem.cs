using Avalonia.Media;
using crm.ViewModels.tabs.home.screens;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.menu
{
    public abstract class ComplexMenuItem : BaseMenuItem
    {
        #region properties
        bool NeedInvoke { get; set; } = true;

        bool isItemExpanded;
        public bool IsItemExpanded
        {
            get => isItemExpanded;
            set
            {
                
                if (value)
                {
                    if (!Screens[0].IsChecked)
                        Screens[0].IsChecked = true;
                }

                this.RaiseAndSetIfChanged(ref isItemExpanded, value);
                if (value && NeedInvoke)
                    IsItemExpandedEvent?.Invoke();
            }
        }

        IBrush expanderBackground;
        public IBrush ExpanderBackground
        {
            get => expanderBackground;
            set => this.RaiseAndSetIfChanged(ref expanderBackground, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> expanderClickCmd { get; }
        #endregion

        public ComplexMenuItem()
        {
            ExpanderBackground = Brushes.Transparent;
            expanderClickCmd = ReactiveCommand.Create(() => {                                    
            });
        }

        #region public
        public void Expand()
        {            
            NeedInvoke = false;          
            IsItemExpanded = true;
            NeedInvoke = true;
        }
        public void Collapse()
        {
            IsItemExpanded = false;
        }

        public void SetExpanderSelected(bool state)
        {
            if (state)
                ExpanderBackground = new SolidColorBrush(0xFFDAEBF7);
            else
                ExpanderBackground = new SolidColorBrush(0x00000000);
        }
        #endregion

        #region callbacks
        public event Action? IsItemExpandedEvent;
        #endregion
    }
}
