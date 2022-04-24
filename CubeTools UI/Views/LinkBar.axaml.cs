using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using Library.ManagerExceptions;

namespace CubeTools_UI.Views
{
    public class LinkBar : UserControl
    {
        public static LinkBarViewModel ViewModel;
        public LinkBar()
        {
            InitializeComponent();
            ViewModel = new LinkBarViewModel(this);
            DataContext = ViewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void AccessError(object? sender, RoutedEventArgs e)
        {
            var popup = new ErrorPopUp.ErrorPopUp(ViewModel.ParentViewModel, new AccessException("Not Implemented Yet"));
            popup.Show();
        }
        
        private void FTP(object? sender, RoutedEventArgs e)
        {
            
        }
    }
}
