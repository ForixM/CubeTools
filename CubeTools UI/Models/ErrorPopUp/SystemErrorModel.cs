using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Library.ManagerExceptions;

namespace CubeTools_UI.Models.ErrorPopUp
{
    public class SystemErrorModel : ErrorPopUpModel
    {
        public MainWindowModel ParentModel;

        public SystemErrorModel(Views.ErrorPopUp.ErrorPopUp attachedView, ManagerException exception,
                        MainWindowModel parent) : base(attachedView,
                        exception)
        {
            ParentModel = parent;
        }

        public override void Initialize()
        {
            if (_attachedView == null || _exception == null) return;
            _attachedView.Title = "System crashes the application";
            _attachedView.StdError.Text = _exception.Errorstd;
            _attachedView.ContentError.Text = _exception.ErrorMessage;
            _attachedView.ImageError.Source = new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsIcons/AppCrashed.ico");
            _attachedView.Button1.IsVisible = false;
            _attachedView.Button2.IsVisible = true;
            _attachedView.Button2.Content = new TextBlock() {Text = "Reload"};
            _attachedView.Button3.IsVisible = true;
            _attachedView.Button3.Content = new TextBlock() {Text = "Quit"};
        }

        public override void Button2Clicked()
        {
            ParentModel.ReloadPath();
            _attachedView?.Close();
        }

        public override void Button3Clicked()
        {
            _attachedView?.Close();
            ParentModel.View.Close();
        }
    }
}