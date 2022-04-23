using System;
using CubeTools_UI.Views.PopUps;
using Library.ManagerReader;
using Library.Pointers;
using ReactiveUI;

namespace CubeTools_UI.ViewModels.PopUps;

public class LoadingPopUpViewModel : ReactiveObject
{
    private LoadingPopUp _main;
    private double _progress;
    private double _max;
    private bool _destroy;
    private FileType _modified;

    public double Progress
    {
        get => _progress;
        set => this.RaiseAndSetIfChanged(ref _progress, value);
    }

    public LoadingPopUpViewModel()
    {
        _modified = FileType.NullPointer;
        _main = null;
        _max = 100;
        
    }
    public LoadingPopUpViewModel(LoadingPopUp main, FileType modified, double max, bool destroy)
    {
        _main = main;
        _modified = modified;
        _progress = 0;
        _max = max;
        _destroy = destroy;
    }
    
    public void ReloadProgress()
    {
        if (_destroy)
        {
            while (!_main.ProcessFinished)
            {
                try
                { 
                    Progress = (_max - ManagerReader.FastReaderFiles(_modified.Path)) / _max * 100;
                }
                catch (Exception) { }
            }
        }
        else
        {
            while (!_main.ProcessFinished)
            {
                try
                { 
                    Progress = ManagerReader.FastReaderFiles(_modified.Path) / _max * 100;
                }
                catch (Exception) { }
            }
        }
    }
}