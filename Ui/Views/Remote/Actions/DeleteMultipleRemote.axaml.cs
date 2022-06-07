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
using Library;
using Ui.Views.Error;
using Pointer = Library.Pointer;

namespace Ui.Views.Remote.Actions
{
    public class DeleteMultipleRemote : Window
    {
        private readonly MainWindowRemote _main;
        public readonly StackPanel GeneratorDisplay;

        public List<DeleteMultipleSelectorRemote> Selected;
        private List<Pointer> _pointers;

        #region Init
        
        public DeleteMultipleRemote()
        {
            InitializeComponent();
            
            GeneratorDisplay = this.FindControl<StackPanel>("GeneratorDisplay");
            Selected = new List<DeleteMultipleSelectorRemote>();
            _pointers = new List<Pointer>();
        }
        public DeleteMultipleRemote(MainWindowRemote main, List<Pointer> pointer) : this()
        {
            _main = main;
            _pointers = pointer;
            // Init display
            foreach (var ft in _pointers) GeneratorDisplay.Children.Add(new DeleteMultipleSelectorRemote(ft, this));
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
                    foreach (DeleteMultipleSelectorRemote selector in GeneratorDisplay.Children.Where(control => control is DeleteMultipleSelectorRemote @selector &&
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
            foreach (DeleteMultipleSelectorRemote selector in GeneratorDisplay.Children.Where(control => control is DeleteMultipleSelectorRemote @selector &&
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
        
        /// <summary>
        /// Perform the action
        /// </summary>
        private void DeletePointers()
        {
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
                    new ErrorBase(e).ShowDialog<object>(_main);
                }
            }
        }
    }
}