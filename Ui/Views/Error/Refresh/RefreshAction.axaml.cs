using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Ui.Views.Error.Refresh
{
    public class RefreshAction : UserControl
    {

        private ErrorBase? popup;
        
        public RefreshAction()
        {
            InitializeComponent();
        }

        public RefreshAction(ErrorBase popup): this()
        {
            this.popup = popup;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        // EVENTS
        private void OkClick(object? sender, RoutedEventArgs e) => popup!.Close(false);
        private void TryReloadClick(object? sender, RoutedEventArgs e) => popup!.Close(true);
    }
}