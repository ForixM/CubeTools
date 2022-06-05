using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using LibraryClient;
using Ui.Views.Error;

namespace Ui.Views.Remote.Actions
{
    public class DeleteRemote : Window
    {
        private readonly MainWindowRemote? _main;
        private readonly RemoteItem? _pointer;

        #region Init
        
        public DeleteRemote()
        {
            InitializeComponent();
            _main = null;
            _pointer = null;
        }
        public DeleteRemote(MainWindowRemote main, RemoteItem pointer) : this()
        {
            _main = main;
            _pointer = pointer;
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
    }
}