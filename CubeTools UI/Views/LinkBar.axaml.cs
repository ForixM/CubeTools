using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using Library.ManagerExceptions;

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
    }
}
