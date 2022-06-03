using crm.Models.appcontext;
using crm.Models.user;
using crm.ViewModels.dialogs;
using crm.WS;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using TextCopy;

namespace crm.ViewModels.tabs.home.screens.users
{
    public class UserListItem : BaseUser, ICheckable<UserListItem> 
    {
        #region vars
        IWindowService ws = WindowService.getInstance();
        #endregion

        #region properties        
        bool isChecked;
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                this.RaiseAndSetIfChanged(ref isChecked, value);
                CheckedEvent?.Invoke(this, value);
            }
        }

        bool status;
        public bool Status
        {
            get => status;
            set => this.RaiseAndSetIfChanged(ref status, value);
        }        
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> showTagsCmd { get; }
        public ReactiveCommand<Unit, Unit> editUserCmd { get; }
        public ReactiveCommand<Unit, Unit> openTelegram { get; set; }
        public ReactiveCommand<string, Unit> copyCmd { get; set; }
        #endregion

        public UserListItem(ApplicationContext appcontext)
        {
            #region commands
            showTagsCmd = ReactiveCommand.CreateFromTask(async () => {
                tagsDlgVM tags = new tagsDlgVM(Roles);
                ws.ShowDialog(tags);               
            });

            editUserCmd = ReactiveCommand.Create(() => {
                ScreenTab editTab = new ScreenTab(appcontext.TabService, new UserEdit(appcontext, this));
                editTab.Show();
            });

            openTelegram = ReactiveCommand.Create(() => {
                Process.Start(new ProcessStartInfo
                {
                    FileName = $"tg://resolve?domain={Telegram.Replace("@", "")}",
                    UseShellExecute = true
                });
            });

            copyCmd = ReactiveCommand.Create<string>((o) => {                
                Clipboard clipboard = new Clipboard();
                clipboard.SetText(o);
                appcontext.BottomPopup.Show("Значение скопировано");
            });
            #endregion
        }

        public event Action<UserListItem, bool> CheckedEvent;
    }
}
