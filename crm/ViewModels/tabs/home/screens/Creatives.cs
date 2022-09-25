using crm.Models.api.server;
using crm.Models.api.socket;
using crm.Models.appcontext;
using crm.Models.creatives;
using crm.Models.storage;
using crm.Models.uniq;
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
using Xabe.FFmpeg;
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
        IPaths paths = Paths.getInstance();
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
                IsServerDirectoriesVisible = false;
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

        bool isUniqRunning;
        public bool IsUniqRunning
        {
            get => isUniqRunning;
            set => this.RaiseAndSetIfChanged(ref isUniqRunning, value);
        }

        bool isServerDirectoriesVisible;
        public bool IsServerDirectoriesVisible
        {
            get => isServerDirectoriesVisible;
            set => this.RaiseAndSetIfChanged(ref isServerDirectoriesVisible, value);
        }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> newCreativeCmd { get; }
        public ReactiveCommand<Unit, Unit> unicalizeCmd { get; }
        public ReactiveCommand<Unit, Unit> deselectAllCmd { get; }
        #endregion

        public Creatives() : base()
        {

            server = AppContext.ServerApi;
            socket = AppContext.SocketApi;
            token = AppContext.User.Token;

            #region commands
            newCreativeCmd = ReactiveCommand.CreateFromTask(async () =>
            {

                string[] files = await ws.ShowFileDialog("Выберите креатив");
                if (files != null && files.Length > 0)
                {
                    var dlg = new creativeUploadDlgVM()
                    {
                        Files = files,
                        CreativeServerDirectory = Content.CreativeServerDirectory
                    };

                    ws.ShowModalWindow(dlg);

                    try
                    {
                        await dlg.RunFilesUploadAsync();
                    } catch (Exception ex)
                    {
                        ws.ShowDialog(new errMsgVM(ex.Message));
                    }
                }
            });

            unicalizeCmd = ReactiveCommand.Create(() =>
            {

                var creatives = Content.CheckedCreatives;
                Debug.WriteLine(IsUniqRunning);

                if (!IsUniqRunning)
                {
                    MassActionText = "Прервать";
                    IsUniqRunning = true;

                    Task.Run(() =>
                    {
                        List<Task> tasks = new();

                        foreach (var creative in creatives)
                        {
                            if (creative.IsChecked)
                            {
                                if (Content.NeedMassUniqalization)
                                    tasks.Add(creative.Uniqalize(Content.Uniques));
                                else
                                    tasks.Add(creative.Uniqalize());
                            }
                        }

                        var continueTask = Task.WhenAll(tasks).ContinueWith((a) =>
                        {
                            IsUniqRunning = false;
                            Content.IsAllChecked = false;
                        });
                    });

                } else
                {
                    foreach (var creative in creatives)
                        creative.StopUniqalization();
                }


            });

            deselectAllCmd = ReactiveCommand.Create(() =>
            {
                Content.IsAllChecked = false;
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

            //MassActionText = (IsMassActionsVisible) ?
            //    $"Уникализировать ({checkedNumber} креатив{ending})" : "";

            MassActionText = (IsMassActionsVisible) ? $"Уникализировать ({checkedNumber} креатив{ending})" : "";

        }
        #endregion

        #region public
        #endregion

        #region callbacks
        private void GeoPage_CreativesSelectionChangedEvent(int number)
        {
            if (!IsUniqRunning)
                updateMassActions(number);
        }
        
        public void OnDragDrop(List<string> files) {

            string file = files[0];

            IUniqalizer uniqalizer = new Uniqalizer();

            string output = Path.Combine(paths.CreativesOutputRootPath, "DragDrop");

            uniqalizer.Uniqalize(file, 1, output);
        }
        #endregion

        #region override
        public override async void OnActivate()
        {
            base.OnActivate();
            //var dlg = new progressDlgVM();
            //ws.ShowModalWindow(dlg);

            await Uniqalizer.Init(Paths.getInstance().CodecBinariesPath, (progress) =>
            {
                //dlg.Progress = progress;
            });

#if ONLINE

            try
            {
                List<CreativeServerDirectory> dirs = await server.GetCreativeServerDirectories(token);

                foreach (var dir in dirs)
                {
                    bool found = GeoPages.Any(o => o.Title.Equals(dir.dir));
                    if (found)
                        continue;

                    var gp = new GeoPage(dir);
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
#endif

        }
        #endregion
    }
}
