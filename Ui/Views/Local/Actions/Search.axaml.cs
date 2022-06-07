using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Library.ManagerReader;

namespace Ui.Views.Local.Actions
{
    public class Search : Window
    {
        private readonly Local? _main;
        private readonly TextBox _textEntered;

        #region Init
        
        public Search()
        {
            InitializeComponent();
            _textEntered = this.FindControl<TextBox>("TextEntered");
        }
        public Search(Local main) : this()
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
                var children = new List<Library.LocalPointer>();
                Task.Run(() =>
                {
                    children = ManagerReader.FastSearchByName(_main.NavigationBarView.FolderLocalPointer.Path,
                        _textEntered.Text, 100).ToList();
                }).GetAwaiter().OnCompleted(() =>
                {
                    _main.Refresh(children);
                });

            });
            
        }
        
    }
}