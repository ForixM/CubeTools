using System;
using System.Collections.ObjectModel;
using CubeTools_UI.Models;
using Library.ManagerReader;
using Library.Pointers;
using ReactiveUI;
namespace CubeTools_UI.ViewModels;

public class PathsBarViewModel : ReactiveObject
{
    private MainWindowModel? _model;
    public MainWindowModel? ModelXaml
    {
        get => _model;
        set => _model = value;
    }

    private PathsBarModel? _modelPathsBar;
    public PathsBarModel? ModelPathsBar
    {
        get => _modelPathsBar;
        set => _modelPathsBar = value;
    }

    
    public PathsBarViewModel()
    {
        _model = null;
        _modelPathsBar = null;
    }
    
}