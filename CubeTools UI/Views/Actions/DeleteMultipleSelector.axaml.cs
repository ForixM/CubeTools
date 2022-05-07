using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.Pointers;

namespace CubeTools_UI.Views.Actions
{
    public class DeleteMultipleSelector : UserControl
    {
        private readonly DeleteMultiplePopUp _main;
        
        #region Variables
        
        public readonly FileType Pointer;

        private readonly CheckBox _checkBox;
        private readonly TextBlock _name;
        
        #endregion
        
        #region Init
        
        public DeleteMultipleSelector()
        {
            InitializeComponent();
            _name = this.FindControl<TextBlock>("Name");
            _checkBox = this.FindControl<CheckBox>("CheckBox");
            Pointer = FileType.NullPointer;
        }

        public DeleteMultipleSelector(FileType pointer, DeleteMultiplePopUp main) : this()
        {
            Pointer = pointer;
            _main = main;
            _name.Text = pointer.Name;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion
        
        #region Events

        private void OnCheckBoxChecked(object? sender, RoutedEventArgs e) => _main.Selected.Add(this);
        private void OnCheckBoxUnchecked(object? sender, RoutedEventArgs e) => _main.Selected.Remove(this);

        #endregion

    }
}