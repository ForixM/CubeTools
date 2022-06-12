using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Ui.Views.Settings.ShortcutsSaver
{
    public class ShortcutsSaver : Window
    {
        private string _shortcut;
        private List<Key> _keys;
        private TextBlock _display;
        private TextBlock _lastDisplay;

        public ShortcutsSaver()
        {
            InitializeComponent();
            _keys = new List<Key>();
            _display = this.FindControl<TextBlock>("Display");
            _lastDisplay = this.FindControl<TextBlock>("LastDisplay");
            SystemDecorations = SystemDecorations.BorderOnly;
            _shortcut = "";
        }

        public ShortcutsSaver(string shortcut) : this()
        {
            _shortcut = shortcut;
            _lastDisplay.Text = "Last Shortcut : " + ConfigLoader.ConfigLoader.Settings.Shortcuts![shortcut].Aggregate("", (current, key) => current + (Enum.GetName(typeof(Key), key) + "+"));
            Title = "Shortcuts : " + shortcut ;
            UpdateText();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void UpdateText() =>
            _display.Text = _keys.Aggregate("", (current, key) => current + (Enum.GetName(typeof(Key), key) + "+"));

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
            if (!_keys.Contains(e.Key))
            {
                _keys.Add(e.Key);
                UpdateText();
            }
        }

        private void Clear(object? sender, RoutedEventArgs e)
        {
            _keys.Clear();
            UpdateText();
        }

        private void Cancel(object? sender, RoutedEventArgs e) => Close();

        private void Valid(object? sender, RoutedEventArgs e)
        {
            ConfigLoader.ConfigLoader.Settings.Shortcuts[_shortcut] = _keys;
            Close();
        }

        private void OnLostFocus(object? sender, RoutedEventArgs e)
        {
            if (sender != this) Close(null);
        }

    }
}