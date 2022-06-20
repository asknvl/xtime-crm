using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using geo = crm.Models.geoservice;

namespace crm.ViewModels.tabs.home.screens.creatives
{
    public class GeoPage : BaseGeoPage
    {
        #region vars        
        #endregion

        #region properties
        geo.GEO geo;
        public geo.GEO GEO
        {
            get => geo;
            set => this.RaiseAndSetIfChanged(ref geo, value);
        }


        public ObservableCollection<BaseCreative> CreativesList { get; } = new();
        
        #endregion
        public GeoPage(geo.GEO g)
        {
            GEO = g;
            Title = GEO.Name;

            CreativesList.Add(new CreativeItem() { GEO = GEO, Name = $"{GEO.Name}1"});
            CreativesList.Add(new CreativeItem() { GEO = GEO, Name = $"{GEO.Name}2", IsChecked = true });
            CreativesList.Add(new CreativeItem() { GEO = GEO, Name = $"{GEO.Name}3" });

        }
    }
}
