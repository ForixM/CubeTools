using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Library.ManagerExceptions;

namespace CubeTools_UI.ViewModels.ErrorPopUp;

public class PathNotFoundViewModel : ErrorPopUpViewModel
{
    private MainWindowViewModel _mainReference;
    
    public PathNotFoundViewModel(Views.ErrorPopUp.ErrorPopUp attachedView, ManagerException exception, MainWindowViewModel main) : base(attachedView, exception)
    {
        _mainReference = main;
    }
    
    public override void Initialize()
    {
        if (_attachedView == null || _exception == null) return;
        _attachedView.Title = "Path not found";
        _attachedView.StdError.Text = _exception.Errorstd;
        _attachedView.ContentError.Text = _exception.ErrorMessage;
        //_attachedView.ImageError.Source = new Bitmap("Assets/CubeToolsIcons/CubeTools.ico");
        _attachedView.Button1.IsVisible = false;
        _attachedView.Button2.IsVisible = false;
        _attachedView.Button3.IsVisible = true;
        _attachedView.Button3.Content =  new TextBlock() { Text = "OK"};
    }
}