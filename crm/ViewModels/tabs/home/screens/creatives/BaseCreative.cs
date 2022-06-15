using crm.Models.appcontext;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens.creatives
{
    public abstract class BaseCreative : ViewModelBase
    {
        #region vars
        protected ApplicationContext AppContext = ApplicationContext.getInstance();
        #endregion

        #region propeties
        GEO geo;
        public GEO GEO
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
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> toggleVisibility { get; }
        #endregion

        #region protected
        bool setVisibility(bool isvisible) {
            bool res = true;
            return res;
            //Запрос по рест сделать видимым            
        }
        #endregion

        #region abstract
        public abstract Task UnicalizeAsync();
        #endregion

        #region public
        public async Task SynchronizeAsync()
        {
            await Task.Run(() => { });
        }

        public async Task DownloadAsync()
        {
            await Task.Run(() => { });
        } 

        public async Task UploadAsync()
        {
            await Task.Run(() => { });
        }
        #endregion
    }
}
