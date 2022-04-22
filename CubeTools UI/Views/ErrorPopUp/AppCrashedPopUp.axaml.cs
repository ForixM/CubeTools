using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CubeTools_UI.Views.PopUps
{
    public class AppCrashedPopUp : Window
    {
        public AppCrashedPopUp()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}