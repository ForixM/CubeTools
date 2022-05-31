using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Library.ManagerReader;

namespace Ui.Views.Settings.Generators
{
    public class PackGenerator : UserControl
    {
        public ComboBox Generator;
        
        public PackGenerator()
        {
            InitializeComponent();
            Generator = this.FindControl<ComboBox>("Generator");
            var packs = Directory.EnumerateDirectories(ConfigLoader.ConfigLoader.Settings.AppPath + "/Assets");
            var packsControl = new List<Control>();
            foreach (var pack in packs)
                packsControl.Add(new TextBlock{Text = ManagerReader.GetPathToName(pack)});
            Generator.Items = packsControl;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (ConfigLoader.ConfigLoader.Settings.Styles != null && e.AddedItems.Count == 1 &&
                e.AddedItems[0] is TextBlock @block)
                ConfigLoader.ConfigLoader.Settings.Styles.Pack = @block.Text;
        }
    }
}
