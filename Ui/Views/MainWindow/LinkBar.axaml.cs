using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Syroot.Windows.IO;
using Ui.Views.Ftp;

namespace Ui.Views.MainWindow
{
    public class LinkBar : UserControl
    {

        public Local.Local Main;
        
        public LinkBar()
        {
            Main = Local.Local.LastReference;
            InitializeComponent();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void AccessError(object? sender, RoutedEventArgs e)
        {
        }
        
        private void FTP(object? sender, RoutedEventArgs e) => new LoginFTP().Show();

        private void OpenDesktop(object? sender, RoutedEventArgs e)
        {
            Main.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }

        private void OpenDocuments(object? sender, RoutedEventArgs e)
        {
            Main.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        }

        private void OpenImages(object? sender, RoutedEventArgs e)
        {
            Main.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
        }

        private void OpenDownloads(object? sender, RoutedEventArgs e)
        {
            Main.AccessPath(KnownFolders.Downloads.Path);
        }

        private void OpenMusic(object? sender, RoutedEventArgs e)
        {
            Main.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
        }

        private void OpenVideos(object? sender, RoutedEventArgs e)
        {
            Main.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
        }

        private void OpenFavorites(object? sender, RoutedEventArgs e)
        {
            Main.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.Favorites));
        }

        private void OpenMyComputer(object? sender, RoutedEventArgs e)
        {
            Main.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.MyComputer));
        }

        private void OpenUser(object? sender, RoutedEventArgs e)
        {
            Main.AccessPath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        }
    }
}
