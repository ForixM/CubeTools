using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using CubeTools_UI.Models.ErrorPopUp;
using Library.ManagerExceptions;

namespace CubeTools_UI.Views.ErrorPopUp
{
    public class ErrorPopUp : Window
    {
        
        #region Attached Components

        public readonly Image ImageError;
        public readonly TextBlock StdError;
        public readonly TextBlock ContentError;
        public readonly Button Button1;
        public readonly Button Button2;
        public readonly Button Button3;
        
        #endregion

        public readonly ErrorPopUpModel? Model;
        public readonly MainWindowModel? ParentModel;

        public ErrorPopUp()
        {
            InitializeComponent();
            ImageError = this.FindControl<Image>("ImageError");
            StdError = this.FindControl<TextBlock>("StdError");
            ContentError = this.FindControl<TextBlock>("ContentError");
            Button1 = this.FindControl<Button>("Button1");
            Button2 = this.FindControl<Button>("Button2");
            Button3 = this.FindControl<Button>("Button3");
        }
        public ErrorPopUp(MainWindowModel parent) : this()
        {
            ParentModel = parent;
        }
        public ErrorPopUp(MainWindowModel parent,ManagerException exception) : this(parent)
        {
            Model = SelectViewModel(exception);
            Model?.Initialize();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private ErrorPopUpModel SelectViewModel(ManagerException exception)
        {
            if (ParentModel == null) return null;
            switch (exception)
            {
                case AccessException:
                    return new AccessModel(this, exception);
                case CorruptedDirectoryException or CorruptedPointerException:
                    return new CorruptedPointerModel(this, exception, ParentModel);
                case PathFormatException:
                    return new PathFormatModel(this, exception, ParentModel);
                case PathNotFoundException:
                    return new PathNotFoundModel(this, exception, ParentModel);
                case DiskNotReadyException :
                    return new DiskNotReadyModel(this, exception);
                case ReplaceException :
                    return new ReplaceModel(this, exception, ParentModel);
                case SystemErrorException :
                    return new SystemErrorModel(this,exception,ParentModel);
                default :
                    return new ErrorPopUpModel(this, exception);
            }
        }

        // EVENTS
        
        private void Button1Clicked(object? sender, RoutedEventArgs e)
        {
            Model?.Button1Clicked();
        }

        private void Button2Clicked(object? sender, RoutedEventArgs e)
        {
            Model?.Button2Clicked();
        }

        private void Button3Clicked(object? sender, RoutedEventArgs e)
        {
            Model?.Button3Clicked();
        }

        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }
    }
}