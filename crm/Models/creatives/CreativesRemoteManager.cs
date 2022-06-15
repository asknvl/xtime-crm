using crm.Models.geoservice;
using crm.ViewModels.tabs.home.screens.creatives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.creatives
{
    public class CreativesRemoteManager : ICreativesRemoteManager
    {
        #region vars
        WebClient client;
        #endregion

        public CreativesRemoteManager()
        {
            client = new WebClient();
        }

        public Task<List<BaseCreative>> GetAvaliableAsync(GEO geo, CreativeType type, bool showInvisible)
        {
            throw new NotImplementedException();
        }

        public Task Upload(GEO geo, string fullname)
        {
            throw new NotImplementedException();
        }
    }
}
