using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerReader;

namespace Ui.Views.Actions
{
    public class Sort : Window
    {
        private readonly OneClient? _main;

        #region Init
        
        public Sort()
        {
            InitializeComponent();
            _main = null;
        }
        
        public Sort(OneClient main) : this()
        {
            _main = main;
        }
        
        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events

        private void OnNameClick(object? sender, RoutedEventArgs e)
        {
            _main?.Refresh( ManagerReader.SortByName(_main.Client.Children));
            Close();
        }
        
        private void OnTypeClick(object? sender, RoutedEventArgs e)
        {
            _main?.Refresh( ManagerReader.SortByType(_main.Client.Children));
            Close();
        }
        
        private void OnSizeClick(object? sender, RoutedEventArgs e)
        {
            _main?.Refresh( ManagerReader.SortBySize(_main.Client.Children));
            Close();
        }
        
        private void OnDateClick(object? sender, RoutedEventArgs e)
        {
            _main?.Refresh( ManagerReader.SortByModifiedDate(_main.Client.Children));
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