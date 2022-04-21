using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;

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
    }
}
