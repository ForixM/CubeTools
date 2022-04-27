using CubeTools_UI.Views;

namespace CubeTools_UI.Models
{
    public class TopBarModel
    {
        public TopBar AttachedView;
        public TopBarModel(TopBar attachedView)
        {
            AttachedView = attachedView;
        }
    }
}
