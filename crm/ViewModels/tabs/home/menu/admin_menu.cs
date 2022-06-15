using crm.Models.appcontext;
using crm.Models.user;

namespace crm.ViewModels.tabs.home.menu
{
    public class admin_menu : BaseMenu
    {

        //public admin_menu() : base()
        //{
        //    ApplicationContext context = ApplicationContext.getInstance();
        //    context.ServerApi = new Models.api.server.ServerApi("");
        //    context.User = new TestUser();
            
        //    SimpleMenuItem dashboard = new items.Dashboard();
        //    dashboard.AddScreen(new screens.Dashboard());
        //    AddItem(dashboard);

        //    ComplexMenuItem proxies = new items.Proxies();
        //    proxies.AddScreen(new screens.TBD("В разработке"));
        //    proxies.AddScreen(new screens.TBD("В разработке"));
        //    AddItem(proxies);

        //     SimpleMenuItem creatives = new items.Creatives();
        //    creatives.AddScreen(new screens.Creatives());
        //    //creatives.AddScreen(new screens.TBD(context, "Креативы"));
        //    AddItem(creatives);
        //}

        public admin_menu() : base()
        {

            SimpleMenuItem dashboard = new items.Dashboard();
            //dashboard.AddScreen(new screens.Dashboard(context));
            dashboard.AddScreen(new screens.TBD("Dashboard"));
            AddItem(dashboard);

            ComplexMenuItem users = new items.Users();            
            users.AddScreen(new screens.UserList());
            users.AddScreen(new screens.UserActions());            
            AddItem(users);

            SimpleMenuItem accimport = new items.AccountsImport();
            //accimport.AddScreen(new screens.AccountsImport(context));
            accimport.AddScreen(new screens.TBD("Импорт аккаунтов"));
            AddItem(accimport);

            SimpleMenuItem creatives = new items.Creatives();
            creatives.AddScreen(new screens.Creatives());
            //creatives.AddScreen(new screens.TBD(context, "Креативы"));
            AddItem(creatives);

            SimpleMenuItem subscriptions = new items.Subscriptions();
            //subscriptions.AddScreen(new screens.Subscriptions(context));
            subscriptions.AddScreen(new screens.TBD("Подписки"));
            AddItem(subscriptions);

            ComplexMenuItem proxies = new items.Proxies();
            proxies.AddScreen(new screens.TBD("В разработке"));
            proxies.AddScreen(new screens.TBD("В разработке"));
            AddItem(proxies);

            SimpleMenuItem devices = new items.Devices();
            //devices.AddScreen(new screens.Devices(context));
            devices.AddScreen(new screens.TBD("Устройства"));
            AddItem(devices);

            SimpleMenuItem geo = new items.GEO();
            //geo.AddScreen(new screens.GEO(context));
            geo.AddScreen(new screens.TBD("ГЕО"));
            AddItem(geo);

            ComplexMenuItem finances = new items.Finances();
            //finances.AddScreen(new screens.Bills(context));
            //finances.AddScreen(new screens.Expenses(context));
            finances.AddScreen(new screens.TBD("В разработке"));
            finances.AddScreen(new screens.TBD("В разработке"));
            AddItem(finances);

            ComplexMenuItem accounts = new items.Accounts();
            accounts.AddScreen(new screens.TBD("В разработке"));
            accounts.AddScreen(new screens.TBD("В разработке"));
            AddItem(accounts);
        }
        
    }
}
