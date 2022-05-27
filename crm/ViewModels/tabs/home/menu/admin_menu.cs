using crm.Models.appcontext;
using crm.Models.user;

namespace crm.ViewModels.tabs.home.menu
{
    public class admin_menu : BaseMenu
    {

        public admin_menu() : base(new ApplicationContext())
        {
            ApplicationContext context = new ApplicationContext();
            context.ServerApi = new Models.api.server.ServerApi("");
            context.User = new TestUser();
            
            SimpleMenuItem dashboard = new items.Dashboard();
            dashboard.AddScreen(new screens.Dashboard(context));
            AddItem(dashboard);

            ComplexMenuItem proxies = new items.Proxies();
            proxies.AddScreen(new screens.TBD(context, "В разработке"));
            proxies.AddScreen(new screens.TBD(context, "В разработке"));
            AddItem(proxies);
        }

        public admin_menu(ApplicationContext context) : base(context)
        {

            SimpleMenuItem dashboard = new items.Dashboard();
            //dashboard.AddScreen(new screens.Dashboard(context));
            dashboard.AddScreen(new screens.TBD(context, "Dashboard"));
            AddItem(dashboard);

            ComplexMenuItem users = new items.Users();            
            users.AddScreen(new screens.UserList(context));
            users.AddScreen(new screens.UserActions(context));            
            AddItem(users);

            SimpleMenuItem accimport = new items.AccountsImport();
            //accimport.AddScreen(new screens.AccountsImport(context));
            accimport.AddScreen(new screens.TBD(context, "Импорт аккаунтов"));
            AddItem(accimport);

            SimpleMenuItem creatives = new items.Creatives();
            //creatives.AddScreen(new screens.Creatives(context));
            creatives.AddScreen(new screens.TBD(context, "Креативы"));
            AddItem(creatives);

            SimpleMenuItem subscriptions = new items.Subscriptions();
            //subscriptions.AddScreen(new screens.Subscriptions(context));
            subscriptions.AddScreen(new screens.TBD(context, "Подписки"));
            AddItem(subscriptions);

            ComplexMenuItem proxies = new items.Proxies();
            proxies.AddScreen(new screens.TBD(context, "В разработке"));
            proxies.AddScreen(new screens.TBD(context, "В разработке"));
            AddItem(proxies);

            SimpleMenuItem devices = new items.Devices();
            //devices.AddScreen(new screens.Devices(context));
            devices.AddScreen(new screens.TBD(context, "Устройства"));
            AddItem(devices);

            SimpleMenuItem geo = new items.GEO();
            //geo.AddScreen(new screens.GEO(context));
            geo.AddScreen(new screens.TBD(context, "ГЕО"));
            AddItem(geo);

            ComplexMenuItem finances = new items.Finances();
            //finances.AddScreen(new screens.Bills(context));
            //finances.AddScreen(new screens.Expenses(context));
            finances.AddScreen(new screens.TBD(context, "В разработке"));
            finances.AddScreen(new screens.TBD(context, "В разработке"));
            AddItem(finances);

            ComplexMenuItem accounts = new items.Accounts();
            accounts.AddScreen(new screens.TBD(context, "В разработке"));
            accounts.AddScreen(new screens.TBD(context, "В разработке"));
            AddItem(accounts);
        }
        
    }
}
