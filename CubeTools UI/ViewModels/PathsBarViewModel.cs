using System.Collections.ObjectModel;
using CubeTools_UI.Views;
using Library.ManagerReader;
using Library.Pointers;

namespace CubeTools_UI.ViewModels
{
    
    public class PathsBarViewModel : BaseViewModel
    {

        #region Models Varaibles
        
        public ObservableCollection<FileType> Items
        {
            get => ParentViewModel == null ? new ObservableCollection<FileType>() : ManagerReader.ListToObservable(ParentViewModel.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            set => AttachedView.ItemsXaml.Items = value;
        }
        
        #endregion

        #region References
        
        public PathsBar AttachedView;
        public MainWindowViewModel? ParentViewModel;
        
        #endregion
        
        public PathsBarViewModel(PathsBar attachedView)
        {
            AttachedView = attachedView;
        }

    }
}
