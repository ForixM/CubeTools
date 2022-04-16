using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using ReactiveUI;

namespace CubeTools_UI.Views
{
    public class ActionBar : UserControl
    {
        public static ActionBarViewModel ViewModel;
        public ActionBar()
        {
            InitializeComponent();
            ViewModel = new ActionBarViewModel();
            DataContext = ViewModel;
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
    }
}
