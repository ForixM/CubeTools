using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using Library.ManagerReader;
using Library.Pointers;

namespace CubeTools_UI.Views.PopUps
{
    public class RenamePopUp : Window
    {
        private TextBox _textEntered;
        private FileType _modifiedPointer;

        public RenamePopUp()
        {
            InitializeComponent();
            _textEntered = this.FindControl<TextBox>("TextEntered");
            _modifiedPointer = FileType.NullPointer;
        }
        public RenamePopUp(FileType ft) : this()
        {
            _modifiedPointer = ft;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void RenameClick(object? sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                // RENAME
            }
        }
    }
}