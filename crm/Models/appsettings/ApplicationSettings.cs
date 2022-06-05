using crm.Models.storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.appsettings
{
    public class ApplicationSettings : IApplicationSettings
    {
        #region vars
        IStorage<ApplicationSettings> storage;
        #endregion

        #region properties
        [JsonProperty("Login")]
        public string Login { get; set; } = "";
        [JsonProperty("Password")]
        public string Password { get; set; } = "";
        [JsonProperty("RememberMe")]
        public bool RememberMe { get; set; } = false;
        #endregion

        public ApplicationSettings()
        {
             storage = new Storage<ApplicationSettings>("settings.json", this);
            
        }

        #region public
        public void Load()
        {
            var t = storage.load();
            Login = t.Login;
            Password = t.Password;
            RememberMe = t.RememberMe;
        }

        public void Save()
        {
            storage.save(this);
        }
        #endregion
    }
}
