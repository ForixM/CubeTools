using System.Collections.Generic;
using CubeTools_UI.Models;
using CubeTools_UI.ViewModels;
using Library.Pointers;

namespace CubeTools_UI.Models
{
    public class MainWindowModel
    {
        // Main ViewModel Reference :
        private MainWindowViewModel _viewModel;
        public MainWindowViewModel ViewModel => _viewModel;
        // ModelXaml Reference :
        public ActionBarModel ModelActionBar;
        public LinkBarModel ModelLinkBar;
        public NavigationBarModel ModelNavigationBar;
        public PathsBarModel ModelPathsBar;
    
        public MainWindowModel(MainWindowViewModel parent)
        {
            _viewModel = parent;
        }

        public void Initialize()
        {
            // Referencing Models
            ModelActionBar = new ActionBarModel(this);
            ModelLinkBar = new LinkBarModel(this);
            ModelNavigationBar = new NavigationBarModel(this);
            ModelPathsBar = new PathsBarModel(this);
        }
    }
}

