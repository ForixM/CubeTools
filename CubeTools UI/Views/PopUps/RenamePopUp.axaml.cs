using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using Library.ManagerReader;
using Library.Pointers;

namespace CubeTools_UI.Views.PopUps
{
    public class RenamePopUp : Window
    {
        private ActionBarViewModel ParentViewModel;
        
        private TextBox TextEntered;
        
        public RenamePopUp(FileType ft)
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Rename_OnClick(object? sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                ParentViewModel.ModelXaml.ViewModel.ViewModelPathsBar.AttachedView.ItemsXaml.Items = 
                                ManagerReader.ListToObservable(ManagerReader.FastSearchByName(ParentViewModel.ModelXaml.ModelNavigationBar.DirectoryPointer.Path, TextEntered.Text, 25).ToList());

            }
        }
    }
}