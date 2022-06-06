using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Library.ManagerExceptions;
using LibraryClient;
using LibraryClient.LibraryOneDrive;
using Syroot.Windows.IO;
using Ui.Views.Error;
using Ui.Views.Ftp;
using Ui.Views.MainWindow.DynamicLinkBar;
using Ui.Views.Remote;

namespace Ui.Views.MainWindow
{
    public class LinkBar : UserControl
    {
        public Local.Local Main;
        
        private StackPanel _quickAccess;
        private StackPanel _favorites;
        private StackPanel _drives;
        private StackPanel _clouds;
        
        public LinkBar()
        {
            Main = Local.Local.LastReference;
            InitializeComponent();
            _quickAccess = this.FindControl<StackPanel>("QuickAccess");
            _favorites = this.FindControl<StackPanel>("Favorites");
            _drives = this.FindControl<StackPanel>("Drives");
            _clouds = this.FindControl<StackPanel>("Clouds");
            InitializeExpanders();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void InitializeExpanders()
        {
	        // Quick Access
	        _quickAccess.Children.Add(new OneLink(Environment.GetFolderPath(Environment.SpecialFolder.MyComputer), "My Computer", ResourcesLoader.ResourcesIconsCompressed.FolderCompressed));
	        _quickAccess.Children.Add(new OneLink(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Me", ResourcesLoader.ResourcesIconsCompressed.UsersCompressed));
	        _quickAccess.Children.Add(new OneLink(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Desktop", ResourcesLoader.ResourcesIconsCompressed.DesktopCompressed));
	        _quickAccess.Children.Add(new OneLink(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Documents", ResourcesLoader.ResourcesIconsCompressed.DocumentsCompressed));
	        _quickAccess.Children.Add(new OneLink(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Pictures", ResourcesLoader.ResourcesIconsCompressed.ImagesCompressed));
	        _quickAccess.Children.Add(new OneLink(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "Music", ResourcesLoader.ResourcesIconsCompressed.MusicCompressed));
	        _quickAccess.Children.Add(new OneLink(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "Videos", ResourcesLoader.ResourcesIconsCompressed.VideoCompressed));
	        _quickAccess.Children.Add(new OneLink(Environment.GetFolderPath(Environment.SpecialFolder.Favorites), "Favorites", ResourcesLoader.ResourcesIconsCompressed.FavoritesCompressed));
	        /*
	        // Favorites
	        foreach (var link in ConfigLoader.ConfigLoader.Settings.)
	        {
		        
	        }
	        */
	        // Drives
	        foreach (var drive in System.IO.DriveInfo.GetDrives())
		        _drives.Children.Add(new OneLink(drive.Name, drive.Name, ResourcesLoader.ResourcesIconsCompressed.DriveCompressed));
	        // Clouds
	        _clouds.Children.Add(new FTPHandler());
	        _clouds.Children.Add(new OneDriveHandler());
	        _clouds.Children.Add(new GoogleDriveHandler());
        }

    }
}
