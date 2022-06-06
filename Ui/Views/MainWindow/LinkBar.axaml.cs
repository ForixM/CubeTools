using System;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
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
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        public void InitializeExpanders()
        {
	        // Quick Access
	        _quickAccess.Children.Add(new OneLink(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Me", ResourcesLoader.ResourcesIconsCompressed.UsersCompressed));
	        _quickAccess.Children.Add(new OneLink(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Desktop", ResourcesLoader.ResourcesIconsCompressed.DesktopCompressed));
	        _quickAccess.Children.Add(new OneLink(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Documents", ResourcesLoader.ResourcesIconsCompressed.DocumentsCompressed));
	        _quickAccess.Children.Add(new OneLink(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Pictures", ResourcesLoader.ResourcesIconsCompressed.ImagesCompressed));
	        _quickAccess.Children.Add(new OneLink(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "Music", ResourcesLoader.ResourcesIconsCompressed.MusicCompressed));
	        _quickAccess.Children.Add(new OneLink(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "Videos", ResourcesLoader.ResourcesIconsCompressed.VideoCompressed));
	        _quickAccess.Children.Add(new OneLink(Environment.GetFolderPath(Environment.SpecialFolder.Favorites), "Favorites", ResourcesLoader.ResourcesIconsCompressed.FavoritesCompressed));
	        // Favorites
	        if (ConfigLoader.ConfigLoader.Settings.Links is not null)
		        foreach (var linkSetting in ConfigLoader.ConfigLoader.Settings.Links.Links)
			        _favorites.Children.Add(new OneLink(linkSetting.Path, linkSetting.Name, ResourcesLoader.ResourcesIconsCompressed.FolderCompressed));
	        // Drives
	        foreach (var drive in System.IO.DriveInfo.GetDrives())
		        _drives.Children.Add(new OneLink(drive.Name, drive.Name, ResourcesLoader.ResourcesIconsCompressed.DriveCompressed));
	        // Clouds
	        _clouds.Children.Add(new FTPHandler());
	        _clouds.Children.Add(new OneDriveHandler());
	        _clouds.Children.Add(new GoogleDriveHandler());

			new Thread(LaunchUpdaterDrivers).Start();
	        new Thread(LaunchUpdateLinks).Start();
        }

        private void LaunchUpdaterDrivers()
        {
	        int last = System.IO.DriveInfo.GetDrives().Length;
	        while (true)
	        {
		        Thread.Sleep(1000);
		        var drives = System.IO.DriveInfo.GetDrives();
		        if (last != drives.Length)
		        {
			        Dispatcher.UIThread.Post(() =>
			        {
				        _drives.Children.Clear();
				        foreach (var drive in System.IO.DriveInfo.GetDrives())
					        _drives.Children.Add(new OneLink(drive.Name, drive.Name,
						        ResourcesLoader.ResourcesIconsCompressed.DriveCompressed));
			        });
		        }

		        last = drives.Length;
	        }
        }

        private void LaunchUpdateLinks()
        {
	        int last = System.IO.DriveInfo.GetDrives().Length;
	        while (true)
	        {
		        Thread.Sleep(1000);
		        if (last != ConfigLoader.ConfigLoader.Settings.Links.Links.Count)
		        {
			        Dispatcher.UIThread.Post(() =>
			        {
				        _favorites.Children.Clear();
				        foreach (var link in ConfigLoader.ConfigLoader.Settings.Links.Links)
					        _favorites.Children.Add(new OneLink(link.Path, link.Name,
						        ResourcesLoader.ResourcesIconsCompressed.DriveCompressed));
			        });
		        }
		        last = ConfigLoader.ConfigLoader.Settings.Links.Links.Count;
	        }
        }

    }
}
