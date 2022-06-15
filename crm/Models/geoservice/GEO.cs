using crm.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.geoservice
{
    public class GEO : ViewModelBase
    {
        #region properties
        string name;
        public string Name {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value.ToUpper());
        }
        #endregion

        public GEO(string name)
        {
            Name = name;
        }

    }
}
