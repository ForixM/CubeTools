using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Pointer = Library.Pointer;

namespace Ui.Views.Local.Actions
{
    public class Delete : Window
    {
        private readonly Local? _main;
        private readonly Pointer _pointer;

        #region Init
        
        public Delete()
        {
            InitializeComponent();
            _main = null;
            _pointer = Pointer.NullPointer;
        }
        public Delete(Local main, Pointer pointer) : this()
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
            // Remove reference from Directory Pointer
            _main?.NavigationBarView.FolderPointer.Remove(_pointer);
            // Run Tasks Async
            try
            {
                if (_pointer.Size > 1000000)
                {
                    // Close display
                    _pointer.DeleteAsync().GetAwaiter().OnCompleted(() =>
                    {
                        _main?.ReloadPath();
                    });
                }
                // Run task sync
                else
                {
                    _pointer.Delete();
                    _main?.ReloadPath();
                }
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = $"Unable to delete {_pointer.Name}";
                    _main?.SelectErrorPopUp(@managerException);
                }
            }
        }
    }
}