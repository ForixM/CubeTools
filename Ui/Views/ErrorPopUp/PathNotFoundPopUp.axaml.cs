using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;

namespace Ui.Views.ErrorPopUp
{
    public class PathNotFoundPopUp : Window
    {
        
        public readonly TextBlock ContentError;
        private readonly Button _buttonQuit;
        private readonly Button _buttonReload;

        public PathNotFoundPopUp()
        {
            InitializeComponent();
            ContentError = this.FindControl<TextBlock>("ContentError");
            _buttonQuit = this.FindControl<Button>("ButtonQuit");
            _buttonReload = this.FindControl<Button>("ButtonReload");
        }
        public PathNotFoundPopUp(ManagerException exception) : this()
        {
            ContentError.Text = exception.ErrorMessage;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);


        // EVENTS

        private void ButtonQuitClicked(object? sender, RoutedEventArgs e) => Close();
        private void ButtonReloadClicked(object? sender, RoutedEventArgs e) => Close();

        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }
    }
}