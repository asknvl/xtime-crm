using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.storage
{
    public class Paths : IPaths
    {
        #region properties        
        public string VerPath { get; set; }
        public string ZipPath { get; set; }
        public string AppDir { get; set; }
        public string AppPath { get; set; }
        public string TmpDir { get; set; }
        public string VerURL { get; set; }
        public string ZipURL { get; set; }
        public string CreativesRootPath { get; set; }
        public string CreativesRootURL { get; set; }
        public string CreativesOutputRootPath { get; set; }
        public string CodecBinariesPath { get; set; }
        #endregion

        private static Paths instance;

        private Paths(bool ismacosx)
        {
            if (ismacosx)
                initPathsMac();
            else
                initPathsWin();

            initWebPaths();
        }

        public static Paths getInstance()
        {
            if (instance == null)
            {
                bool isMacOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
                instance = new Paths(isMacOSX);
            }
            return instance;
        }

        #region private
        Settings loadSettings()
        {
            //string json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "settings.json"));            
            //Settings settings = JsonConvert.DeserializeObject<Settings>(json);
            //return settings;
            Settings settings = new Settings();
            settings.app_name = "XTime CRM";
            settings.product_folder = "XTime";
            settings.version_file = "version.json";
            settings.update_url = "https://asemenets.com";
            return settings;

        }
        void initPathsWin()
        {
            Settings settings = loadSettings();
            string user_path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string product_path = Path.Combine(user_path,
                                                "Library",
                                                "Application Support",
                                                settings.product_folder);
            if (!Directory.Exists(product_path))
                Directory.CreateDirectory(product_path);

            string app_dir = Path.Combine(user_path,
                                            "Library",
                                            "Application Support",
                                            settings.product_folder,
                                            settings.app_name);
            if (!Directory.Exists(app_dir))
                Directory.CreateDirectory(app_dir);

            string tmp_path = Path.Combine(app_dir, "tmp");
            if (!Directory.Exists(tmp_path))
                Directory.CreateDirectory(tmp_path);

            VerURL = $"{settings.update_url}/{settings.version_file}";

            VerPath = Path.Combine(app_dir, settings.version_file);

            AppDir = app_dir;

            AppPath = Path.Combine(app_dir, $"{settings.app_name}.exe");

            CreativesRootPath = Path.Combine(app_dir, "Creatives");
            if (!Directory.Exists(CreativesRootPath))
                Directory.CreateDirectory(CreativesRootPath);

            Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            string userRootPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            CreativesOutputRootPath = Path.Combine(userRootPath, "Downloads", "Creatives");
            if (!Directory.Exists(CreativesOutputRootPath))
                Directory.CreateDirectory(CreativesOutputRootPath);

            CodecBinariesPath = Path.Combine(app_dir, "FFMPEG");
            if (!Directory.Exists(CodecBinariesPath))
                Directory.CreateDirectory(CodecBinariesPath);   

        }
        void initPathsMac()
        {
            Settings settings = loadSettings();
            string user_path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string product_path = Path.Combine(user_path,
                                                "Library",
                                                "Application Support",
                                                settings.product_folder);
            if (!Directory.Exists(product_path))
                Directory.CreateDirectory(product_path);

            string app_dir = Path.Combine(user_path,
                                            "Library",
                                            "Application Support",
                                            settings.product_folder,
                                            settings.app_name);
            if (!Directory.Exists(app_dir))
                Directory.CreateDirectory(app_dir);

            string tmp_path = Path.Combine(app_dir, "tmp");
            if (!Directory.Exists(tmp_path))
                Directory.CreateDirectory(tmp_path);

            TmpDir = tmp_path;

            VerURL = $"{settings.update_url}/{settings.version_file}";

            VerPath = Path.Combine(app_dir, settings.version_file);

            AppDir = app_dir;

            AppPath = Path.Combine(app_dir, $"{settings.app_name}.app");

            CreativesRootPath = Path.Combine(app_dir, "Creatives");
            if (!Directory.Exists(CreativesRootPath))
                Directory.CreateDirectory(CreativesRootPath);

        }

        void initWebPaths()
        {
            CreativesRootURL = "http://136.243.74.153:4080/webdav/uniq";
        }
        #endregion
    }

    public class Settings
    {
        public string product_folder { get; set; }
        public string app_name { get; set; }
        public string version_file { get; set; }
        public string update_url { get; set; }

    }
}
