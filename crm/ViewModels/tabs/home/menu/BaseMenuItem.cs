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
    public abstract class BaseMenuItem : ViewModelBase
    {
        public ObservableCollection<BaseScreen> Screens { get; } = new ObservableCollection<BaseScreen>();
        public abstract string Title { get; }
        public abstract string IconPath { get; }

        public BaseMenuItem() {

            showInTabCmd = ReactiveCommand.Create(() => {

                Debug.WriteLine("showInTab");
            
            });
        
        }

        public void AddScreen(BaseScreen screen)        
        {
            screen.ScreenCheckedEvent += Screen_ScreenCheckedEvent;
            Screens.Add(screen);
        }

        private void Screen_ScreenCheckedEvent(BaseScreen s, bool v)
        {
            ScreenCheckedEvent?.Invoke(s, v);
        }

        public event Action<BaseScreen, bool> ScreenCheckedEvent;

        public ReactiveCommand<Unit, Unit> showInTabCmd { get; }

        
    }
}
