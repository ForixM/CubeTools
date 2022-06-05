using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Ui.Views.Error.Information;

namespace Ui.Views.Error
{
    public class ErrorBase : Window
    {
        private readonly TextBlock _contentError;
        private readonly Image _imageError;
        public Control Container;
        public readonly ManagerException BaseException;
        
        public ErrorBase()
        {
            InitializeComponent();
            Container = this.FindControl<Control>("Container");
            _contentError = this.FindControl<TextBlock>("ContentError");
            _imageError = this.FindControl<Image>("ImageError");
        }
        public ErrorBase(ManagerException exception) : this()
        {
            BaseException = exception;
            _contentError.Text = exception.ErrorMessage;
            CustomizeWindow();
        }

        public void CustomizeWindow()
        {
            Title = BaseException.Errorstd;
            _contentError.Text = BaseException.ErrorMessage;
            _imageError.Source = BaseException.Level switch
            {
                Level.Info => ResourcesLoader.ResourcesIcons.WarningIcon,
                Level.Normal => ResourcesLoader.ResourcesIcons.ErrorIcon,
                _ => ResourcesLoader.ResourcesIcons.CriticalErrorIcon
            };
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close(null);
        }
    }
}