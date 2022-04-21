using System.Collections.Generic;
using CubeTools_UI.Views;
using ReactiveUI;

namespace CubeTools_UI.ViewModels
{
    public class LinkBarViewModel : ReactiveObject
    {
        #region Models Variables

        // MODELS VARIABLE 
        public Dictionary<string, string> StaticPaths;

        #endregion

        #region References
        
        public MainWindowViewModel? ParentViewModel;
        public LinkBar AttachedView;
        
        #endregion
        
        public LinkBarViewModel(LinkBar attachedView)
        {
            AttachedView = attachedView;
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
