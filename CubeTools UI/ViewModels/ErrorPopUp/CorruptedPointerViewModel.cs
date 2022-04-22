using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Library.ManagerExceptions;

namespace CubeTools_UI.ViewModels.ErrorPopUp;

public class CorruptedPointerViewModel : ErrorPopUpViewModel
{
    private MainWindowViewModel _mainReference;
    
    public CorruptedPointerViewModel(Views.ErrorPopUp.ErrorPopUp attachedView, ManagerException exception, MainWindowViewModel main) : base(attachedView, exception)
    {
        _mainReference = main;
    }
    
    public override void Initialize()
    {
        if (_attachedView == null || _exception == null) return;
        _attachedView.Title = "Internal Error";
        _attachedView.StdError.Text = _exception.Errorstd;
        _attachedView.ContentError.Text = _exception.ErrorMessage;
        //_attachedView.ImageError.Source = new Bitmap("Assets/CubeToolsIcons/CubeTools.ico");
        _attachedView.Button1.IsVisible = false;
        _attachedView.Button2.IsVisible = false;
        _attachedView.Button3.IsVisible = true;
        _attachedView.Button3.Content =  new TextBlock() { Text = "Reload"};
    }

    public override void Button3Clicked()
    {
        _mainReference.ReloadPath();
        _attachedView?.Close();
    }
}