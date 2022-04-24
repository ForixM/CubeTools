using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using CubeTools_UI.Views;
using Library.ManagerReader;
using Library.Pointers;
using ReactiveUI;

namespace CubeTools_UI.ViewModels
{
    
    public class PathsBarViewModel : BaseViewModel
    {
        #region References
        
        public PathsBar AttachedView;
        public MainWindowViewModel? ParentViewModel;
        
        #endregion
        
        public PathsBarViewModel(PathsBar attachedView)
        {
            AttachedView = attachedView;
        }

        public void ReloadPath(List<FileType> list)
        {
            AttachedView.ReloadPath(list);
        }

    }
}
