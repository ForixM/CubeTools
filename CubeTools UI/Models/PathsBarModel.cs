using System.Collections.Generic;
using CubeTools_UI.Views;
using Library.Pointers;
namespace CubeTools_UI.Models
{
    
    public class PathsBarModel : BaseModel
    {
        #region References
        
        public PathsBar View;
        public MainWindowModel? ParentModel;
        
        #endregion
        
        public PathsBarModel(PathsBar view)
        {
            View = view;
        }

        public void ReloadPath(List<FileType> list)
        {
            View.ReloadPath(list);
        }

    }
}
