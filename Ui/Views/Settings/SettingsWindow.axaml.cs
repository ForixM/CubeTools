using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
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
            ConfigLoader.ConfigLoader.SaveConfiguration();
            Close();
        }
        private void Quit(object? sender, RoutedEventArgs e) => Close();

        private void CreateLink(object? sender, RoutedEventArgs e)
        {
            string key = "New Link";
            int i = 1;
            while (ConfigLoader.ConfigLoader.Settings.Links.ContainsKey(key))
            {
                key = key.Remove(key.Length - 1, 1) + i;
                i++;
            }

            ConfigLoader.ConfigLoader.Settings.Links.Add(key, "");
            LinksGeneratorXaml.Generator.Children.Add(new FavoriteLinkObject(key, "", LinksGeneratorXaml));
        }

        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter) Save(this, e);
            else if (e.Key is Key.Escape) Close();
        }
    }
}
