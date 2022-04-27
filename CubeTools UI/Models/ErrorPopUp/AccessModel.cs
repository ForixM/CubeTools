using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Library.ManagerExceptions;

namespace CubeTools_UI.Models.ErrorPopUp
{
    public class AccessModel : ErrorPopUpModel
    {

        public AccessModel(Views.ErrorPopUp.ErrorPopUp attachedView, ManagerException exception) : base(attachedView, exception)
        {
        
        }

        public override void Initialize()
        {
            if (_attachedView == null || _exception == null) return;
            _attachedView.Title = "Missing Privileges";
            _attachedView.StdError.Text = "Missing Privileges to execute operation !\nWould you like to continue as administrator ?";
            _attachedView.ContentError.Text = _exception.ErrorMessage;
            _attachedView.ImageError.Source = new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsIcons/Error.ico");
            _attachedView.Button1.IsVisible = false;
            _attachedView.Button2.IsVisible = true;
            _attachedView.Button2.Content = new TextBlock() {Text = "No"};
            _attachedView.Button3.IsVisible = true;
            _attachedView.Button3.Content =  new TextBlock() { Text = "Yes"};
        }

        public override void Button3Clicked()
        {
            // TODO Ask for privileges
        }
    }
}