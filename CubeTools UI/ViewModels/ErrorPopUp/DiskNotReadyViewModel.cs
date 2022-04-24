using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Library.ManagerExceptions;

namespace CubeTools_UI.ViewModels.ErrorPopUp
{
    
    public class DiskNotReadyViewModel : ErrorPopUpViewModel
    {

        public DiskNotReadyViewModel(Views.ErrorPopUp.ErrorPopUp attachedView, ManagerException exception) : base(attachedView,
                        exception)
        {
        
        }

        public override void Initialize()
        {
            if (_attachedView == null || _exception == null) return;
            _attachedView.Title = "Disk is not currently ready";
            _attachedView.StdError.Text = _exception.Errorstd;
            _attachedView.ContentError.Text = _exception.ErrorMessage;
            // TODO BE CAREFULL
            _attachedView.ImageError.Source = new Bitmap(MainWindowViewModel.CubeToolsPath + "/../../../Assets/CubeToolsIcons/CubeTools.ico");
            _attachedView.Button1.IsVisible = false;
            _attachedView.Button2.IsVisible = false;
            _attachedView.Button3.IsVisible = true;
            _attachedView.Button3.Content =  new TextBlock() { Text = "OK"};
        }
    }
}