using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ConfigLoader.Settings;
using DynamicData;
using Tmds.DBus;

namespace Ui.Views.Settings.Generators.SingleObject
{
    public class FavoriteLinkObject : UserControl
    {

        private LinksGenerator _main;
        
        private TextBox _name;
        private TextBox _path;

        private string _lastKey;

        public FavoriteLinkObject()
        {
            InitializeComponent();
            _name = this.FindControl<TextBox>("Name");
            _path = this.FindControl<TextBox>("Path");
            _lastKey = "";
        }
        public FavoriteLinkObject(string name, string path, LinksGenerator main) : this()
        {
            _name.Text = name;
            _path.Text = path;
            _lastKey = name;
            _main = main;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OnButtonClick(object? sender, RoutedEventArgs e)
        {
            ConfigLoader.ConfigLoader.Settings.Links.Remove(_lastKey);
            ConfigLoader.ConfigLoader.Settings.Links.Add(_name.Text, _path.Text);
            _lastKey = _name.Text;
        }

        private void OnDeleteClick(object? sender, RoutedEventArgs e)
        {
            _main.Generator.Children.Remove(this);
            ConfigLoader.ConfigLoader.Settings.Links.Remove(_lastKey);
        }
    }
}
