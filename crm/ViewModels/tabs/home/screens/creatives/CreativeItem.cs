using crm.Models.api.server;
using crm.Models.creatives;
using crm.Models.storage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using geo = crm.Models.geoservice;

namespace crm.ViewModels.tabs.home.screens.creatives
{
    public class CreativeItem : ViewModelBase, ICreative
    {
        #region vars
        ICreativesRemoteManager remoteManager = new CreativesRemoteManager();
        ICreativesLocalManager localManager = new CreativesLocalManager();
        IPaths paths = Paths.getInstance();
        #endregion

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

        public CreativeType Type { get; set; }
        public int Id { get; set; }
        public geo.GEO GEO { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string LocalPath { get; set; }
        public string UrlPath { get; set; }
        public bool IsVisible { get; set; }
        public bool IsUploaded { get; set; }
        #endregion

        public CreativeItem(CreativeDTO dto)
        {
            Id = dto.id;
            Name = dto.name;
            //FileName = $"{dto.filename}.{dto.file_extension}";
            GEO = new geo.GEO() { Id = dto.geolocation_id, Code = dto.geolocation_code };
            Type = (dto.file_type.Equals("video")) ? CreativeType.video : CreativeType.picture;

            string stype ="";
            switch (Type)
            {
                case CreativeType.video:
                    stype = "videos";
                    break;
                case CreativeType.picture:
                    stype = "picures";
                    break;
                default:
                    break;
            }

            UrlPath = $"{paths.CreativesRootURL}/{dto.geolocation_code}/{stype}/{dto.name}.{dto.file_extension}";

            string geopath = Path.Combine(paths.CreativesRootPath, dto.geolocation_code);
            if (!Directory.Exists(geopath))
                Directory.CreateDirectory(geopath);

            LocalPath = Path.Combine(paths.CreativesRootPath, dto.geolocation_code, $"{dto.name}.{dto.file_extension}");

            IsVisible = dto.visibility;
            IsUploaded = dto.uploaded;
        }

        public void Synchronize()
        {
            remoteManager.Download(this);
        }

        public async Task UnicalizeAsync()
        {
            await Task.Run(() => { });
        }

        public event Action<CreativeItem, bool> CheckedEvent;
    }
}
