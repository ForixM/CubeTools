using CubeTools_UI.Models;
using ReactiveUI;

namespace CubeTools_UI.ViewModels;

public class LinkBarViewModel : ReactiveObject
{
    // INIT
    private MainWindowModel model;
    public MainWindowModel ModelXaml
    {
        get => model;
        set => model = value;
    }

    private LinkBarModel modelLinkBar;

    public LinkBarModel ModelLinkBarXaml
    {
        get => modelLinkBar;
        set => modelLinkBar = value;
    }
    public LinkBarViewModel(MainWindowModel model)
    {
        this.model = model;
        this.modelLinkBar = model.ModelLinkBar;
    }

    public LinkBarViewModel()
    {
        model = null;
        modelLinkBar = null;
    }

    #region Bindings
    

    #endregion
    
    #region Actions
    
    #endregion
}