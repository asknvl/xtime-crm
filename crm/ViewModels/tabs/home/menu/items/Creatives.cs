using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.menu.items
{
    public class Creatives : SimpleMenuItem
    {
        public override string Title => "Креативы";
        public override string IconPath => "/Assets/svgs/leftslidemenu/creo.svg";

        public Creatives() { }
    }
}
