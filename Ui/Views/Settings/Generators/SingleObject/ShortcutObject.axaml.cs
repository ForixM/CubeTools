using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Ui.Views.Settings.Generators.SingleObject
{
    public class ShortcutObject : UserControl
    {
        private string _shortcutName;
        public ShortcutObject()
        {
            InitializeComponent();
        }
        public ShortcutObject(string shortcutName) : this()
        {
            _shortcutName = shortcutName;
            this.FindControl<TextBlock>("ShortcutName").Text = shortcutName;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void ModifyShortcut(object? sender, RoutedEventArgs e) => new ShortcutsSaver.ShortcutsSaver(_shortcutName).Show();
    }
}
