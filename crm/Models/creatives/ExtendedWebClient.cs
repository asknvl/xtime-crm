using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.creatives
{
    public class ExtendedWebClient : WebClient
    {
        public int Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request =  base.GetWebRequest(address);
            
            request.Timeout = Timeout;
            return request;
        }
    }
}
