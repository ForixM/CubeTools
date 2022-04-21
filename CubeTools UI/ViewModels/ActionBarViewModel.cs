using System.Collections.Generic;
using CubeTools_UI.Views;
using Library.Pointers;

namespace CubeTools_UI.ViewModels
{
    public class ActionBarViewModel
    {
        #region Models Variables
        
        private List<FileType> _selected;
        private List<FileType> _copied;
        private List<FileType> _cut;
        public List<FileType> SelectedXaml
        {
            get => _selected;
            set => _selected = value;
        }
        public List<FileType> CopiedXaml
        {
            get => _copied;
            set => _copied = value;
        }
        public List<FileType> CutXaml
        {
            get => _cut;
            set => _cut = value;
        }
        #endregion
        
        #region References
        
        public MainWindowViewModel? ParentViewModel;
        public ActionBar AttachedView;
        
        #endregion

        // CTOR
        public ActionBarViewModel(ActionBar attachedView)
        {
            AttachedView = attachedView;
            _selected = new List<FileType>();
            _copied = new List<FileType>();
            _cut = new List<FileType>();
        }

    }
}

