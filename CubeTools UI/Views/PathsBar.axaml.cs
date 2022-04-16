using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using Library.Pointers;

namespace CubeTools_UI.Views
{
    public class PathsBar : UserControl
    {
        public static PathsBarViewModel ViewModel;
        public PathsBar()
        {
            InitializeComponent();
            ViewModel = new PathsBarViewModel();
            DataContext = ViewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnSelected(object? sender, RoutedEventArgs e)
        {
            if (((Button) e.Source!)?.DataContext is FileType context)
            {
                ViewModel.ModelXaml.ModelActionBar.SelectedXaml.Clear();
                ViewModel.ModelXaml.ModelActionBar.SelectedXaml.Add(context);
            }
        }
    }
}
