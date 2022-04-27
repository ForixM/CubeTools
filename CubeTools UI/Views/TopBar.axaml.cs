using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;

namespace CubeTools_UI.Views
{
    public class TopBar : UserControl
    {
        public static TopBarModel Model;
        public TopBar()
        {
            InitializeComponent();
            Model = new TopBarModel(this);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
