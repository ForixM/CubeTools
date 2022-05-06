using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using Library.ManagerReader;

namespace CubeTools_UI.Views.Actions
{
    public class SearchPopUp : Window
    {
        private readonly ActionBarModel? _model;
        private readonly TextBox _textEntered;

        #region Init
        
        public SearchPopUp()
        {
            InitializeComponent();
            _textEntered = this.FindControl<TextBox>("TextEntered");
        }
        public SearchPopUp(ActionBarModel vm) : this()
        {
            _model = vm;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events
        
        private void SearchClick(object? sender, RoutedEventArgs e)
        {
            if (sender is Button && _model?.ParentModel != null)
                SearchList();
            Close();
        }

        private void SearchEnter(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter && _model?.ParentModel != null)
            {
                SearchList();
                Close();
            }
        }
        
        private void OnKeyPressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter && _model?.ParentModel != null)
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
            _model!.ParentModel.ModelPathsBar.ReloadPath(ManagerReader.FastSearchByName(_model.ParentModel.ModelNavigationBar.DirectoryPointer.Path,
                _textEntered.Text, 100).ToList());
        }
        
    }
}