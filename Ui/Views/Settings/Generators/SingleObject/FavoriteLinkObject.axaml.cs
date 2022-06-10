using System.Collections.Generic;
using System.IO;
using System.Xml;
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
            if (ConfigLoader.ConfigLoader.Settings.Links.ContainsKey(_name.Text))
            {
                int i = 1;
                string tmp = _name.Text + 1;
                while (ConfigLoader.ConfigLoader.Settings.Links.ContainsKey(tmp))
                {
                    tmp = tmp.Remove(tmp.Length - 1, 1) + i;
                    i++;
                }
                _lastKey = tmp;
                ConfigLoader.ConfigLoader.Settings.Links.Add(tmp, _path.Text);
            }
            else
            {
                ConfigLoader.ConfigLoader.Settings.Links.Add(_name.Text, _path.Text);
                _lastKey = _name.Text;
            }
        }

        private void OnDeleteClick(object? sender, RoutedEventArgs e)
        {
            _main.Generator.Children.Remove(this);
            ConfigLoader.ConfigLoader.Settings.Links.Remove(_lastKey);
        }
    }
}
