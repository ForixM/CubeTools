using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;

namespace Ui.Views.Error
{
    public class ErrorPopUp : Window
    {
        private readonly TextBlock _contentError;

        public ErrorPopUp()
        {
            InitializeComponent();
            _contentError = this.FindControl<TextBlock>("ContentError");
        }
        public ErrorPopUp(ManagerException exception) : this()
        {
            Title = exception.Errorstd;
            _contentError.Text = exception.ErrorMessage;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        // EVENTS

        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }
    }
}