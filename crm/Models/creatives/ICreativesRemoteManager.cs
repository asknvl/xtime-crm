using crm.Models.geoservice;
using crm.ViewModels.tabs.home.screens.creatives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.creatives
{    
    internal interface ICreativesRemoteManager
    {
        Task Upload(GEO geo, string fullname);
        Task<List<BaseCreative>> GetAvaliableAsync(GEO geo, CreativeType type, bool showInvisible);
    }
}
