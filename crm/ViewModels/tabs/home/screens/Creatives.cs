﻿using crm.Models.api.server;
using crm.Models.api.socket;
using crm.Models.appcontext;
using crm.Models.creatives;
using crm.ViewModels.dialogs;
using crm.ViewModels.tabs.home.screens.creatives;
using crm.WS;
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
using geo = crm.Models.geoservice;

namespace crm.ViewModels.tabs.home.screens
{
    public class Creatives : BaseScreen
    {
        #region vars
        IServerApi server;
        ISocketApi socket;
        string token;
        IWindowService ws = WindowService.getInstance();
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
                content.OnActivate();             
            }
        }

        bool isMassActionsVisible;
        public bool IsMassActionsVisible
        {
            get => isMassActionsVisible;
            set => this.RaiseAndSetIfChanged(ref isMassActionsVisible, value);
        }

        bool isMassActionOpen;
        public bool IsMassActionOpen
        {
            get => isMassActionOpen;
            set => this.RaiseAndSetIfChanged(ref isMassActionOpen, value);
        }

        string massActiontext;
        public string MassActionText
        {
            get => massActiontext;
            set => this.RaiseAndSetIfChanged(ref massActiontext, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> newCreativeCmd { get; }
        public ReactiveCommand<Unit, Unit> testCmd { get; }
        #endregion

        public Creatives() : base()
        {

            server = AppContext.ServerApi;
            socket = AppContext.SocketApi;
            token = AppContext.User.Token;

            #region commands
            newCreativeCmd = ReactiveCommand.CreateFromTask( async () => {

                //geo.GEO geo = Content.GEO;
                //Debug.WriteLine(geo.Code);
                string[] files = await ws.ShowFileDialog("Выберите креатив");
                if (files != null && files.Length > 0)
                {
                    var dlg = new creativeUploadDlgVM()
                    {
                        Files = files,
                        GEO = Content.GEO
                    };                    

                    ws.ShowModalWindow(dlg);                   

                    try
                    {
                        await dlg.RunFilesUploadAsync();
                    }
                    catch (Exception ex)
                    {
                        ws.ShowDialog(new errMsgVM(ex.Message));
                    }

                    
                }

            });


            testCmd = ReactiveCommand.CreateFromTask( async () => {

                WebClient client = new WebClient();
                NetworkCredential credential = new NetworkCredential(
                 "user287498742876",
                 "TK&9HhALSv3utvd58px3#tGgQ"
                 );                
                
                client.Credentials = credential;
                client.UploadProgressChanged +=(s,e)=>{
                    Debug.WriteLine($"{e.BytesSent} {e.TotalBytesToSend} {e.ProgressPercentage}%");
                };

                await client.UploadFileTaskAsync(@"http://136.243.74.153:4080/webdav/uniq/IND/videos/1.mp4", "PUT", @"D:\out\1.mp4");
                //await client.DownloadFileTaskAsync("http://136.243.74.153:4080/webdav/1.mp4", @"D:\out\2.mp4");

                //CreativesRemoteManager remote = new CreativesRemoteManager("http://136.243.74.153:4080/webdav/", AppContext.ServerApi, AppContext.SocketApi);
                //remote.UploadProgressUpdateEvent += Remote_UploadProgressUpdateEvent;
                //Models.geoservice.GEO geo = new Models.geoservice.GEO("IND");
                //await remote.Upload(geo, @"D:\out\1.mp4");

            });
            #endregion
        }

        #region helpers
        void updateMassActions(int checkedNumber)
        {            
            IsMassActionsVisible = checkedNumber > 0;

            string ending = "";           
            
            
            if (checkedNumber.ToString().EndsWith("11") ||
                checkedNumber.ToString().EndsWith("12") ||
                checkedNumber.ToString().EndsWith("13") ||
                checkedNumber.ToString().EndsWith("14"))
                ending = "ов";
            else
            if (checkedNumber.ToString().EndsWith("2") ||
                checkedNumber.ToString().EndsWith("3") ||
                checkedNumber.ToString().EndsWith("4"))
                ending = "a";
            else
            if (checkedNumber.ToString().EndsWith("1"))
                ending = "";
            else
                ending = "ов";

            MassActionText = (IsMassActionsVisible) ?
                $"Уникализировать ({checkedNumber} креатив{ending})" : "";
        }
        #endregion

        #region public
        #endregion

        #region callbacks
        private void GeoPage_CreativesSelectionChangedEvent(int number)
        {
            updateMassActions(number);
        }
        #endregion

        #region override
        public override async void OnActivate()
        {
            base.OnActivate();

#if ONLINE

            try
            {
                List<geo.GEO> geos = await server.GetGeos(token, "+id");

                foreach (var geo in geos)
                {
                    bool found = GeoPages.Any(o => o.Title.Equals(geo.Code));
                    if (found)
                        continue;

                    var gp = new GeoPage(geo);
                    gp.CreativesSelectionChangedEvent += GeoPage_CreativesSelectionChangedEvent;
                    GeoPages.Add(gp);
                }

                if (Content == null)
                    Content = GeoPages[0];

            } catch (Exception ex)
            {
                ws.ShowDialog(new errMsgVM(ex.Message));
            }
#else
            GeoPage p1 = new GeoPage(new geo.GEO() { Code = "IND" });
            p1.CreativesSelectionChangedEvent += GeoPage_CreativesSelectionChangedEvent;
            GeoPage p2 = new GeoPage(new geo.GEO() { Code = "PER" });
            p2.CreativesSelectionChangedEvent += GeoPage_CreativesSelectionChangedEvent;
            GeoPage p3 = new GeoPage(new geo.GEO() { Code = "COL"});
            p3.CreativesSelectionChangedEvent += GeoPage_CreativesSelectionChangedEvent;
            GeoPage p4 = new GeoPage(new geo.GEO() { Code = "PERX1" });
            p4.CreativesSelectionChangedEvent += GeoPage_CreativesSelectionChangedEvent;
            GeoPage p5 = new GeoPage(new geo.GEO() { Code = "PERX2" });
            p4.CreativesSelectionChangedEvent += GeoPage_CreativesSelectionChangedEvent;
            GeoPage p6 = new GeoPage(new geo.GEO() { Code = "PERX3" });
            p4.CreativesSelectionChangedEvent += GeoPage_CreativesSelectionChangedEvent;
            GeoPage p7 = new GeoPage(new geo.GEO() { Code = "COLX1" });
            p4.CreativesSelectionChangedEvent += GeoPage_CreativesSelectionChangedEvent;
            GeoPage p8 = new GeoPage(new geo.GEO() { Code = "COLX2" });
            p8.CreativesSelectionChangedEvent += GeoPage_CreativesSelectionChangedEvent;

            GeoPages.Add(p1);
            GeoPages.Add(p2);
            GeoPages.Add(p3);
            GeoPages.Add(p4);
            GeoPages.Add(p5);
            GeoPages.Add(p6);
            GeoPages.Add(p7);
            GeoPages.Add(p8);            

            Content = GeoPages[0];
#endif
            //get all avaliable geos
            //create list for selected geo
        }
        #endregion
    }
}
