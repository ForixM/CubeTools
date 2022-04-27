using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using CubeTools_UI.Models;
using Library.ManagerReader;

namespace CubeTools_UI.Views.PopUps
{
    public class SearchPopUp : Window
    {
        private readonly ActionBarModel? Model;
        private readonly TextBox TextEntered;

        #region Init
        
        public SearchPopUp()
        {
            InitializeComponent();
            TextEntered = this.FindControl<TextBox>("TextEntered");
            Model = null;
        }
        public SearchPopUp(ActionBarModel vm) : this()
        {
            Model = vm;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion

        #region Events
        
        private void SearchClick(object? sender, RoutedEventArgs e)
        {
            if (sender is Button && Model?.ParentModel != null)
                SearchList();
            Close();
        }

        private void SearchEnter(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter && Model?.ParentModel != null)
            {
                SearchList();
                Close();
            }
        }
        
        private void OnKeyPressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter && Model?.ParentModel != null)
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
            Model!.ParentModel.ModelPathsBar.ReloadPath(ManagerReader.FastSearchByName(Model.ParentModel.ModelNavigationBar.DirectoryPointer.Path,
                TextEntered.Text, 100).ToList());
        }
        
    }
}