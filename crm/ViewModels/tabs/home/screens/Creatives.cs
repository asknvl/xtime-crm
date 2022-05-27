using crm.Models.appcontext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens
{
    public class Creatives : BaseScreen
    {
        public Creatives(ApplicationContext context) : base(context)
        {
        }

        public override string Title => "Креативы";
    }
}
