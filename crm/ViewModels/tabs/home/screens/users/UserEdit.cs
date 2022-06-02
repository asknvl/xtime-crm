using crm.Models.appcontext;
using crm.Models.user;
using crm.ViewModels.dialogs;
using crm.Views.dialogs;
using crm.WS;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens.users
{
    public class UserEdit : BaseScreen
    {
        #region vars
        editUserInfo editUser;
        editUserDocuments editUserDocuments;

        IWindowService ws = WindowService.getInstance();
        BaseUser user;
        #endregion

        #region properties
        public ObservableCollection<BaseScreen> EditActions { get; }

        //public override string Title => $"{AppContext.User.FirstName} {AppContext.User.LastName}";
        public override string Title => $"{user.FirstName} {user.LastName}";

        Object content;
        public Object Content
        {
            get => content;
            set => this.RaiseAndSetIfChanged(ref content, value);
        }

        bool needCancel { get; set; } = true;

        bool isEditChecked;
        public bool IsEditChecked
        {
            get => isEditChecked;
            set
            {
                ((ICommandActions)Content).IsEditable = value;
                if (!value && needCancel)
                    ((ICommandActions)Content).Cancel();
                this.RaiseAndSetIfChanged(ref isEditChecked, value);
            }
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> confirmCmd { get; }
        #endregion

        public UserEdit() : base(new ApplicationContext() { User = new TestUser() })
        {
            EditActions = new ObservableCollection<BaseScreen>();

            EditActions.Add(new editUserInfo(AppContext, new TestUser()));
            EditActions.Add(new editUserDevices(AppContext));
            EditActions.Add(new editUserDocuments(AppContext));
            Content = EditActions[0];
        }
        public UserEdit(ApplicationContext context, BaseUser user) : base(context)
        {

            //this.user = user;
            Task.Run(async () => {
                this.user = await AppContext.ServerApi.GetUser(user.Id, AppContext.User.Token);
            }).Wait();

            confirmCmd = ReactiveCommand.CreateFromTask(async () => {
                try
                {
                    bool res = await ((ICommandActions)Content).Save();
                    needCancel = false;
                    IsEditChecked = !res;
                    needCancel = true;
                } catch (Exception ex)
                {
                    ws.ShowDialog(new errMsgVM(ex.Message));
                }
            });

            EditActions = new ObservableCollection<BaseScreen>();

            EditActions.Add(new editUserInfo(AppContext, this.user));
            EditActions.Add(new editUserDevices(AppContext));
            EditActions.Add(new editUserDocuments(AppContext));

            Content = EditActions[0];
        }
    }
}
