using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;

namespace Ui.Views.ErrorPopUp
{
    public class NormalErrorPopUp : Window
    {
        public readonly TextBlock ContentError;
        private readonly Button ButtonOk;
        
        public NormalErrorPopUp()
        {
            InitializeComponent();
            ContentError = this.FindControl<TextBlock>("ContentError");
            ButtonOk = this.FindControl<Button>("ButtonOk");
        }
        public NormalErrorPopUp(ManagerException exception) : this()
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