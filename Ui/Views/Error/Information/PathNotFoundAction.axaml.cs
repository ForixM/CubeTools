using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Ui.Views.Error.Information
{
    public class PathNotFoundInfo : UserControl
    {

        private ErrorBase? popup;
        
        public PathNotFoundInfo()
        {
            InitializeComponent();
        }

        public PathNotFoundInfo(ErrorBase popup)
        {
            this.popup = popup;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        // EVENTS
        private void ButtonQuitClicked(object? sender, RoutedEventArgs e) => popup!.Close(false);
        private void ButtonReloadClicked(object? sender, RoutedEventArgs e) => popup!.Close(true);
    }
}