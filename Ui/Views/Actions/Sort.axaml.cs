using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerReader;

namespace Ui.Views.Actions
{
    public class Sort : Window
    {
        private readonly ClientUI? _main;

        #region Init
        
        public Sort()
        {
            InitializeComponent();
            _main = null;
        }
        
        public Sort(ClientUI main) : this()
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

        private void OnKeyReleased(object? sender, KeyEventArgs e)
        {
            if (_main.Main is MainWindow window)
            {
                window.KeysPressed.Remove(e.Key);
            }
            else if (_main.Main is MainWindowRemote windowRemote)
            {
                windowRemote.KeysPressed.Remove(e.Key);
            };
        }
    }
}