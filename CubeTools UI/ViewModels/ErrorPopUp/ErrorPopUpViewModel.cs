using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Library.ManagerExceptions;

namespace CubeTools_UI.ViewModels.ErrorPopUp
{

    public class ErrorPopUpViewModel
    {
        protected Views.ErrorPopUp.ErrorPopUp? _attachedView;
        protected ManagerException? _exception;
        
        public ErrorPopUpViewModel() {}
        
        public ErrorPopUpViewModel(Views.ErrorPopUp.ErrorPopUp attachedView, ManagerException exception)
        {
            _attachedView = attachedView;
            _exception = exception;
        }
        
        public virtual void Initialize()
        {
            if (_attachedView == null || _exception == null) return;
            _attachedView.Title = "Error";
            _attachedView.StdError.Text = _exception.Errorstd;
            _attachedView.ContentError.Text = _exception.ErrorMessage;
            _attachedView.ImageError.Source = new Bitmap("Assets/CubeToolsIcons/CubeTools.ico");
            _attachedView.Button1.IsVisible = false;
            _attachedView.Button2.IsVisible = false;
            _attachedView.Button3.IsVisible = true;
            _attachedView.Button3.Content =  new TextBlock() { Text = "OK"};
        }
        
        // EVENTS
        public virtual void Button1Clicked()
        {
            _attachedView?.Close();
        }
        public virtual void Button2Clicked()
        {
            _attachedView?.Close();
        }
        public virtual void Button3Clicked()
        {
            _attachedView?.Close();
        }
    }
}