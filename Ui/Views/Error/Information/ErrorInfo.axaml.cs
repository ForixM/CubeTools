using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Ui.Views.Error.Information
{
    public class ErrorInfo : UserControl
    {

        private ErrorBase? popup;
        
        public ErrorInfo()
        {
            InitializeComponent();
        }

        public ErrorInfo(ErrorBase popup)
        {
            this.popup = popup;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        // EVENTS
        private void CloseClick(object? sender, RoutedEventArgs e) => popup!.Close(false);
        private void TryReloadClick(object? sender, RoutedEventArgs e) => popup!.Close(true);
    }
}