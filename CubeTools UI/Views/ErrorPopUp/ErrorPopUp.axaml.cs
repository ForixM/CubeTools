using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using Library.ManagerExceptions;

namespace CubeTools_UI.Views.ErrorPopUp
{
    public class ErrorPopUp : Window
    {
        public readonly TextBlock ContentError;
        public readonly MainWindowModel? ParentModel;

        public ErrorPopUp()
        {
            InitializeComponent();
            ContentError = this.FindControl<TextBlock>("ContentError");
        }
        public ErrorPopUp(MainWindowModel parent) : this()
        {
            ParentModel = parent;
        }
        public ErrorPopUp(MainWindowModel parent,ManagerException exception) : this(parent)
        {
            switch (exception)
            {
                case PathNotFoundException @pathNotFoundException:
                    new PathNotFoundPopUp(parent, @pathNotFoundException).Show();
                    Close();
                    break;
                case AccessException @accessException:
                    new AccessDeniedPopUp(parent, @accessException).Show();
                    Close();
                    break;
                case DiskNotReadyException @diskNotReadyException:
                    new DiskNotReadyPopUp(parent, @diskNotReadyException).Show();
                    Close();
                    break;
                case SystemErrorException @systemErrorException:
                    new SystemErrorPopUp(parent, @systemErrorException).Show();
                    Close();
                    break;
                default:
                    ContentError.Text = exception.ErrorMessage;
                    break;
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        // EVENTS

        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }
    }
}