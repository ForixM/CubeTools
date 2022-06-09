using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;

namespace Ui.Views.Error
{
    public class ErrorBase : Window
    {
        private readonly TextBlock _contentError;
        private readonly TextBlock _stdError;
        private readonly Image _imageError;
        public Grid Container;
        public PopUpAction Type;
        public readonly ManagerException BaseException;
        
        public ErrorBase()
        {
            InitializeComponent();
            Container = this.FindControl<Grid>("Container");
            _contentError = this.FindControl<TextBlock>("ContentError");
            _stdError = this.FindControl<TextBlock>("StdError");
            _imageError = this.FindControl<Image>("ImageError");
        }
        public ErrorBase(ManagerException exception)
        {
            BaseException = exception;
            InitializeComponent();
            Container = this.FindControl<Grid>("Container");
            _contentError = this.FindControl<TextBlock>("ContentError");
            _stdError = this.FindControl<TextBlock>("StdError");
            _imageError = this.FindControl<Image>("ImageError");
            _contentError.Text = exception.ErrorMessage;
            ErrorConverter.SetContainer(this, exception);
            CustomizeWindow();
        }

        public void CustomizeWindow()
        {
            Title = BaseException.Errorstd;
            _stdError.Text = BaseException.ErrorType;
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