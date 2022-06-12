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
            /*
            try 
            {
                
            }
            catch (Exception e)
            {
                if (e is ManagerException exception) Dispatcher.UIThread.Post(new ErrorBase(exception).Show);
                else 
                    Dispatcher.UIThread.Post(new ErrorBase(new ManagerException("App Crash", Level.Crash, "Unable to resolve the error",
                    "An unresolved error occured, the app has crashed")).Show);
                
                Thread.Sleep(2000);
                Environment.Exit(1);
            }
            Environment.Exit(0);
            */
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>().UsePlatformDetect().LogToTrace().UseReactiveUI();
        
    }
}