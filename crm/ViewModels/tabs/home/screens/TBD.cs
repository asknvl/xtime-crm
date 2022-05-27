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
        public TBD(ApplicationContext context, string title) : base(context)
        {
            Title = title;
        }

        public override string Title { get; }
    }
}
