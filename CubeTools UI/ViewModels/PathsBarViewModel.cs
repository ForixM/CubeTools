using System;
using System.Collections.ObjectModel;
using CubeTools_UI.Models;
using CubeTools_UI.Views;
using Library.ManagerReader;
using Library.Pointers;
using ReactiveUI;

namespace CubeTools_UI.ViewModels
{
    
    public class PathsBarViewModel : ReactiveObject
    {

        public PathsBar AttachedView;
    
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
    
        private MainWindowViewModel _parentParentViewModel;
        public MainWindowViewModel ParentViewModelXaml
        {
            get => _parentParentViewModel;
            set => _parentParentViewModel = value;
        }
    
        public ObservableCollection<FileType> Items
        {
            get => ManagerReader.ListToObservable(_parentParentViewModel.Model.ModelNavigationBar.DirectoryPointer.ChildrenFiles);
            set
            {
                var val = ManagerReader.ListToObservable(_parentParentViewModel.Model.ModelNavigationBar.DirectoryPointer.ChildrenFiles);
                this.RaiseAndSetIfChanged(ref val, value);
            }
        }

    
        public PathsBarViewModel(PathsBar attachedView)
        {
            _model = null;
            _modelPathsBar = null;
            AttachedView = attachedView;
        }

    }
}
