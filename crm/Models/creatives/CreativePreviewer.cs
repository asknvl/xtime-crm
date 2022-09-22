﻿using System;
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

                //string s = "tell application \"QuickTime Player\" activat set thisFile to open POSIX file \"/Users/alexeykonovalov/Downloads/IMG_1379.MP4\" play thisFile end tell";

                //string s = "tell application \"QuickTime Player\"\nactivate\net thisFile to open POSIX file \"/Users/alexeykonovalov/Downloads/IMG_1379.MP4\"\nplay thisFile\nend tell";

                string s = "osascript -e 'tell application \"QuickTime Player\" to activate'";


                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = "/bin/bash",
                    //FileName = "osascript -e 'display dialog \"Hello\"'",
                    Arguments = $"-c \"osascript -e \' tell application \\\"QuickTime Player\\\"\n" +
                    $"activate\n" +
                    //$"set thisFile to open POSIX file \\\"/Users/alexeykonovalov/Downloads/IMG_1379.MP4\\\"\n" +
                    $"set thisFile to open POSIX file \\\"{creative.LocalPath}\\\"\n" +
                    $"play thisFile\n" +
                    $"end tell \' \"",
                    UseShellExecute = true

                };

                Process process = new Process() { StartInfo = startInfo };
                process.Start();

                //System.Diagnostics.Process.Start();


            } else
            {               
                //Process.Start("wmp.exe");
                Process.Start(@"c:\Program Files (x86)\Windows Media Player\wmplayer.exe", "\"" + creative.LocalPath + "\"");
            } 
        }
        #endregion
    }
}
