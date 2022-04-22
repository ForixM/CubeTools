using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Library.ManagerExceptions;

namespace CubeTools_UI.ViewModels.ErrorPopUp;

public class ReplaceViewModel : ErrorPopUpViewModel
{
    public MainWindowViewModel ParentViewModel;

    public ReplaceViewModel(Views.ErrorPopUp.ErrorPopUp attachedView, ManagerException exception, MainWindowViewModel parent) : base(attachedView,
        exception)
    {
        ParentViewModel = parent;
    }

    public override void Initialize()
    {
        if (_attachedView == null || _exception == null) return;
        _attachedView.Title = "Conflict with files";
        _attachedView.StdError.Text = _exception.Errorstd;
        _attachedView.ContentError.Text = _exception.ErrorMessage;
        //_attachedView.ImageError.Source = new Bitmap("Assets/CubeToolsIcons/CubeTools.ico");
        _attachedView.Button1.IsVisible = true;
        _attachedView.Button2.Content = new TextBlock() {Text = "Cancel"};
        _attachedView.Button2.IsVisible = true;
        _attachedView.Button2.Content = new TextBlock() {Text = "Replace"};
        _attachedView.Button3.IsVisible = true;
        _attachedView.Button3.Content =  new TextBlock() { Text = "Replace All"};
    }

    public override void Button1Clicked()
    {
        ParentViewModel.ReloadPath();
        _attachedView?.Close();
    }

    public override void Button2Clicked()
    {
        // TODO Add new stuff here
        base.Button2Clicked();
    }

    public override void Button3Clicked()
    {
        // TODO Add new Stuff here
        base.Button3Clicked();
    }
}