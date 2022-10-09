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
        Task Upload(CreativeServerDirectory dir, string fullname);
        Task Download(ICreative creative);
        Task<long> GetFileSize(ICreative creative);

        event Action<int> UploadProgressUpdateEvent;

        event Action<int> DownloadProgessUpdateEvent;

        event Action DownloadCompleted;
        
    }
}
