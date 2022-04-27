using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Library.ManagerExceptions;

namespace CubeTools_UI.Models.ErrorPopUp
{
    
    public class DiskNotReadyModel : ErrorPopUpModel
    {

        public DiskNotReadyModel(Views.ErrorPopUp.ErrorPopUp attachedView, ManagerException exception) : base(attachedView,
                        exception)
        {
        
        }

        public override void Initialize()
        {
            if (_attachedView == null || _exception == null) return;
            _attachedView.Title = "Disk is not currently ready";
            _attachedView.StdError.Text = _exception.Errorstd;
            _attachedView.ContentError.Text = _exception.ErrorMessage;
            _attachedView.ImageError.Source = new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsIcons/AppCrashed.ico");
            _attachedView.Button1.IsVisible = false;
            _attachedView.Button2.IsVisible = false;
            _attachedView.Button3.IsVisible = true;
            _attachedView.Button3.Content =  new TextBlock() { Text = "OK"};
        }
    }
}