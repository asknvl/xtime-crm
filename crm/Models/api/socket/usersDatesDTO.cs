using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace crm.Models.api.socket
{
    public class usersDatesDTO
    {
        [JsonPropertyName("user_id")]
        public string user_id { get; set; }
        [JsonPropertyName("last_login_date")]
        public string last_login_date { get; set; }
        [JsonPropertyName("last_event_date")]
        public string last_event_date { get; set; }
    }
}
