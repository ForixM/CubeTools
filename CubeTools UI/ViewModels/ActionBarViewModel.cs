using System.Collections.Generic;
using Avalonia.Controls;
using CubeTools_UI.Views;

namespace CubeTools_UI.ViewModels
{
    public class ActionBarViewModel
    {
        #region Models Variables
        
        public List<PointerItem> SelectedXaml;
        public List<PointerItem> CopiedXaml;
        public List<PointerItem> CutXaml;
        
        #endregion
        
        #region References
        
        public MainWindowViewModel ParentViewModel;
        public ActionBar AttachedView;
        
        #endregion

        // CTOR
        public ActionBarViewModel(ActionBar attachedView)
        {
            AttachedView = attachedView;
            SelectedXaml = new List<PointerItem>();
            CopiedXaml = new List<PointerItem>();
            CutXaml = new List<PointerItem>();
        }

    }
}

