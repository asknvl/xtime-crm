using crm.Models.appcontext;
using crm.Models.creatives;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using geo = crm.Models.geoservice;

namespace crm.Models.creatives
{
    public interface ICreative
    {   
        public CreativeType Type { get; set; }        
        public int Id { get; set; }                
        CreativeServerDirectory ServerDirectory { get; set; }        
        public string Name { get; set; }                
        public string FileName { get; set; }                
        public string LocalPath { get; set; }                
        public string UrlPath { get; set; }                
        public bool IsVisible { get; set; }        
        public bool IsUploaded { get; set; }       
        
        public Task Uniqalize();
        public Task Uniqalize(int uniques);
        public void StopUniqalization();
        public void Synchronize();               
        
    }
}
