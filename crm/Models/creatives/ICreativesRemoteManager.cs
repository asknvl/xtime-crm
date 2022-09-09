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
        Task Download(GEO geo, string filename);
        Task<List<CreativeItem>> GetAvaliableCreatives(GEO geo, CreativeType type);

        event Action<int> UploadProgressUpdateEvent;

        event Action<int> DownloadProgessUpdateEvent;
        
    }
}
