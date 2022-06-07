using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Library.ManagerReader;
using Ui.Views.Settings.Generators.SingleObject;

namespace Ui.Views.Settings.Generators
{
    public class ShortcutsGenerator : UserControl
    {
        public StackPanel Generator;
        
        public ShortcutsGenerator()
        {
            InitializeComponent();
            Generator = this.FindControl<StackPanel>("Generator");
            foreach (var shortcut in ConfigLoader.ConfigLoader.Settings.Shortcuts)
                Generator.Children.Add(new ShortcutObject(shortcut.Key));
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
