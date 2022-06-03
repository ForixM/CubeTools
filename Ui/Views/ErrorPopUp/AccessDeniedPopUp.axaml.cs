using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;

namespace Ui.Views.ErrorPopUp
{
    public class AccessDeniedPopUp : Window
    {
        public readonly TextBlock ContentError;
        private readonly Button _buttonYes;
        private readonly Button _buttonNo;
        
        public AccessDeniedPopUp()
        {
            InitializeComponent();
            ContentError = this.FindControl<TextBlock>("ContentError");
            _buttonYes = this.FindControl<Button>("ButtonYes");
            _buttonNo = this.FindControl<Button>("ButtonNo");
        }
        public AccessDeniedPopUp(ManagerException exception) : this()
        {
            ContentError.Text = exception.ErrorMessage;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        // EVENTS
        private void ButtonYesClicked(object? sender, RoutedEventArgs e)
        {
            // TODO Edit Yes button for admin action
        }
        private void ButtonNoClicked(object? sender, RoutedEventArgs e) => Close();
        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }
    }
}