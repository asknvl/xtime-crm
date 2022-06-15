using crm.Models.appcontext;
using crm.ViewModels.tabs.home.screens.creatives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens
{
    public class Creatives : BaseScreen
    {
        #region properties
        public ObservableCollection<BaseCreative> CreativesList { get; }
        #endregion
        public Creatives() : base(new ApplicationContext())
        {

        }

        public Creatives(ApplicationContext context) : base(context)
        {
        }

        public override string Title => "Креативы";
    }
}
