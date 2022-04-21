using CubeTools_UI.Views;

namespace CubeTools_UI.ViewModels
{
    public class TopBarViewModel
    {
        public TopBar AttachedView;
        public TopBarViewModel(TopBar attachedView)
        {
            AttachedView = attachedView;
        }
    }
}
