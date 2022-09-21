using crm.Models.api.server;
using crm.Models.creatives;
using crm.Models.storage;
using crm.Models.uniq;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using geo = crm.Models.geoservice;

namespace crm.ViewModels.tabs.home.screens.creatives
{
    public class CreativeItem : ViewModelBase, ICreative
    {
        #region vars
        ICreativesRemoteManager remoteManager;
        ICreativesLocalManager localManager;
        IUniqalizer uniqalizer;
        IPaths paths = Paths.getInstance();
        bool isSynchronizing = false;
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

        int uniques = 20;
        public int Uniques
        {
            get => uniques;
            set => this.RaiseAndSetIfChanged(ref uniques, value);
        }

        int progress;
        public int Progress
        {
            get => progress;
            set => this.RaiseAndSetIfChanged(ref progress, value);
        }

        bool isSynchronized;
        public bool IsSynchronized
        {
            get => isSynchronized;
            set => this.RaiseAndSetIfChanged(ref isSynchronized, value);
        }
        public CreativeType Type { get; set; }
        public int Id { get; set; }

        public CreativeServerDirectory ServerDirectory { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string LocalPath { get; set; }
        public string UrlPath { get; set; }
        public bool IsVisible { get; set; }
        public bool IsUploaded { get; set; }        
        #endregion

        public CreativeItem(CreativeDTO dto, CreativeServerDirectory dir)
        {

            remoteManager = new CreativesRemoteManager();
            remoteManager.DownloadProgessUpdateEvent += RemoteManager_DownloadProgessUpdateEvent;
            remoteManager.DownloadCompleted += RemoteManager_DownloadCompleted;
            localManager = new CreativesLocalManager();
            uniqalizer = new Uniqalizer();
            uniqalizer.UniqalizeProgessUpdateEvent += Uniqalizer_UniqalizeProgessUpdateEvent;

            Id = dto.id;
            Name = dto.name;
            //FileName = $"{dto.filename}.{dto.file_extension}";

            ServerDirectory = new CreativeServerDirectory()
            {
                id = dir.id,
                dir = dir.dir
            };

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
            

            UrlPath = $"{paths.CreativesRootURL}/{ServerDirectory.dir}/{stype}/{dto.name}.{dto.file_extension}";

            string geopath = Path.Combine(paths.CreativesRootPath, ServerDirectory.dir);
            if (!Directory.Exists(geopath))
                Directory.CreateDirectory(geopath);

            LocalPath = Path.Combine(paths.CreativesRootPath, ServerDirectory.dir, $"{dto.name}.{dto.file_extension}");

            IsVisible = dto.visibility;
            IsUploaded = dto.uploaded;
        }

        private void Uniqalizer_UniqalizeProgessUpdateEvent(int progress)
        {
            Progress = (progress < 100) ? progress : 0;
        }

        private void RemoteManager_DownloadCompleted()
        {
            IsSynchronized = true;
            Progress = 0;
        }

        private void RemoteManager_DownloadProgessUpdateEvent(int progress)
        {
            Progress = progress;            
        }

        #region public
        public void Synchronize()
        {
            if (isSynchronizing)
                return;

            Task.Run(async () => {
                isSynchronizing = true;
                if (localManager.CheckCreativeDownloaded(this))
                {
                    IsSynchronized = true;
                } else
                {
                    await remoteManager.Download(this);
                }
                isSynchronizing = false;
            });
        }

        public async Task Uniqalize()
        {
            if (!IsChecked)
                return;
            await uniqalizer.Uniqalize(this, Uniques, paths.CreativesOutputRootPath);
        }

        public async Task Uniqalize(int uniques)
        {
            if (!IsChecked)
                return;
            await uniqalizer.Uniqalize(this, uniques, paths.CreativesOutputRootPath);
        }

        public void StopUniqalization()
        {
            uniqalizer.Cancel();
        }
        #endregion

        #region callbacks
        public event Action<CreativeItem, bool> CheckedEvent;
        #endregion
    }
}
