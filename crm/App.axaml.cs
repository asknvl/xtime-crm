using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Mixins;
using Avalonia.Markup.Xaml;
using crm.ViewModels;
using crm.Views;
using crm.WS;

namespace crm
{
    public partial class App : Application
    {

        WindowService ws = WindowService.getInstance();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {


                mainVM main = new mainVM();
                ws.ShowWindow(main);

                //desktop.MainWindow = new MainWindow
                //{
                //    DataContext = new mainVM(),
                //};
            }

            base.OnFrameworkInitializationCompleted();
        }
    }


}
