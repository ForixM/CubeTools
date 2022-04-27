using System.Collections.Generic;
using CubeTools_UI.Views;

namespace CubeTools_UI.Models
{
    public class ActionBarModel
    {
        #region Models Variables
        
        public List<PointerItem> SelectedXaml;
        public List<PointerItem> CopiedXaml;
        public List<PointerItem> CutXaml;
        
        #endregion
        
        #region References
        
        public MainWindowModel ParentModel;
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

