using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerReader;
using Ui.Models;

namespace Ui.Views.Actions
{
    public class SortPopUp : Window
    {
        private readonly LocalModel? _main;

        #region Init
        
        public SortPopUp()
        {
            InitializeComponent();
            _main = null;
        }
        
        public SortPopUp(LocalModel main) : this()
        {
            _main = main;
        }
        
        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events

        private void OnNameClick(object? sender, RoutedEventArgs e)
        {
            _main!.ModelNavigationBar.FolderPointer.ChildrenFiles = ManagerReader.SortByName(_main.ModelNavigationBar.FolderPointer.ChildrenFiles);
            _main.ModelPathsBar.ReloadPath(_main.ModelNavigationBar.FolderPointer.ChildrenFiles);
            Close();
        }
        
        private void OnTypeClick(object? sender, RoutedEventArgs e)
        {
            _main!.ModelNavigationBar.FolderPointer.ChildrenFiles = ManagerReader.SortByType(_main.ModelNavigationBar.FolderPointer.ChildrenFiles);
            _main.ModelPathsBar.ReloadPath(_main.ModelNavigationBar.FolderPointer.ChildrenFiles);
            Close();
        }
        
        private void OnSizeClick(object? sender, RoutedEventArgs e)
        {
            _main!.ModelNavigationBar.FolderPointer.ChildrenFiles = ManagerReader.SortBySize(_main.ModelNavigationBar.FolderPointer.ChildrenFiles);
            _main.ModelPathsBar.ReloadPath(_main.ModelNavigationBar.FolderPointer.ChildrenFiles);
            Close();
        }
        
        private void OnDateClick(object? sender, RoutedEventArgs e)
        {
            _main!.ModelNavigationBar.FolderPointer.ChildrenFiles = ManagerReader.SortByModifiedDate(_main.ModelNavigationBar.FolderPointer.ChildrenFiles);
            _main.ModelPathsBar.ReloadPath(_main.ModelNavigationBar.FolderPointer.ChildrenFiles);
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