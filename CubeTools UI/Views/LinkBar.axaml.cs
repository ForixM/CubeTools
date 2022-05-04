using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using Library.ManagerExceptions;
using Library.Pointers;

namespace CubeTools_UI.Views
{
    public class LinkBar : UserControl
    {
        public static LinkBarModel Model;
        public LinkBar()
        {
            InitializeComponent();
            Model = new LinkBarModel(this);
            DataContext = Model;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void AccessError(object? sender, RoutedEventArgs e)
        {
            var popup = new ErrorPopUp.ErrorPopUp(Model.ParentModel!, new AccessException("Not Implemented Yet"));
            popup.Show();
        }
        
        private void FTP(object? sender, RoutedEventArgs e)
        {
            var popup = new Ftp.LoginFTP();
            popup.Show();
        }

        private void OpenDesktop(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }

        private void OpenDocuments(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        }

        private void OpenImages(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
        }

        private void OpenDownloads(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.MyComputer));;
        }

        private void OpenMusic(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
        }

        private void OpenVideos(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
        }

        private void OpenFavorites(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.Favorites));
        }

        private void OpenMyComputer(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.MyComputer));
        }

        private void OpenUser(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        }
    }
}
