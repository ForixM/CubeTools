using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using Library.Pointers;
using Syroot.Windows.IO;

namespace CubeTools_UI.Views
{
    public class LinkBar : UserControl
    {
        public static LinkBarModel LastModel;
        public LinkBarModel Model;
        public LinkBar()
        {
            InitializeComponent();
            Model = new LinkBarModel(this);
            LastModel = Model;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void AccessError(object? sender, RoutedEventArgs e)
        {
        }
        
        private void FTP(object? sender, RoutedEventArgs e) => new Ftp.LoginFTP().Show();

        private void OpenDesktop(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel?.ModelLocal.ModelNavigationBar.Add(new DirectoryType(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));
            Model.ParentModel?.ModelLocal.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }

        private void OpenDocuments(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel?.ModelLocal.ModelNavigationBar.Add(new DirectoryType(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));
            Model.ParentModel?.ModelLocal.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        }

        private void OpenImages(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel?.ModelLocal.ModelNavigationBar.Add(new DirectoryType(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)));
            Model.ParentModel?.ModelLocal.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
        }

        private void OpenDownloads(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel?.ModelLocal.ModelNavigationBar.Add(new DirectoryType(KnownFolders.Downloads.Path));
            Model.ParentModel?.ModelLocal.AccessPath(KnownFolders.Downloads.Path);;
        }

        private void OpenMusic(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel?.ModelLocal.ModelNavigationBar.Add(new DirectoryType(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)));
            Model.ParentModel?.ModelLocal.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
        }

        private void OpenVideos(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel?.ModelLocal.ModelNavigationBar.Add(new DirectoryType(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)));
            Model.ParentModel?.ModelLocal.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
        }

        private void OpenFavorites(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel?.ModelLocal.ModelNavigationBar.Add(new DirectoryType(Environment.GetFolderPath(Environment.SpecialFolder.Favorites)));
            Model.ParentModel?.ModelLocal.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.Favorites));
        }

        private void OpenMyComputer(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel?.ModelLocal.ModelNavigationBar.Add(new DirectoryType(Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)));
            Model.ParentModel?.ModelLocal.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.MyComputer));
        }

        private void OpenUser(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel?.ModelLocal.ModelNavigationBar.Add(new DirectoryType(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)));
            Model.ParentModel?.ModelLocal.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        }
    }
}
