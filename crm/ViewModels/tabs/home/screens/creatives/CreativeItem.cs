using crm.Models.api.server;
using crm.Models.creatives;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using geo = crm.Models.geoservice;

namespace crm.ViewModels.tabs.home.screens.creatives
{
    public class CreativeItem : BaseCreative
    {
        #region properties        
        bool isChecked;
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                this.RaiseAndSetIfChanged(ref isChecked, value);
                CheckedEvent?.Invoke(this, value);
            }
        }

        int uniques;
        public int Uniques
        {
            get => uniques;
            set => this.RaiseAndSetIfChanged(ref uniques, value);
        }
        #endregion

        public CreativeItem() { }

        public CreativeItem(CreativeDTO dto)
        {
            Id = dto.id;
            Name = dto.name;
            //FileName = $"{dto.filename}.{dto.file_extension}";
            GEO = new geo.GEO() { Id = dto.geolocation_id, Code = dto.geolocation_code };
            Type = (dto.file_type.Equals("video")) ? CreativeType.video : CreativeType.picture;
            IsVisible = dto.visibility;
            IsUploaded = dto.uploaded;
        }      

        #region events
        public event Action<CreativeItem, bool> CheckedEvent;
        #endregion
    }
}
