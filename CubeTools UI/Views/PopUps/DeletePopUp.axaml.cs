using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using Library.Pointers;

namespace CubeTools_UI.Views.PopUps
{
    public class DeletePopUp : Window
    {
        private MainWindowViewModel? ViewModel;
        private FileType _pointer;

        #region Init
        
        public DeletePopUp()
        {
            InitializeComponent();
            ViewModel = null;
        }
        public DeletePopUp(MainWindowViewModel vm, FileType pointer) : this()
        {
            ViewModel = vm;
            _pointer = pointer;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
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
        
        private void OnCancelClicked(object? sender, RoutedEventArgs e)
        {
            Close();
        }
        

        #endregion

        

        private void DeletePointer()
        {
            // Create a new task to delete the pointer
            var task = new Task(() =>
            {
                if (_pointer.IsDir)
                    ManagerWriter.DeleteDir(_pointer);
                else 
                    ManagerWriter.Delete(_pointer);
            });
            // Remove reference from Directory Pointer
            ViewModel.ViewModelNavigationBar.DirectoryPointer.Remove(_pointer);
            // Run Tasks Async
            try
            {
                if (_pointer.Size > 1000000)
                {
                    // Run async task
                    task.Start();
                    // Display loading box
                    var loadingPopUp = new LoadingPopUp((int) ManagerReader.GetFileSize(_pointer), _pointer,true);
                    loadingPopUp.Show();
                    // Close display
                    task.GetAwaiter().OnCompleted(() =>
                    {
                        loadingPopUp.Close();
                        ViewModel.ReloadPath();
                    });
                }
                // Run task sync
                else
                {
                    task.RunSynchronously();
                    ViewModel.ReloadPath();
                }
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = $"Unable to delete {_pointer.Name}";
                    new Views.ErrorPopUp.ErrorPopUp(ViewModel, @managerException).Show();
                }
            }
        }
    }
}