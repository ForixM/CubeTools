using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Ui.Views.MainWindow.DynamicLinkBar;
using Ui.Views.Settings.Generators;
using Ui.Views.Settings.Generators.SingleObject;

namespace Ui.Views.Settings
{
    public class SettingsWindow : Window
    {
        public PackGenerator PackGeneratorXaml;
        public LinksGenerator LinksGeneratorXaml;
        
        public SettingsWindow()
        {
            InitializeComponent();
            PackGeneratorXaml = this.FindControl<PackGenerator>("PackGenerator");
            LinksGeneratorXaml = this.FindControl<LinksGenerator>("LinksGenerator");
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void Save(object? sender, RoutedEventArgs e)
        {
            ConfigLoader.ConfigLoader.SaveConfiguration(ConfigLoader.ConfigLoader.Settings.LoadedJson);
            Close();
        }
        private void Quit(object? sender, RoutedEventArgs e) => Close();

        private void CreateLink(object? sender, RoutedEventArgs e)
        {
            string key = "New Link";
            int i = 1;
            while (ConfigLoader.ConfigLoader.Settings.Links.ContainsKey(key))
            {
                key.Remove(key.Length - 1, 1);
                key += i;
                i++;
            }

            ConfigLoader.ConfigLoader.Settings.Links.Add(key, "");
            LinksGeneratorXaml.Generator.Children.Add(new FavoriteLinkObject(key, ""));
        }
    }
}
