using System;
using System.Collections.ObjectModel;
using CubeTools_UI.Models;
using CubeTools_UI.Views;
using Library.ManagerReader;
using Library.Pointers;
using ReactiveUI;

namespace CubeTools_UI.ViewModels
{
    
    public class PathsBarViewModel : BaseViewModel
    {

        public PathsBar AttachedView;

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
