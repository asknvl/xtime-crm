using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.menu.items
{
    internal class Subscriptions : SimpleMenuItem
    {
        public override string Title => "Подписки";
        public override string IconPath => "/Assets/svgs/leftslidemenu/subscriptions.svg";
    }
}
