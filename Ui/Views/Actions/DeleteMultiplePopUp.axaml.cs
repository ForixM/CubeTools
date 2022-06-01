using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Library.ManagerWriter;
using Library;
using Ui.Models;
using Pointer = Library.Pointer;

namespace Ui.Views.Actions
{
    public class DeleteMultiplePopUp : Window
    {
        private readonly LocalModel? _model;
        public readonly StackPanel GeneratorDisplay;

        public List<DeleteMultipleSelector> Selected;
        private List<Pointer> _pointers;

        #region Init
        
        public DeleteMultiplePopUp()
        {
            InitializeComponent();
            
            GeneratorDisplay = this.FindControl<StackPanel>("GeneratorDisplay");
            Selected = new List<DeleteMultipleSelector>();
            _pointers = new List<Pointer>();
        }
        public DeleteMultiplePopUp(LocalModel vm, List<Pointer> pointer) : this()
        {
            _model = vm;
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
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.Enter:
                {
                    Selected.Clear();
                    foreach (DeleteMultipleSelector selector in GeneratorDisplay.Children.Where(control => control is DeleteMultipleSelector @selector &&
                                 (File.Exists(selector.Pointer.Path) || Directory.Exists(selector.Pointer.Path))))
                        Selected.Add(selector);
                    DeletePointers();
                    Close();
                    break;
                }
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

        #endregion
        
        private void DeletePointers()
        {
            List<Task> tasks = new List<Task>();
            // Create a new task to delete the pointer
            foreach (var ft in Selected.Select(pointer => pointer.Pointer))
            {
                var task = new Task(() =>
                {
                    ft.Delete();
                    _model?.ModelNavigationBar.FolderPointer.Remove(ft);
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
                    _model?.SelectErrorPopUp(e);
                }
            }
        }
    }
}