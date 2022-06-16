using crm.Models.appcontext;
using crm.ViewModels.tabs.home.screens.creatives;
using Independentsoft.Webdav;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace crm.ViewModels.tabs.home.screens
{
    public class Creatives : BaseScreen
    {
        #region properties
        public override string Title => "Креативы";
        public ObservableCollection<BaseCreative> CreativesList { get; }
        #endregion

        #region commands
        public ReactiveCommand<Unit, Unit> testCmd { get; }
        #endregion

        public Creatives() : base()
        {
            testCmd = ReactiveCommand.CreateFromTask( async () => {

                NetworkCredential credential = new NetworkCredential("user287498742876", "TK&9HhALSv3utvd58px3#tGgQ");
                WebdavSession session = new WebdavSession(credential);
                Resource resource = new Resource(session);

                try
                {
                    string[] list = resource.List("http://136.243.74.153:4080/webdav/");
                    resource.CreateFolder("http://136.243.74.153:4080/webdav/1");

                    for (int i = 0; i < list.Length; i++)
                    {
                        Console.WriteLine(list[i]);
                    }
                } catch (Exception ex)
                {

                }
            });
        }       
    }
}
