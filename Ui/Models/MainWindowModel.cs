// CubeTools UIs Imports

using System.Collections.Generic;
using Avalonia.Input;
using Ui.Views;

namespace Ui.Models
{
    public class MainWindowModel
    {
        public MainWindow View;
        public List<Key> KeysPressed;

        #region Children ViewModels

        public readonly LocalModel ModelLocal;
        public readonly LinkBarModel ModelLinkBar;

        #endregion

        #region Init
        
        public MainWindowModel()
        {
            KeysPressed = new List<Key>();
            // NavigationBar
            ModelLinkBar = LinkBar.LastModel;
            // PathsBar
            ModelLocal = Local.LastModel;
            // Referencing THIS
            ModelLocal.ParentModel = this;
            ModelLinkBar.ParentModel = this;
        }

        public MainWindowModel(MainWindow view) : this()
        {
            View = view;
        }

        #endregion
    }
}