﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Library;
using Library.ManagerReader;
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
        
        public void InitializeExpanders()
        {
	        // Quick Access
	        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)))
				_quickAccess.Children.Add(new OneLinkMenu(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Me", ResourcesLoader.ResourcesIconsCompressed.UsersCompressed));
	        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
				_quickAccess.Children.Add(new OneLinkMenu(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Desktop", ResourcesLoader.ResourcesIconsCompressed.DesktopCompressed));
	        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)))
				_quickAccess.Children.Add(new OneLinkMenu(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Documents", ResourcesLoader.ResourcesIconsCompressed.DocumentsCompressed));
	        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)))
				_quickAccess.Children.Add(new OneLinkMenu(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Pictures", ResourcesLoader.ResourcesIconsCompressed.ImagesCompressed));
	        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)))	
				_quickAccess.Children.Add(new OneLinkMenu(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "Music", ResourcesLoader.ResourcesIconsCompressed.MusicCompressed));
	        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)))
				_quickAccess.Children.Add(new OneLinkMenu(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "Videos", ResourcesLoader.ResourcesIconsCompressed.VideoCompressed));
	        //if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Favorites)))
			//	_quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.Favorites), "Favorites", ResourcesLoader.ResourcesIconsCompressed.FavoritesCompressed));
	        // Favorites
	        if (ConfigLoader.ConfigLoader.Settings.Links is not null)
		        foreach (var (key, path) in ConfigLoader.ConfigLoader.Settings.Links)
			        _favorites.Children.Add(new OneLinkMenu(ConfigLoader.ConfigLoader.Settings.Links[key],key, ResourcesConverter.TypeToIcon(path, ManagerReader.GetFileExtension(path), Directory.Exists(path))));
	        // Drives
	        foreach (var drive in DriveInfo.GetDrives())
		        _drives.Children.Add(new OneLinkMenuDrives(drive.Name, $"{drive.VolumeLabel} ({drive.Name})", ResourcesLoader.ResourcesIconsCompressed.DriveCompressed));
	        // Clouds
	        _clouds.Children.Add(new FTPHandler());
	        _clouds.Children.Add(new OneDriveHandler());
	        _clouds.Children.Add(new GoogleDriveHandler());
			// Workers
			new Thread(LaunchUpdaterDrivers).Start();
	        new Thread(LaunchUpdateLinks).Start();
	        new Thread(LaunchUpdateStaticLinks).Start();
        }
        
        private void LaunchUpdaterDrivers()
        {
	        int last = DriveInfo.GetDrives().Length;
	        while (_main is MainWindow {IsClosed: false})
	        {
		        Thread.Sleep(1000);
		        var drives = DriveInfo.GetDrives();
		        if (last != drives.Length)
		        {
			        Dispatcher.UIThread.Post(() =>
			        {
				        _drives.Children.Clear();
				        foreach (var drive in DriveInfo.GetDrives())
					        _drives.Children.Add(new OneLinkMenu(drive.Name, $"{drive.VolumeLabel} ({drive.Name})",
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
	        while (_main is MainWindow {IsClosed: false})
	        {
		        Thread.Sleep(1000);
		        Dispatcher.UIThread.Post(() =>
		        {
			        _favorites.Children.Clear();
			        foreach (var (key, path) in ConfigLoader.ConfigLoader.Settings.Links)
				        _favorites.Children.Add(new OneLinkMenu(ConfigLoader.ConfigLoader.Settings.Links[key],key, ResourcesConverter.TypeToIcon(path, ManagerReader.GetFileExtension(path), Directory.Exists(path))));

		        }, DispatcherPriority.Background);
	        }
        }

        /// <summary>
        /// Launch the worker for static links
        /// </summary>
        private void LaunchUpdateStaticLinks()
        {
	        while (_main is MainWindow {IsClosed: false})
	        {
		        Thread.Sleep(2000);
		        Dispatcher.UIThread.Post(() =>
		        {
			        _quickAccess.Children.Clear();
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)))
				        _quickAccess.Children.Add(new OneLinkMenu(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Me", ResourcesLoader.ResourcesIconsCompressed.UsersCompressed));
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
				        _quickAccess.Children.Add(new OneLinkMenu(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Desktop", ResourcesLoader.ResourcesIconsCompressed.DesktopCompressed));
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)))
				        _quickAccess.Children.Add(new OneLinkMenu(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Documents", ResourcesLoader.ResourcesIconsCompressed.DocumentsCompressed));
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)))
				        _quickAccess.Children.Add(new OneLinkMenu(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Pictures", ResourcesLoader.ResourcesIconsCompressed.ImagesCompressed));
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)))	
				        _quickAccess.Children.Add(new OneLinkMenu(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "Music", ResourcesLoader.ResourcesIconsCompressed.MusicCompressed));
			        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)))
				        _quickAccess.Children.Add(new OneLinkMenu(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "Videos", ResourcesLoader.ResourcesIconsCompressed.VideoCompressed));
			        //if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Favorites)))
				    //    _quickAccess.Children.Add(new OneLink(Main, Environment.GetFolderPath(Environment.SpecialFolder.Favorites), "Favorites", ResourcesLoader.ResourcesIconsCompressed.FavoritesCompressed));
		        }, DispatcherPriority.Background);
	        }
        }

        public void InitializeComponents()
        {
            foreach (IControl control in _main.LinkBarView._drives.Children)
            {
                _drives.Children.Add(control);
            }
            foreach (IControl control in _main.LinkBarView._favorites.Children)
            {
                _favorites.Children.Add(control);
            }
            foreach (IControl control in _main.LinkBarView._clouds.Children)
            {
                _clouds.Children.Add(control);
            }
            foreach (IControl control in _main.LinkBarView._quickAccess.Children)
            {
                _quickAccess.Children.Add(control);
            }
        }
            
        // private void InitializeDrives()
        // {
        //     foreach (var oneLink in _main.LinkBarView.QuickAccess.Children.Cast<OneLink>())
        //     {
        //         _quickAccess.Children.Add(
        //                         new OneLink(oneLink.LocalPointer.Path, oneLink.LocalPointer.Name, oneLink.Image.Source));
        //     }
        //     
        //     foreach (var oneLink in _main.LinkBarView.Favorites.Children.Cast<OneLink>())
        //     {
        //         _favorites.Children.Add(new OneLink(oneLink.LocalPointer.Path, oneLink.LocalPointer.Name, oneLink.Image.Source));
        //     }
        //     
        //     foreach (var oneLink in _main.LinkBarView.Drives.Children.Cast<OneLink>())
        //     {
        //         _drives.Children.Add(new OneLink(oneLink.LocalPointer.Path, oneLink.LocalPointer.Name, oneLink.Image.Source));
        //     }
        //     
        //     _clouds.Children.Add(new FTPHandler());
        //     _clouds.Children.Add(new OneDriveHandler());
        //     _clouds.Children.Add(new GoogleDriveHandler());
        // }
    }
}