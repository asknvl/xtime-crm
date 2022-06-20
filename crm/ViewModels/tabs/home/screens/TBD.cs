using crm.Models.appcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens
{
    public class TBD : BaseScreen
    {
        public TBD(string title) : base()
        {
            Title = title;
        }

        public override string Title { get; set; }
    }
}
