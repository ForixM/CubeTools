using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;

namespace Ui.Views.Error
{
    public class CriticalErrorPopUp : Window
    {
        public readonly TextBlock ContentError;
        private readonly Button _buttonOk;
        
        public CriticalErrorPopUp()
        {
            InitializeComponent();
            ContentError = this.FindControl<TextBlock>("ContentError");
            _buttonOk = this.FindControl<Button>("ButtonOk");
        }
        public CriticalErrorPopUp(ManagerException exception) : this()
        {
            ContentError.Text = exception.ErrorMessage;
            Title = exception.Errorstd;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        // EVENTS
        private void ButtonOkClicked(object? sender, RoutedEventArgs e) => Close();
        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }
    }
}