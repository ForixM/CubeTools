using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Library.ManagerExceptions;

namespace CubeTools_UI.ViewModels.ErrorPopUp
{
    public class SystemErrorViewModel : ErrorPopUpViewModel
    {
        public MainWindowViewModel ParentViewModel;

        public SystemErrorViewModel(Views.ErrorPopUp.ErrorPopUp attachedView, ManagerException exception,
                        MainWindowViewModel parent) : base(attachedView,
                        exception)
        {
            ParentViewModel = parent;
        }

        public override void Initialize()
        {
            if (_attachedView == null || _exception == null) return;
            _attachedView.Title = "System crashes the application";
            _attachedView.StdError.Text = _exception.Errorstd;
            _attachedView.ContentError.Text = _exception.ErrorMessage;
            // TODO BE CAREFUL
            _attachedView.ImageError.Source = new Bitmap(MainWindowViewModel.CubeToolsPath + "/../../../Assets/CubeToolsIcons/CubeTools.ico");
            _attachedView.Button1.IsVisible = false;
            _attachedView.Button2.IsVisible = true;
            _attachedView.Button2.Content = new TextBlock() {Text = "Reload"};
            _attachedView.Button3.IsVisible = true;
            _attachedView.Button3.Content = new TextBlock() {Text = "Quit"};
        }

        public override void Button2Clicked()
        {
            ParentViewModel.ReloadPath();
            _attachedView?.Close();
        }

        public override void Button3Clicked()
        {
            _attachedView?.Close();
            ParentViewModel.AttachedView.Close();
        }
    }
}