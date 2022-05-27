using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Mixins;
using Avalonia.ReactiveUI;
using Avalonia.Svg;
using Avalonia.Svg.Skia;
using Avalonia.Xaml.Interactions.Core;
using Avalonia.Xaml.Interactivity;
using System;

namespace crm
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) => BuildAvaloniaApp()
           .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {

            GC.KeepAlive(typeof(SvgImageExtension).Assembly);
            GC.KeepAlive(typeof(Avalonia.Svg.Skia.Svg).Assembly);

            return AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .LogToTrace()
                         .UseReactiveUI();
        }
    }
}


