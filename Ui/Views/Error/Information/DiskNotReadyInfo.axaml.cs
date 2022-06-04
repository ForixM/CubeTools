using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Ui.Views.Error.Information
{
    public class DiskNotReadyInfo : UserControl
    {

        private ErrorBase? popup;
        
        public DiskNotReadyInfo()
        {
            InitializeComponent();
        }

        public DiskNotReadyInfo(ErrorBase popup)
        {
            this.popup = popup;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        // EVENTS
        private void OkClick(object? sender, RoutedEventArgs e) => popup!.Close(false);
        private void TryReloadClick(object? sender, RoutedEventArgs e) => popup!.Close(true);
    }
}