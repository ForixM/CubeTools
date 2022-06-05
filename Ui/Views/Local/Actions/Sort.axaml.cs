using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerReader;

namespace Ui.Views.Local.Actions
{
    public class Sort : Window
    {
        private readonly Local? _main;

        #region Init
        
        public Sort()
        {
            InitializeComponent();
            _main = null;
        }
        
        public Sort(Local main) : this()
        {
            _main = main;
        }
        
        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events

        private void OnNameClick(object? sender, RoutedEventArgs e)
        {
            _main?.Refresh( ManagerReader.SortByName(_main.NavigationBarView.FolderPointer.ChildrenFiles));
            Close();
        }
        
        private void OnTypeClick(object? sender, RoutedEventArgs e)
        {
            _main?.Refresh( ManagerReader.SortByType(_main.NavigationBarView.FolderPointer.ChildrenFiles));
            Close();
        }
        
        private void OnSizeClick(object? sender, RoutedEventArgs e)
        {
            _main?.Refresh( ManagerReader.SortBySize(_main.NavigationBarView.FolderPointer.ChildrenFiles));
            Close();
        }
        
        private void OnDateClick(object? sender, RoutedEventArgs e)
        {
            _main?.Refresh( ManagerReader.SortByModifiedDate(_main.NavigationBarView.FolderPointer.ChildrenFiles));
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