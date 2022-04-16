using System.Collections.Generic;

namespace CubeTools_UI.Models
{
    public class LinkBarModel
    {
        // Main Model
        private MainWindowModel _model;
        // STATIC PATHS
        public Dictionary<string, string> StaticPaths;
        // CTOR
        public LinkBarModel(MainWindowModel model)
        {
            _model = model;
            StaticPaths = new Dictionary<string, string>
            {
                {"Home", "C:/..."},
                {"Desktop", "C:/..."},
                {"Document", "C:/..."},
                {"Download", "C:/"},
                {"Picture", "C:/"},
                {"Trash", "C::"}
            };
        }
    }
}
