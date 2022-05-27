using crm.Models.user;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.api.server.serialization
{
    public class sroles
    {
        [JsonProperty("roles")]
        public List<Role> roles { get; set; }

        public sroles(List<Role> roles)
        {
            this.roles = roles;
        }
    }
}
