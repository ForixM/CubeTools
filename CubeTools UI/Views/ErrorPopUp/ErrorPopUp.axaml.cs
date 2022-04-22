using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using CubeTools_UI.ViewModels.ErrorPopUp;
using Library.ManagerExceptions;

namespace CubeTools_UI.Views.ErrorPopUp
{
    public class ErrorPopUp : Window
    {
        
        #region Attached Components

        public Image ImageError;
        public TextBlock StdError;
        public TextBlock ContentError;
        public Button Button1;
        public Button Button2;
        public Button Button3;
        
        #endregion

        public ErrorPopUpViewModel? ViewModel;
        public MainWindowViewModel? ParentViewModel;

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
        public ErrorPopUp(MainWindowViewModel parent) : this()
        {
            ParentViewModel = parent;
        }
        public ErrorPopUp(MainWindowViewModel parent,ManagerException exception) : this(parent)
        {
            ViewModel = SelectViewModel(exception);
            ViewModel.Initialize();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private ErrorPopUpViewModel SelectViewModel(ManagerException exception)
        {
            if (ParentViewModel == null) return null;
            switch (exception)
            {
                case AccessException:
                    return new AccessViewModel(this, exception);
                case CorruptedDirectoryException or CorruptedPointerException:
                    return new CorruptedPointerViewModel(this, exception, ParentViewModel);
                case PathFormatException:
                    return new PathFormatViewModel(this, exception, ParentViewModel);
                case PathNotFoundException:
                    return new PathNotFoundViewModel(this, exception, ParentViewModel);
                case DiskNotReadyException :
                    return new DiskNotReadyViewModel(this, exception);
                case ReplaceException :
                    return new ReplaceViewModel(this, exception, ParentViewModel);
                case SystemErrorException :
                    return new SystemErrorViewModel(this,exception,ParentViewModel);
                default :
                    return new ErrorPopUpViewModel(this, exception);
            }
        }

        // EVENTS
        
        private void Button1Clicked(object? sender, RoutedEventArgs e)
        {
            ViewModel?.Button1Clicked();
        }

        private void Button2Clicked(object? sender, RoutedEventArgs e)
        {
            ViewModel?.Button2Clicked();
        }

        private void Button3Clicked(object? sender, RoutedEventArgs e)
        {
            ViewModel?.Button3Clicked();
        }
    }
}