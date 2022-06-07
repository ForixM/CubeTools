using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library;
using Ui.Views.Remote.Actions;

namespace Ui.Views.Local.Actions
{
    public class DeleteMultipleSelector : UserControl
    {
        private readonly DeleteMultiple _main;
        
        #region Variables
        
        public readonly LocalPointer LocalPointer;

        private readonly CheckBox _checkBox;
        private readonly TextBlock _name;
        
        #endregion
        
        #region Init
        
        public DeleteMultipleSelector()
        {
            InitializeComponent();
            _name = this.FindControl<TextBlock>("Name");
            _checkBox = this.FindControl<CheckBox>("CheckBox");
            LocalPointer = LocalPointer.NullLocalPointer;
        }

        public DeleteMultipleSelector(LocalPointer localPointer, DeleteMultiple main) : this()
        {
            LocalPointer = localPointer;
            _main = main;
            _name.Text = localPointer.Name;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion
        
        #region Events

        private void OnCheckBoxChecked(object? sender, RoutedEventArgs e) => _main.Selected.Add(this);
        private void OnCheckBoxUnchecked(object? sender, RoutedEventArgs e) => _main.Selected.Remove(this);

        #endregion

    }
}
