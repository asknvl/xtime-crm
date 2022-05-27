using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.dialogs
{
    public class tagsListItem : ViewModelBase
    {
        string name;
        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }

        public tagsListItem(string name)
        {
            Name = name;
        }
    }
}
