// CubeTools UIs Imports
using CubeTools_UI.Views;

namespace CubeTools_UI.Models
{
    public class MainWindowModel
    {
        public MainWindow View;
        public static string CubeToolsPath;

        public bool IsCtrlPressed;
        
        #region Children ViewModels

        public readonly LocalModel ModelLocal;
        public readonly LinkBarModel ModelLinkBar;

        #endregion

        #region Init
        
        public MainWindowModel()
        {
            IsCtrlPressed = false;
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