using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
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
            _main.ReloadPath(ManagerReader.FastSearchByName(_main.NavigationBarView.FolderPointer.Path, _textEntered.Text, 100).ToList());
        }
        
    }
}