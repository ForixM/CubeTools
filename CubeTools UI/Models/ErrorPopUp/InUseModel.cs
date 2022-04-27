using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Library.ManagerExceptions;

namespace CubeTools_UI.Models.ErrorPopUp
{
    public class InUseModel : ErrorPopUpModel
    {
        private MainWindowModel _mainReference;
    
        public InUseModel(Views.ErrorPopUp.ErrorPopUp attachedView, ManagerException exception, MainWindowModel main) : base(attachedView, exception)
        {
            _mainReference = main;
        }

        public override void Initialize()
        {
            if (_attachedView == null || _exception == null) return;
            _attachedView.Title = "Data in use";
            _attachedView.StdError.Text = _exception.Errorstd;
            _attachedView.ContentError.Text = _exception.ErrorMessage;
            _attachedView.ImageError.Source = new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsIcons/Error.ico");
            _attachedView.Button1.IsVisible = false;
            _attachedView.Button2.IsVisible = true;
            _attachedView.Button2.Content = new TextBlock() {Text = "Cancel"};
            _attachedView.Button3.IsVisible = true;
            _attachedView.Button3.Content =  new TextBlock() { Text = "Try again"};
        }

        public override void Button3Clicked()
        {
            // TODO Ask fro priviliges
        }
    }   
}