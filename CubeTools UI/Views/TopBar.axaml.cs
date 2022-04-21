using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;

namespace CubeTools_UI.Views
{
    public class TopBar : UserControl
    {
        public static TopBarViewModel ViewModel;
        public TopBar()
        {
            InitializeComponent();
            ViewModel = new TopBarViewModel(this);
            DataContext = ViewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
