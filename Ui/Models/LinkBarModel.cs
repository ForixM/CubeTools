using System.Collections.Generic;
using ReactiveUI;
using Ui.Views;

namespace Ui.Models
{
    public class LinkBarModel
    {
        // MODELS VARIABLE 
        public Dictionary<string, string> StaticPaths;

        public MainWindowModel? ParentModel;
        public LinkBar View;
        
        public LinkBarModel(LinkBar view)
        {
            View = view;
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
