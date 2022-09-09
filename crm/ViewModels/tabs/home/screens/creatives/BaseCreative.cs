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
    public abstract class BaseCreative : ViewModelBase
    {
        #region propeties
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

        string path;
        public string Path
        {
            get => path;
            set => this.RaiseAndSetIfChanged(ref path, value);
        }

        bool isVisible;
        public bool IsVisible
        {
            get => isVisible;
            set => this.RaiseAndSetIfChanged(ref isVisible, value);
        }

        int uniques;
        public int Uniques
        {
            get => uniques;
            set => this.RaiseAndSetIfChanged(ref uniques, value);
        }

        //bool isChecked;
        //public virtual bool IsChecked
        //{
        //    get => isChecked;
        //    set => this.RaiseAndSetIfChanged(ref isChecked, value);
        //}
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
