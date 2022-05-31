using System.Collections.Generic;
using Ui.Views;

namespace Ui.Models
{
    public class ActionBarModel
    {
        #region Models Variables
        
        public List<PointerItem> SelectedXaml;
        public List<PointerItem> CopiedXaml;
        public List<PointerItem> CutXaml;
        
        #endregion
        
        #region References
        
        public LocalModel ParentModel;
        public ActionBar View;
        
        #endregion

        // CTOR
        public ActionBarModel(ActionBar view)
        {
            View = view;
            SelectedXaml = new List<PointerItem>();
            CopiedXaml = new List<PointerItem>();
            CutXaml = new List<PointerItem>();
        }

    }
}

