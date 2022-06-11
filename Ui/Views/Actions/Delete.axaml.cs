using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Library;
using Library.ManagerExceptions;
using Ui.Views.Error;
using Pointer = Library.Pointer;

namespace Ui.Views.Actions
{
    public class Delete : Window
    {
        private readonly ClientUI? _main;
        private readonly Pointer? _pointer;

        #region Init
        
        public Delete()
        {
            InitializeComponent();
            _main = null;
            _pointer = null;
        }
        public Delete(ClientUI main, Pointer pointer) : this()
        {
            _main = main;
            _pointer = pointer;
            Title = $"Delete {pointer.Name} ?";
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events

        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
            if (e.Key is Key.Enter)
            {
                DeletePointer();
                Close();
            }
        }
        
        private void OnDeleteClick(object? sender, RoutedEventArgs e)
        {
            DeletePointer();
            Close();
        }
        
        private void OnCancelClicked(object? sender, RoutedEventArgs e) => Close();

        #endregion
        
        private void DeletePointer()
        {
            if (_main?.Client is null || _pointer is null) return;
            
            try
            {
                _main?.Client.Delete(_pointer);
                _main?.Refresh();
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = $"Unable to delete {_pointer.Name}";
                    new ErrorBase(@managerException).ShowDialog<object>(this);
                }
            }
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
            }
        }
    }
}