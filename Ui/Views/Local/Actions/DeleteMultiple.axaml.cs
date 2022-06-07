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
using Library;
using Library.ManagerExceptions;
using Ui.Views.Error;

namespace Ui.Views.Local.Actions
{
    public class DeleteMultiple : Window
    {
        private readonly Local? _main;
        public readonly StackPanel GeneratorDisplay;

        public List<DeleteMultipleSelector> Selected;
        private List<LocalPointer> _pointers;

        #region Init

        public DeleteMultiple()
        {
            InitializeComponent();

            GeneratorDisplay = this.FindControl<StackPanel>("GeneratorDisplay");
            Selected = new List<DeleteMultipleSelector>();
            _pointers = new List<LocalPointer>();
        }

        public DeleteMultiple(Local main, List<LocalPointer> pointer) : this()
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
                    foreach (DeleteMultipleSelector selector in GeneratorDisplay.Children.Where(control =>
                                 control is DeleteMultipleSelector @selector &&
                                 (File.Exists(selector.LocalPointer.Path) || Directory.Exists(selector.LocalPointer.Path))))
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
            foreach (DeleteMultipleSelector selector in GeneratorDisplay.Children.Where(control =>
                         control is DeleteMultipleSelector @selector &&
                         (File.Exists(selector.LocalPointer.Path) || Directory.Exists(selector.LocalPointer.Path))))
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
            foreach (var pointer in Selected.Select(pointer => pointer.LocalPointer))
            {
                try
                {
                    Dispatcher.UIThread.Post(
                        () => { Task.Run(pointer.Delete).GetAwaiter().OnCompleted(_main!.Refresh); },
                        DispatcherPriority.MaxValue);
                }
                catch (Exception exception)
                {
                    if (exception is ManagerException @managerException)
                    {
                        @managerException.Errorstd = $"Unable to delete {pointer.Name}";
                        new ErrorBase(@managerException).ShowDialog<object>(_main!.Main);
                    }
                }
            }
        }
    }
}