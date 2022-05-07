using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Views.Settings.Generators;

namespace CubeTools_UI.Views.Settings
{
    public class SettingsWindow : Window
    {
        public PackGenerator PackGeneratorXaml;
        
        public SettingsWindow()
        {
            InitializeComponent();
            PackGeneratorXaml = this.FindControl<PackGenerator>("PackGenerator");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ReloadToDefaultStyle(object? sender, RoutedEventArgs e)
        {
            
        }

        private void Save(object? sender, RoutedEventArgs e)
        {
            ConfigLoader.ConfigLoader.SaveConfiguration(ConfigLoader.ConfigLoader.Settings.LoadedJson);
        }
        private void Quit(object? sender, RoutedEventArgs e) => Close();
    }
}
