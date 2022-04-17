using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using CubeTools_UI.ViewModels;
using CubeTools_UI.Views;

namespace CubeTools_UI
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.MainWindow = new MainWindow
                {
                };
            base.OnFrameworkInitializationCompleted();
        }
    }
}