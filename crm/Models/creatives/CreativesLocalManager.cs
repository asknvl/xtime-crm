using crm.Models.geoservice;
using crm.ViewModels.tabs.home.screens.creatives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.creatives
{
    public class CreativesLocalManager : ICreativesLocalManager
    {
        public Task<List<BaseCreative>> GetAvaliableAsync(GEO geo, CreativeType type)
        {
            throw new NotImplementedException();
        }
    }
}
