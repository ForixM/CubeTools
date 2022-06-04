using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Ui.Views.Error.Information;

namespace Ui.Views.Error
{
    public class ErrorBase : Window
    {
        private readonly TextBlock ContentError;
        private Control Container;
        public readonly ManagerException BaseException;
        
        public ErrorBase()
        {
            InitializeComponent();
            Container = this.FindControl<Control>("Container");
            ContentError = this.FindControl<TextBlock>("ContentError");
        }
        public ErrorBase(ManagerException exception) : this()
        {
            BaseException = exception;
            ContentError.Text = exception.ErrorMessage;
        }

        public void CustomizeWindow()
        {
            Title = BaseException.Errorstd;
            ContentError.Text = BaseException.ErrorMessage;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void InitializeContainer()
        {
            Container = BaseException switch
            {
                PathNotFoundException => new PathNotFoundInfo(this),
                _ => Container
            };
        }
        
        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close(null);
        }
    }
}