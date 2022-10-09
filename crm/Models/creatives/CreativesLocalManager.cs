using crm.Models.geoservice;
using crm.Models.storage;
using crm.ViewModels.tabs.home.screens.creatives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;

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
        public bool CheckCreativeDownloaded(ICreative creative, long remote_size)
        {
            if (File.Exists(creative.LocalPath)) {
                long local_size = new FileInfo(creative.LocalPath).Length;
                return remote_size == local_size;
            }

            return false;
        }

        public async Task<string> GetThumbNail(ICreative creative)
        {
            IConversion conversion = await FFmpeg.Conversions.FromSnippet.Snapshot(creative.LocalPath, creative.ThumbNail, TimeSpan.FromSeconds(0));
            IConversionResult result = await conversion.Start();
            return creative.ThumbNail;
        }
        #endregion
    }
}
