using System;
using System.Collections.Generic;
using System.Threading;
using Avalonia;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using Library.ManagerExceptions;
using Newtonsoft.Json;
using Ui.Views.Error;

namespace Ui
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            // Initialization
            InitLoader.InitLoader.Start();
            // Initialization of views and Avalonia process
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>().UsePlatformDetect().LogToTrace().UseReactiveUI();
        
    }
}