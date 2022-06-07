using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ui.Views.Settings.Generators.SingleObject;

namespace Ui.Views.Settings.Generators
{
    public class LinksGenerator : UserControl
    {
        public StackPanel Generator;
        
        public LinksGenerator()
        {
            InitializeComponent();
            Generator = this.FindControl<StackPanel>("Generator");
            foreach (var key in ConfigLoader.ConfigLoader.Settings.Links.Keys)
                Generator.Children.Add(new FavoriteLinkObject(key, ConfigLoader.ConfigLoader.Settings.Links[key]));
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}
