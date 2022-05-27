using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.user
{
    public class SocialNetwork
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("social_network")]
        public string Address { get; set; }
        [JsonProperty("account")]
        public string Account { get; set; }
    }
}
