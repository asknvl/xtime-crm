using crm.ViewModels;
using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.geoservice
{
    public class GEO : ViewModelBase
    {
        #region properties             
        int id;
        [JsonProperty("id")]
        public int Id
        {
            get => id;
            set => this.RaiseAndSetIfChanged(ref id, value);
        }

        string location_rus;
        [JsonProperty("location_rus")]
        public string Location_rus
        {
            get => location_rus;
            set => this.RaiseAndSetIfChanged(ref location_rus, value);
        }

        string location_eng;
        [JsonProperty("location_eng")]
        public string Location_eng
        {
            get => location_eng;
            set => this.RaiseAndSetIfChanged(ref location_eng, value);
        }

        string code;
        [JsonProperty("code")]
        public string Code
        {
            get => code;
            set => this.RaiseAndSetIfChanged(ref code, value); 
        }

        bool enabled;
        [JsonProperty("enabled")]
        public bool Enabled
        {
            get => enabled;
            set => this.RaiseAndSetIfChanged(ref enabled, value);
        }
        #endregion

        public GEO()
        {            
        }
    }
}
