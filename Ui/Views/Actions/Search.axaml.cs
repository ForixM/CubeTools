using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Library.ManagerReader;
using Pointer = Library.Pointer;

namespace Ui.Views.Actions
{
    public class Search : Window
    {
        private readonly ClientUI? _main;
        private readonly TextBox _textEntered;

        #region Init
        
        public Search()
        {
            InitializeComponent();
            _textEntered = this.FindControl<TextBox>("TextEntered");
        }
        public Search(ClientUI main) : this()
        {
            _main = main;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events
        
        private void SearchClick(object? sender, RoutedEventArgs e)
        {
            if (sender is Button)
                SearchList();
            Close();
        }

        private void SearchEnter(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter)
            {
                SearchList();
                Close();
            }
        }
        
        private void OnKeyPressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter)
            {
                SearchList();
                Close();
            }
            else if (e.Key is Key.Escape)
                Close();
        }
        
        #endregion

        private void SearchList()
        {
            Dispatcher.UIThread.Post(() =>
            {
                var children = new List<Library.Pointer>();
                Task.Run(() =>
                {
                    children.Clear();
                    children.AddRange(ManagerReader.FastSearchByName(_main.Client.CurrentFolder.Path, _textEntered.Text, 100).Cast<Pointer>());
                }).GetAwaiter().OnCompleted(() =>
                {
                    _main.Refresh(children);
                });

            });
            
        }

        private void OnKeyReleased(object? sender, KeyEventArgs e)
        {
            if (_main.Main is MainWindow window)
            {
                MainWindow.KeysPressed.Remove(e.Key);
            }
            else if (_main.Main is MainWindowRemote windowRemote)
            {
                windowRemote.KeysPressed.Remove(e.Key);
            };
        }
    }
}