using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using Library.ManagerReader;

namespace CubeTools_UI.Views.PopUps
{
    public class SearchPopUp : Window
    {
        private ActionBarViewModel? ViewModel;
        private TextBox TextEntered;

        #region Init
        
        public SearchPopUp()
        {
            InitializeComponent();
            TextEntered = this.FindControl<TextBox>("TextEntered");
            ViewModel = null;
        }
        public SearchPopUp(ActionBarViewModel vm) : this()
        {
            ViewModel = vm;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion

        #region Events
        
        private void SearchClick(object? sender, RoutedEventArgs e)
        {
            if (sender is Button && ViewModel?.ParentViewModel != null)
                SearchList();
            Close();
        }

        private void SearchEnter(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter && ViewModel?.ParentViewModel != null)
            {
                SearchList();
                Close();
            }
        }
        
        private void OnKeyPressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter && ViewModel?.ParentViewModel != null)
            {
                SearchList();
                Close();
            }
            else if (e.Key is Key.Escape)
                Close();
        }
        
        #endregion

        private void SearchList()
        {
            ViewModel.ParentViewModel.ViewModelPathsBar.ReloadPath(ManagerReader.FastSearchByName(ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer.Path,
                TextEntered.Text, 100).ToList());
        }
        
    }
}