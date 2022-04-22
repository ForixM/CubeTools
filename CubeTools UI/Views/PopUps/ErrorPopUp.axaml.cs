using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CubeTools_UI.Views.PopUps
{
    public class ErrorPopUp : Window
    {
        public ErrorPopUp()
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