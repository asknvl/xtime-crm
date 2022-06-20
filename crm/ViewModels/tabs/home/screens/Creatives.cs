﻿using crm.Models.api.server;
using crm.Models.api.socket;
using crm.Models.appcontext;
using crm.Models.creatives;
using crm.ViewModels.tabs.home.screens.creatives;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens
{
    public class Creatives : BaseScreen
    {
        #region vars
        IServerApi server;
        ISocketApi socket;
        #endregion

        #region properties
        public override string Title => "Креативы";
        public ObservableCollection<GeoPage> GeoPages { get; } = new();

        GeoPage content;
        public GeoPage Content
        {
            get => content;
            set
            {
                this.RaiseAndSetIfChanged(ref content, value);
            }
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> testCmd { get; }
        #endregion

        public Creatives() : base()
        {

            GeoPages.Add(new GeoPage(new Models.geoservice.GEO("IND")));
            GeoPages.Add(new GeoPage(new Models.geoservice.GEO("PER")));
            GeoPages.Add(new GeoPage(new Models.geoservice.GEO("LAM")));

            Content = GeoPages[0];

            #region dependencies
            #endregion

            testCmd = ReactiveCommand.CreateFromTask( async () => {

                WebClient client = new WebClient();
                NetworkCredential credential = new NetworkCredential(
                 "user287498742876",
                 "TK&9HhALSv3utvd58px3#tGgQ"
                 );                
                
                client.Credentials = credential;
                //client.DownloadProgressChanged += Client_DownloadProgressChanged;

                await client.UploadFileTaskAsync(@"http://136.243.74.153:4080/webdav/uniq/1.mp4", "PUT", @"D:\out\1.mp4");
                //await client.DownloadFileTaskAsync("http://136.243.74.153:4080/webdav/1.mp4", @"D:\out\2.mp4");

                //CreativesRemoteManager remote = new CreativesRemoteManager("http://136.243.74.153:4080/webdav/", AppContext.ServerApi, AppContext.SocketApi);
                //remote.UploadProgressUpdateEvent += Remote_UploadProgressUpdateEvent;
                //Models.geoservice.GEO geo = new Models.geoservice.GEO("IND");
                //await remote.Upload(geo, @"D:\out\1.mp4");

            });
        }

        public override void OnActivate()
        {
            base.OnActivate();            

            //get all avaliable geos
            //create list for selected geo

        }
    }
}
