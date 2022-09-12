using crm.Models.geoservice;
using crm.Models.storage;
using crm.ViewModels.tabs.home.screens.creatives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.creatives
{
    public class CreativesLocalManager : ICreativesLocalManager
    {
        #region vars
        IPaths paths = Paths.getInstance();
        #endregion

        public CreativesLocalManager()
        {

        }
        #region public
        public bool CheckCreativeDownloaded(ICreative creative)
        {   
            return File.Exists(creative.LocalPath);            
        }
        #endregion
    }
}
