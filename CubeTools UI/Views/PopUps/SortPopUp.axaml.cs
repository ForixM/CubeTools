using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using Library.ManagerReader;

namespace CubeTools_UI.Views.PopUps
{
    public class SortPopUp : Window
    {
        private readonly MainWindowModel? _main;

        #region Init
        
        public SortPopUp()
        {
            InitializeComponent();
            _main = null;
        }
        
        public SortPopUp(MainWindowModel main) : this()
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
            _main!.ModelNavigationBar.DirectoryPointer.ChildrenFiles = ManagerReader.SortByName(_main.ModelNavigationBar.DirectoryPointer.ChildrenFiles);
            _main.ModelPathsBar.ReloadPath(_main.ModelNavigationBar.DirectoryPointer.ChildrenFiles);
            Close();
        }
        
        private void OnTypeClick(object? sender, RoutedEventArgs e)
        {
            _main!.ModelNavigationBar.DirectoryPointer.ChildrenFiles = ManagerReader.SortByType(_main.ModelNavigationBar.DirectoryPointer.ChildrenFiles);
            _main.ModelPathsBar.ReloadPath(_main.ModelNavigationBar.DirectoryPointer.ChildrenFiles);
            Close();
        }
        
        private void OnSizeClick(object? sender, RoutedEventArgs e)
        {
            _main!.ModelNavigationBar.DirectoryPointer.ChildrenFiles = ManagerReader.SortBySize(_main.ModelNavigationBar.DirectoryPointer.ChildrenFiles);
            _main.ModelPathsBar.ReloadPath(_main.ModelNavigationBar.DirectoryPointer.ChildrenFiles);
            Close();
        }
        
        private void OnDateClick(object? sender, RoutedEventArgs e)
        {
            _main!.ModelNavigationBar.DirectoryPointer.ChildrenFiles = ManagerReader.SortByModifiedDate(_main.ModelNavigationBar.DirectoryPointer.ChildrenFiles);
            _main.ModelPathsBar.ReloadPath(_main.ModelNavigationBar.DirectoryPointer.ChildrenFiles);
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