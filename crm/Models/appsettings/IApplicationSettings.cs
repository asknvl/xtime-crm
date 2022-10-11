using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.appsettings
{
    public interface IApplicationSettings
    {        
        public string Login { get; set; }     
        public string Password { get; set; }        
        public bool RememberMe { get; set; }
        public int CreativesPerPage { get; set; }
        public void Load();
        public void Save();        
    }
}
