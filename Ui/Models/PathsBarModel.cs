using System.Collections.Generic;
using Library;
using Ui.Views;

namespace Ui.Models
{
    
    public class PathsBarModel : BaseModel
    {
        #region References
        
        public PathsBar View;
        public LocalModel? ParentModel;
        
        #endregion
        
        public PathsBarModel(PathsBar view)
        {
            View = view;
        }

        public void ReloadPath(List<FilePointer> list)
        {
            View.ReloadPath(list);
        }

    }
}
