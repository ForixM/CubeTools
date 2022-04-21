using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using Library.ManagerReader;

namespace CubeTools_UI.Views
{
    public class SearchPopUp : Window
    {
        private ActionBarViewModel ParentViewModel;

        private TextBox TextEntered;
        
        public SearchPopUp(ActionBarViewModel vm)
        {
            InitializeComponent();
            ParentViewModel = vm;
            TextEntered = this.FindControl<TextBox>("TextEntered");
        }
        
        public SearchPopUp()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Search_OnClick(object? sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                ParentViewModel.ModelXaml.ViewModel.ViewModelPathsBar.AttachedView.ItemsXaml.Items = 
                                ManagerReader.ListToObservable(ManagerReader.FastSearchByName(ParentViewModel.ModelXaml.ModelNavigationBar.DirectoryPointer.Path, TextEntered.Text, 25).ToList());

            }
        }
    }
}