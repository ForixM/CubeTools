using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Ui.Views.Error.Replace
{
    public class ReplaceAction : UserControl
    {

        private ErrorBase? popup;
        
        public ReplaceAction()
        {
            InitializeComponent();
        }

        public ReplaceAction(ErrorBase popup): this()
        {
            this.popup = popup;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        // EVENTS
        private void ReplaceClick(object? sender, RoutedEventArgs e) => popup!.Close(1);
        private void CopyClick(object? sender, RoutedEventArgs e) => popup!.Close(2);
        private void CancelClick(object? sender, RoutedEventArgs e) => popup!.Close(0);
    }
}