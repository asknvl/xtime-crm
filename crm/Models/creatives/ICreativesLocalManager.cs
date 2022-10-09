using crm.Models.geoservice;
using crm.ViewModels.tabs.home.screens.creatives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.creatives
{
    public interface ICreativesLocalManager
    {
        bool CheckCreativeDownloaded(ICreative creative, long remote_size);
        Task<string> GetThumbNail(ICreative creative);
    }
}
