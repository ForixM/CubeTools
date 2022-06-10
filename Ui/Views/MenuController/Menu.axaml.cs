using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Library;
using ResourcesLoader;
using Ui.Views.LinkBar;
using Ui.Views.Settings;

namespace Ui.Views.MenuController
{
    public class Menu : UserControl
    {
        private MainWindow _main;
        private ClientUI _client;
        
        private WrapPanel _quickAccess;
        private WrapPanel _favorites;
        private WrapPanel _drives;
        private WrapPanel _clouds;
        
        public Menu()
        {
            InitializeComponent();
            _quickAccess = this.FindControl<WrapPanel>("QuickAccess");
            _favorites = this.FindControl<WrapPanel>("Favorites");
            _clouds = this.FindControl<WrapPanel>("Clouds");
            _drives = this.FindControl<WrapPanel>("Drives");
        }

        public Menu(ClientUI client) : this()
        {
            _client = client;
            _main = (MainWindow)_client.Main;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void InitializeDrives()
        {
            foreach (var oneLink in _main.LinkBarView.QuickAccess.Children.Cast<OneLink>())
            {
                _quickAccess.Children.Add(
                                new OneLink(oneLink.LocalPointer.Path, oneLink.LocalPointer.Name, oneLink.Image.Source));
            }
            
            foreach (var oneLink in _main.LinkBarView.Favorites.Children.Cast<OneLink>())
            {
                _favorites.Children.Add(new OneLink(oneLink.LocalPointer.Path, oneLink.LocalPointer.Name, oneLink.Image.Source));
            }
            
            foreach (var oneLink in _main.LinkBarView.Drives.Children.Cast<OneLink>())
            {
                _drives.Children.Add(new OneLink(oneLink.LocalPointer.Path, oneLink.LocalPointer.Name, oneLink.Image.Source));
            }
            
            _clouds.Children.Add(new FTPHandler());
            _clouds.Children.Add(new OneDriveHandler());
            _clouds.Children.Add(new GoogleDriveHandler());
        }
    }
}