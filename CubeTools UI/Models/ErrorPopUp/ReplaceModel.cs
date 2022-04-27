using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Library.ManagerExceptions;

namespace CubeTools_UI.Models.ErrorPopUp
{
    public class ReplaceModel : ErrorPopUpModel
    {
        public MainWindowModel ParentModel;

        public ReplaceModel(Views.ErrorPopUp.ErrorPopUp attachedView, ManagerException exception, MainWindowModel parent) : base(attachedView,
                        exception)
        {
            ParentModel = parent;
        }

        public override void Initialize()
        {
            if (_attachedView == null || _exception == null) return;
            _attachedView.Title = "Conflict with files";
            _attachedView.StdError.Text = _exception.Errorstd;
            _attachedView.ContentError.Text = _exception.ErrorMessage;
            _attachedView.ImageError.Source = new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsIcons/Error.ico");
            _attachedView.Button1.IsVisible = true;
            _attachedView.Button2.Content = new TextBlock() {Text = "Cancel"};
            _attachedView.Button2.IsVisible = true;
            _attachedView.Button2.Content = new TextBlock() {Text = "Replace"};
            _attachedView.Button3.IsVisible = true;
            _attachedView.Button3.Content =  new TextBlock() { Text = "Replace All"};
        }

        public override void Button1Clicked()
        {
            ParentModel.ReloadPath();
            _attachedView?.Close();
        }
    }   
}