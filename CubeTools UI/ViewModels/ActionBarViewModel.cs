using System.Collections.Generic;
using Avalonia.Controls;
using CubeTools_UI.Views;

namespace CubeTools_UI.ViewModels
{
    public class ActionBarViewModel
    {
        #region Models Variables
        
        public List<ListBoxItem> SelectedXaml;
        public List<ListBoxItem> CopiedXaml;
        public List<ListBoxItem> CutXaml;
        
        #endregion
        
        #region References
        
        public MainWindowViewModel ParentViewModel;
        public ActionBar AttachedView;
        
        #endregion

        // CTOR
        public ActionBarViewModel(ActionBar attachedView)
        {
            AttachedView = attachedView;
            SelectedXaml = new List<ListBoxItem>();
            CopiedXaml = new List<ListBoxItem>();
            CutXaml = new List<ListBoxItem>();
        }

    }
}

