using crm.Models.user;
using crm.ViewModels.Helpers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.dialogs
{    
    public class tagsDlgVM : BaseDialog
    {
        #region vars
        TagsAndRolesConvetrer convetrer = new();
        #endregion

        #region properties        
        public ObservableCollection<tagsListItem> Tags { get; } = new ObservableCollection<tagsListItem>();
        public List<tagsListItem> SelectedTags { get; }
        #endregion

        public tagsDlgVM()
        {
            Tags = convetrer.GetAllTags();
        }

        public tagsDlgVM(List<Role> roles)
        {
            Tags = convetrer.GetAllTags();
            SelectedTags = convetrer.RolesToTags(roles);
        }
    }
}
