using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using Library.ManagerReader;

namespace CubeTools_UI.Views.PopUps
{
    public class SortPopUp : Window
    {
        private MainWindowViewModel _main;

        #region Init
        
        public SortPopUp()
        {
            InitializeComponent();
            _main = null;
        }
        
        public SortPopUp(MainWindowViewModel main) : this()
        {
            _main = main;
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion

        #region Events

        private void OnNameClick(object? sender, RoutedEventArgs e)
        {
            _main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles = ManagerReader.SortByName(_main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            _main.ViewModelPathsBar.ReloadPath(_main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            Close();
        }
        
        private void OnTypeClick(object? sender, RoutedEventArgs e)
        {
            _main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles = ManagerReader.SortByType(_main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            _main.ViewModelPathsBar.ReloadPath(_main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            Close();
        }
        
        private void OnSizeClick(object? sender, RoutedEventArgs e)
        {
            _main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles = ManagerReader.SortBySize(_main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            _main.ViewModelPathsBar.ReloadPath(_main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            Close();
        }
        
        private void OnDateClick(object? sender, RoutedEventArgs e)
        {
            _main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles = ManagerReader.SortByModifiedDate(_main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            _main.ViewModelPathsBar.ReloadPath(_main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            Close();
        }
        
        private void Cancel(object? sender, RoutedEventArgs e)
        {
            Close();
        }
        
        #endregion

        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }
    }
}