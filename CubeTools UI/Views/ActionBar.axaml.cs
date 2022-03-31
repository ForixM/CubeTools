using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;

namespace CubeTools_UI.Views
{
    public partial class ActionBar : UserControl
    {
        public static MainWindowViewModel? mainWindowViewModel;
        public ActionBar()
        {
            InitializeComponent();
            if (mainWindowViewModel != null)
                DataContext = new ActionBarViewModel(mainWindowViewModel);
            else 
                DataContext = new ActionBar(new MainWindowViewModel());
        }
        public ActionBar(MainWindowViewModel parent)
        {
            InitializeComponent();
            if (mainWindowViewModel != null)
                DataContext = new ActionBarViewModel(mainWindowViewModel);
            else
                DataContext = new ActionBarViewModel(parent);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static void InitializeViewReferences(MainWindowViewModel mwvm)
        {
            mainWindowViewModel = mwvm;
        }
    }
}
