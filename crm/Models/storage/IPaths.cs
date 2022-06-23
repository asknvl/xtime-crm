using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.Models.storage
{
    public interface IPaths
    {
        public string VerPath { get; set; }        
        public string AppDir { get; set; }
        public string AppPath { get; set; }        
        public string VerURL { get; set; }
        public string CreativesRootPath { get; set; }
        public string CreativesRootURL { get; set; }
        
    }
}
