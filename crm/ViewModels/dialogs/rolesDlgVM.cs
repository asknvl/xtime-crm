using Avalonia.Controls.Selection;
using crm.Models.api.server;
using crm.Models.appcontext;
using crm.Models.user;
using crm.ViewModels.Helpers;
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
using TextCopy;

namespace crm.ViewModels.dialogs
{
    public class rolesDlgVM : BaseDialog
    {
        #region vars
        IWindowService ws = WindowService.getInstance();
        TagsAndRolesConvetrer convetrer = new();
        #endregion

        #region properties
        bool isValidSelection = false;
        public bool IsValidSelection
        {
            get => isValidSelection;
            set => this.RaiseAndSetIfChanged(ref isValidSelection, value);
        }
        public ObservableCollection<tagsListItem> Tags { get; } = new();
        public List<tagsListItem> SelectedTags { get; } = new();
        public SelectionModel<tagsListItem> Selection { get; }       
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> cancelCmd { get; }
        public ReactiveCommand<Unit, Unit> acceptCmd { get; }
        #endregion

        public rolesDlgVM()
        {
            IsValidSelection = false;
            Tags = convetrer.GetAllTags();
        }

        public rolesDlgVM(ApplicationContext appcontext)
        {
            IServerApi api = appcontext.ServerApi;
            string token = appcontext.User.Token;

            Selection = new SelectionModel<tagsListItem>();
            Selection.SingleSelect = false;
            Selection.SelectionChanged += Selection_SelectionChanged;

            Tags = convetrer.GetAllTags();

            #region commands    
            cancelCmd = ReactiveCommand.Create(() => {
                OnCloseRequest();
            });

            acceptCmd = ReactiveCommand.CreateFromTask(async () => {

                var roles = convetrer.TagsToRoles(SelectedTags);

                string newtoken = "";
                try
                {
                    newtoken = await api.GetNewUserToken(roles, token);

                    OnCloseRequest();

                } catch (Exception ex)
                {
                    ws.ShowDialog(new errMsgVM(ex.Message));
                }

                Clipboard clipboard = new Clipboard();
                try
                {
                    clipboard.SetText(newtoken);

                    appcontext.BottomPopup.Show("Токен скопирован");

                } catch { 
                }



            });
            #endregion
        }

        private void Selection_SelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs<tagsListItem> e)
        {

            foreach (var item in e.SelectedItems)
            {
                SelectedTags.Add(item);
            }

            foreach (var item in e.DeselectedItems)
            {                
                SelectedTags.Remove(item);
            }

            bool isTeamLead = SelectedTags.Any( t => t.Name.Equals(Role.teamlead));
            bool isAdmin = SelectedTags.Any(t => t.Name.Equals(Role.admin));
            bool isAnyOne = SelectedTags.Any(t => !t.Name.Equals(Role.teamlead) && !t.Name.Equals(Role.admin));

            IsValidSelection = (isAdmin && !isTeamLead ) || (isTeamLead && isAnyOne) || isAnyOne;

        }
    }
}
