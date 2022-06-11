using System;
using System.IO;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Library;
using Library.ManagerReader;
using ResourcesLoader;

namespace Ui.Views.LinkBar
{
    public class LinkBar : UserControl
    {
        public ClientUI Main;
        
        private StackPanel _quickAccess;
        private StackPanel _favorites;
        private StackPanel _drives;
        private StackPanel _clouds;
        private Image _remoteIcon;
        public StackPanel stackPanel;
        
        public LinkBar()
        {
            // Main = ClientUI.LastReference;
            InitializeComponent();
            _quickAccess = this.FindControl<StackPanel>("QuickAccess");
            _favorites = this.FindControl<StackPanel>("Favorites");
            _drives = this.FindControl<StackPanel>("Drives");
            _clouds = this.FindControl<StackPanel>("Clouds");
            _remoteIcon = this.FindControl<Image>("RemoteIcon");
            stackPanel = this.FindControl<StackPanel>("stackPanel");
        }

        public void ChangeLinkBarIcon()
        {
	        if (Main is not null && Main.Main is MainWindowRemote) //TODO Change this
	        {
		        _remoteIcon.Source = ((MainWindowRemote) Main.Main).RemoteView.Client.Type switch
		        {
						        ClientType.FTP => ResourcesLoader.ResourcesIconsCompressed.FtpCompressed,
						        ClientType.ONEDRIVE => ResourcesLoader.ResourcesIconsCompressed.OneDriveCompressed,
						        ClientType.GOOGLEDRIVE => ResourcesLoader.ResourcesIconsCompressed.GoogleDriveCompressed,
						        _ => _remoteIcon.Source
		        };
	        }
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        public void InitializeExpanders()
        {
	        // Quick Access
	        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)))
				_quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Me", ResourcesLoader.ResourcesIconsCompressed.UsersCompressed));
	        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
				_quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Desktop", ResourcesLoader.ResourcesIconsCompressed.DesktopCompressed));
	        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)))
				_quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Documents", ResourcesLoader.ResourcesIconsCompressed.DocumentsCompressed));
	        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)))
				_quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Pictures", ResourcesLoader.ResourcesIconsCompressed.ImagesCompressed));
	        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)))	
				_quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "Music", ResourcesLoader.ResourcesIconsCompressed.MusicCompressed));
	        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)))
				_quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "Videos", ResourcesLoader.ResourcesIconsCompressed.VideoCompressed));
	        //if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Favorites)))
			//	_quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.Favorites), "Favorites", ResourcesLoader.ResourcesIconsCompressed.FavoritesCompressed));
	        // Favorites
	        if (ConfigLoader.ConfigLoader.Settings.Links is not null)
		        foreach (var (key, path) in ConfigLoader.ConfigLoader.Settings.Links)
			        _favorites.Children.Add(new OneLink(Main, ConfigLoader.ConfigLoader.Settings.Links[key],key, ResourcesConverter.TypeToIcon(path, ManagerReader.GetFileExtension(path), Directory.Exists(path))));
	        // Drives
	        foreach (var drive in DriveInfo.GetDrives())
		        _drives.Children.Add(new OneLink(Main, drive.Name, $"{drive.VolumeLabel} ({drive.Name})", ResourcesLoader.ResourcesIconsCompressed.DriveCompressed));
	        // Clouds
	        _clouds.Children.Add(new FTPHandler());
	        _clouds.Children.Add(new OneDriveHandler());
	        _clouds.Children.Add(new GoogleDriveHandler());
			// Workers
			new Thread(LaunchUpdaterDrivers).Start();
	        new Thread(LaunchUpdateLinks).Start();
	        new Thread(LaunchUpdateStaticLinks).Start();
        }

        #region Workers
        
        /// <summary>
        /// Launch the worker in background
        /// </summary>
        private void LaunchUpdaterDrivers()
        {
	        int last = DriveInfo.GetDrives().Length;
	        while (Main.Main is MainWindow {IsClosed: false})
	        {
		        Thread.Sleep(1000);
		        var drives = DriveInfo.GetDrives();
		        if (last != drives.Length)
		        {
			        Dispatcher.UIThread.Post(() =>
			        {
				        _drives.Children.Clear();
				        foreach (var drive in DriveInfo.GetDrives())
					        _drives.Children.Add(new OneLink(Main, drive.Name, $"{drive.VolumeLabel} ({drive.Name})",
						        ResourcesLoader.ResourcesIconsCompressed.DriveCompressed));
			        }, DispatcherPriority.Background);
		        }
		        last = drives.Length;
	        }
	        while (Main.Main is MainWindowRemote {IsClosed: false})
	        {
		        Thread.Sleep(1000);
		        var drives = DriveInfo.GetDrives();
		        if (last != drives.Length)
		        {
			        Dispatcher.UIThread.Post(() =>
			        {
				        _drives.Children.Clear();
				        foreach (var drive in DriveInfo.GetDrives())
					        _drives.Children.Add(new OneLink(Main, drive.Name, $"{drive.VolumeLabel} ({drive.Name})",
						        ResourcesLoader.ResourcesIconsCompressed.DriveCompressed));
			        }, DispatcherPriority.Background);
		        }
		        last = drives.Length;
	        }
        }

        /// <summary>
        /// Launch the worker in background
        /// </summary>
        private void LaunchUpdateLinks()
        {
	        var last = ConfigLoader.ConfigLoader.Settings.Links;
	        while (Main.Main is MainWindow {IsClosed: false})
	        {
		        Thread.Sleep(1000);
		        Dispatcher.UIThread.Post(() =>
		        {
			        _favorites.Children.Clear();
			        foreach (var (key, path) in ConfigLoader.ConfigLoader.Settings.Links)
				        _favorites.Children.Add(new OneLink(Main, ConfigLoader.ConfigLoader.Settings.Links[key],key, ResourcesConverter.TypeToIcon(path, ManagerReader.GetFileExtension(path), Directory.Exists(path))));

		        }, DispatcherPriority.Background);
	        }
	        while (Main.Main is MainWindowRemote {IsClosed: false})
	        {
		        Thread.Sleep(1000);
		        Dispatcher.UIThread.Post(() =>
		        {
			        _favorites.Children.Clear();
			        foreach (var key in ConfigLoader.ConfigLoader.Settings.Links.Keys)
				        _favorites.Children.Add(new OneLink(Main, ConfigLoader.ConfigLoader.Settings.Links[key], key,
					        ResourcesLoader.ResourcesIconsCompressed.DriveCompressed));
		        }, DispatcherPriority.Background);
	        }
        }

        /// <summary>
        /// Launch the worker for static links
        /// </summary>
        private void LaunchUpdateStaticLinks()
        {
	        while (Main.Main is MainWindow {IsClosed: false})
	        {
		        Thread.Sleep(2000);
		        Dispatcher.UIThread.Post(() =>
		        {
			        _quickAccess.Children.Clear();
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)))
				        _quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Me", ResourcesLoader.ResourcesIconsCompressed.UsersCompressed));
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
				        _quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Desktop", ResourcesLoader.ResourcesIconsCompressed.DesktopCompressed));
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)))
				        _quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Documents", ResourcesLoader.ResourcesIconsCompressed.DocumentsCompressed));
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)))
				        _quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Pictures", ResourcesLoader.ResourcesIconsCompressed.ImagesCompressed));
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)))	
				        _quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "Music", ResourcesLoader.ResourcesIconsCompressed.MusicCompressed));
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)))
				        _quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "Videos", ResourcesLoader.ResourcesIconsCompressed.VideoCompressed));
			        //if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Favorites)))
				    //    _quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.Favorites), "Favorites", ResourcesLoader.ResourcesIconsCompressed.FavoritesCompressed));
		        }, DispatcherPriority.Background);
	        }
	        while (Main.Main is MainWindowRemote {IsClosed: false})
	        {
		        Thread.Sleep(2000);
		        Dispatcher.UIThread.Post(() =>
		        {
			        _quickAccess.Children.Clear();
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)))
				        _quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Me", ResourcesLoader.ResourcesIconsCompressed.UsersCompressed));
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
				        _quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Desktop", ResourcesLoader.ResourcesIconsCompressed.DesktopCompressed));
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)))
				        _quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Documents", ResourcesLoader.ResourcesIconsCompressed.DocumentsCompressed));
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)))
				        _quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Pictures", ResourcesLoader.ResourcesIconsCompressed.ImagesCompressed));
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)))	
				        _quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "Music", ResourcesLoader.ResourcesIconsCompressed.MusicCompressed));
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)))
				        _quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "Videos", ResourcesLoader.ResourcesIconsCompressed.VideoCompressed));
			        //if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Favorites)))
				    //    _quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.Favorites), "Favorites", ResourcesLoader.ResourcesIconsCompressed.FavoritesCompressed));
		        }, DispatcherPriority.Background);
	        }
        }
		
        #endregion

        private void OnClick(object? sender, RoutedEventArgs e)
        {
	        Main.AccessPath("");
        }
    }
}
