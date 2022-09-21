using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.creatives
{
    public class CreativePreviewer : ICreativePreviewer
    {
        #region vars
        bool isMacOSX;
        #endregion

        public CreativePreviewer()
        {
            isMacOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        }

        #region private
        #endregion

        #region public
        public void Preview(ICreative creative)
        {
            if (isMacOSX)
            {

            } else
            {               
                //Process.Start("wmp.exe");
                Process.Start(@"c:\Program Files (x86)\Windows Media Player\wmplayer.exe", "\"" + creative.LocalPath + "\"");
            } 
        }
        #endregion
    }
}
