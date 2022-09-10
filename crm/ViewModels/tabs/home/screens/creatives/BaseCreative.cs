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

namespace crm.ViewModels.tabs.home.screens.creatives
{
    public abstract class BaseCreative
    {

        #region vars
        ICreativesLocalManager localManaged = new CreativesLocalManager();
        ICreativesRemoteManager remoteManaged = new CreativesRemoteManager();
        #endregion

        #region propeties
        CreativeType type;
        public CreativeType Type {
            get => type;
            set => this.RaiseAndSetIfChanged(ref type, value);  
        }

        int id;
        public int Id
        {
            get => id;
            set => this.RaiseAndSetIfChanged(ref id, value);
        }

        geo.GEO geo;
        public geo.GEO GEO
        {
            get => geo;
            set => this.RaiseAndSetIfChanged(ref geo, value);
        }

        string name;
        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }

        string fname;
        public string FileName
        {
            get => fname;
            set => this.RaiseAndSetIfChanged(ref fname, value);
        }

        string lpath;
        public string LocalPath
        {
            get => lpath;
            set => this.RaiseAndSetIfChanged(ref lpath, value);
        }

        string upath;
        public string UrlPath
        {
            get => upath;
            set => this.RaiseAndSetIfChanged(ref upath, value);
        }

        bool isVisible;
        public bool IsVisible
        {
            get => isVisible;
            set => this.RaiseAndSetIfChanged(ref isVisible, value);
        }

        bool isUploaded;
        public bool IsUploaded
        {
            get => isUploaded;
            set => this.RaiseAndSetIfChanged(ref isUploaded, value);
        }

       
        #endregion
        public BaseCreative()
        {         
        }

        #region abstract
        public virtual async Task UnicalizeAsync() {
            await Task.Run(() => { });
        }
        #endregion

        #region public
        public virtual void Synchronize()
        {
            Task.Run(() => { });
        }        
        #endregion
    }
}
