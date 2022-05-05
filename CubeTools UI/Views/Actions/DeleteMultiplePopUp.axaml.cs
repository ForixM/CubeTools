using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using Library.ManagerExceptions;
using Library.ManagerWriter;
using Library.Pointers;

namespace CubeTools_UI.Views.Actions
{
    public class DeleteMultiplePopUp : Window
    {
        private MainWindowModel? Model;
        public StackPanel GeneratorDisplay;

        public List<DeleteMultipleSelector> Selected;
        private List<FileType> _pointers;

        #region Init
        
        public DeleteMultiplePopUp()
        {
            InitializeComponent();
            
            GeneratorDisplay = this.FindControl<StackPanel>("GeneratorDisplay");
            Selected = new List<DeleteMultipleSelector>();
            _pointers = new List<FileType>();
        }
        public DeleteMultiplePopUp(MainWindowModel vm, List<FileType> pointer) : this()
        {
            Model = vm;
            _pointers = pointer;
            // Init display
            foreach (var ft in _pointers)
                GeneratorDisplay.Children.Add(new DeleteMultipleSelector(ft, this));
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        #endregion

        #region Events

        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
            if (e.Key is Key.Enter)
            {
                Selected.Clear();
                foreach (DeleteMultipleSelector selector in GeneratorDisplay.Children.Where(control => control is DeleteMultipleSelector @selector &&
                             (File.Exists(selector.Pointer.Path) || Directory.Exists(selector.Pointer.Path))))
                    Selected.Add(selector);
                DeletePointers();
                Close();
            }
        }
        
        private void OnDeleteAllClick(object? sender, RoutedEventArgs e)
        {
            Selected.Clear();
            foreach (DeleteMultipleSelector selector in GeneratorDisplay.Children.Where(control => control is DeleteMultipleSelector @selector &&
                         (File.Exists(selector.Pointer.Path) || Directory.Exists(selector.Pointer.Path))))
                Selected.Add(selector);
            DeletePointers();
            Close();
        }
        
        private void OnDeleteSelectedClick(object? sender, RoutedEventArgs e)
        {
            DeletePointers();
            Close();
        }
        
        private void OnCancelClicked(object? sender, RoutedEventArgs e) => Close();
        
        private void OnClosing(object? sender, CancelEventArgs e) => Model!.ReloadPath();

        #endregion
        
        private void DeletePointers()
        {
            List<Task> tasks = new List<Task>();
            // Create a new task to delete the pointer
            foreach (var ft in Selected.Select(pointer => pointer.Pointer))
            {
                var task = new Task(() =>
                {
                    if (ft.IsDir) ManagerWriter.DeleteDir(ft);
                    else ManagerWriter.Delete(ft);
                    Model?.ModelNavigationBar.DirectoryPointer.Remove(ft);
                });
                tasks.Add(task);
            }
            // Launch tasks
            foreach (var task in tasks)
            {
                try
                {
                    task.Start();
                }
                catch (ManagerException e)
                {
                    new ErrorPopUp.ErrorPopUp(Model!, e).Show();
                }
            }
        }
    }
}