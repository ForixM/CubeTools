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

namespace Ui.Views.Local.Actions
{
    public class Delete : Window
    {
        private readonly Local? _main;
        private readonly LocalPointer _localPointer;

        #region Init
        
        public Delete()
        {
            InitializeComponent();
            _main = null;
            _localPointer = LocalPointer.NullLocalPointer;
        }
        public Delete(Local main, LocalPointer localPointer) : this()
        {
            _main = main;
            _localPointer = localPointer;
            Title = $"Delete {localPointer.Name} ?";
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
            _main?.NavigationBarView.FolderLocalPointer.Remove(_localPointer);
            // Run Tasks Async
            try
            {
                Dispatcher.UIThread.Post(() =>
                {
                    Task.Run(_localPointer.Delete).GetAwaiter().OnCompleted(_main!.Refresh);
                }, DispatcherPriority.MaxValue);
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = $"Unable to delete {_localPointer.Name}";
                    new ErrorBase(@managerException).ShowDialog<object>(_main!.Main);
                }
            }
        }
    }
}