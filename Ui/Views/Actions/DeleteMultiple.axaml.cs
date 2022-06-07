using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Library.ManagerExceptions;
using Ui.Views.Error;
using Pointer = Library.Pointer;

namespace Ui.Views.Actions
{
    public class DeleteMultiple : Window
    {
        private readonly OneClient? _main;
        public readonly StackPanel GeneratorDisplay;

        public List<DeleteMultipleSelector> Selected;
        private List<Pointer> _pointers;

        #region Init

        public DeleteMultiple()
        {
            InitializeComponent();
            GeneratorDisplay = this.FindControl<StackPanel>("GeneratorDisplay");
            Selected = new List<DeleteMultipleSelector>();
            _pointers = new List<Pointer>();
        }

        public DeleteMultiple(OneClient main, List<Pointer> pointer) : this()
        {
            _main = main;
            _pointers = pointer;
            // Init display
            foreach (var ft in _pointers) GeneratorDisplay.Children.Add(new DeleteMultipleSelector(ft, this));
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
                                 (_main?.Client.GetItem(selector.Pointer.Path, true) is not null)))
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
            foreach (DeleteMultipleSelector selector in GeneratorDisplay.Children.Where(control => control is DeleteMultipleSelector))
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
        
        /// <summary>
        /// Perform the action
        /// </summary>
        private void DeletePointers()
        {
            if (_main is null) return;
            
            foreach (var pointer in Selected.Select(pointer => pointer.Pointer))
            {
                try
                {
                    Dispatcher.UIThread.Post(
                        () => { Task.Run(() => _main.Client.Delete(pointer)).GetAwaiter().OnCompleted(_main.Refresh); },
                        DispatcherPriority.MaxValue);
                }
                catch (ManagerException e)
                {
                    new ErrorBase(e).ShowDialog<object>(_main.Main);
                }
            }
        }
    }
}