using crm.Models.api.server;
using crm.Models.api.socket;
using crm.Models.appsettings;
using crm.Models.user;
using crm.ViewModels.popups;
using crm.ViewModels.tabs.tabservice;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.appcontext
{
    public class ApplicationContext
    {
        public IServerApi ServerApi { get; set; }
        public ISocketApi SocketApi { get; set; }
        public BaseUser User { get; set; }
        public ITabService TabService { get; set; }
        public IBottomPopupService BottomPopup { get; set; }
        public IApplicationSettings Settings { get; set; }
    }
}
