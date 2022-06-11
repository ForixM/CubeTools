using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Ui.Views.Settings.Generators.SingleObject;

namespace Ui.Views.Settings
{
    public class SettingsWindow : Window
    {

        #region Variables

        private bool _isClosed;

        public ComboBox DarkPackGenerator;
        public ComboBox LightPackGenerator;
        public StackPanel LinksGenerator;
        public StackPanel ShortcutsGenerator;
        public StackPanel FtpGenerator;

        #endregion

        #region Init

        public SettingsWindow()
        {
            InitializeComponent();
            // Getting References
            DarkPackGenerator = this.FindControl<ComboBox>("DarkPackGenerator");
            LightPackGenerator = this.FindControl<ComboBox>("LightPackGenerator");
            LinksGenerator = this.FindControl<StackPanel>("LinksGenerator");
            ShortcutsGenerator = this.FindControl<StackPanel>("ShortcutsGenerator");
            FtpGenerator = this.FindControl<StackPanel>("FtpGenerator");
            
            // Refreshing
            RefreshFtpGenerator();
            RefreshLinksGenerator();
            RefreshShortcutsGenerator();
            RefreshDarkPackGenerator();
            RefreshLightPackGenerator();
            
            // Launching Workers
            new Thread(FtpGeneratorRefresher).Start();
            new Thread(ShortcutsGeneratorRefresher).Start();
            new Thread(LinksGeneratorRefresher).Start();
            new Thread(DarkPackGeneratorRefresher).Start();
            new Thread(LightPackGeneratorRefresher).Start();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        #endregion

        #region Events

        private void Save(object? sender, RoutedEventArgs e)
        {
            ConfigLoader.ConfigLoader.SaveConfiguration();
            Close();
        }
        
        private void Quit(object? sender, RoutedEventArgs e) => Close();
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    Save(this, e);
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }
        
        private void CreateLink(object? sender, RoutedEventArgs e)
        {
            string key = "New Link1";
            int i = 1;
            while (ConfigLoader.ConfigLoader.Settings.Links.ContainsKey(key))
            {
                key = key.Remove(key.Length - 1, 1) + i;
                i++;
            }
            ConfigLoader.ConfigLoader.Settings.Links.Add(key, "");
            LinksGenerator.Children.Add(new LinkSettingObject(key, "", this));
        }

        
        private void OnClosing(object? sender, CancelEventArgs e) => _isClosed = true;

        private void OnLightSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1 && e.AddedItems[0] is TextBlock @block)
            {
                LightPackGenerator.SelectedItem = @block;
                LightPackGenerator.PlaceholderText = @block.Text;
                ConfigLoader.ConfigLoader.Settings.Styles.FolderLight = @block.Text;
                RefreshLightPackGenerator();
            }
        }

        private void OnDarkSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1 && e.AddedItems[0] is TextBlock @block)
            {
                DarkPackGenerator.SelectedItem = @block.Text;
                ConfigLoader.ConfigLoader.Settings.Styles.FolderDark = @block.Text;
                RefreshDarkPackGenerator();
            }
        }

        #endregion

        #region Refreshers

        private void RefreshLinksGenerator()
        {
            if (ConfigLoader.ConfigLoader.Settings.Links is null) return;
            LinksGenerator.Children.Clear();
            foreach (var key in ConfigLoader.ConfigLoader.Settings.Links.Keys)
                LinksGenerator.Children.Add(new LinkSettingObject(key, ConfigLoader.ConfigLoader.Settings.Links[key], this));
        }

        private void RefreshShortcutsGenerator()
        {
            if (ConfigLoader.ConfigLoader.Settings.Shortcuts is null) return;
            ShortcutsGenerator.Children.Clear();
            foreach (var key in ConfigLoader.ConfigLoader.Settings.Shortcuts.Keys)
                ShortcutsGenerator.Children.Add(new ShortcutSettingObject(key));
        }

        private void RefreshFtpGenerator()
        {
            if (ConfigLoader.ConfigLoader.Settings.Ftp is null) return;
            FtpGenerator.Children.Clear();
            foreach (var server in ConfigLoader.ConfigLoader.Settings.Ftp.Servers)
                FtpGenerator.Children.Add(new FtpSettingObject(server));
        }

        private void RefreshDarkPackGenerator()
        {
            DarkPackGenerator.SelectedItem =
                Directory.Exists(ConfigLoader.ConfigLoader.Settings.AppPath + "/Assets/" +
                                 ConfigLoader.ConfigLoader.Settings.Styles.FolderDark)
                    ? ConfigLoader.ConfigLoader.Settings.Styles.FolderDark
                    : "";
            DarkPackGenerator.Items = Directory
                .EnumerateDirectories(ConfigLoader.ConfigLoader.Settings.AppPath + "/Assets")
                .Select(enumerateDirectory => new TextBlock {Text = Path.GetFileName(enumerateDirectory)});
        }

        private void RefreshLightPackGenerator()
        {
            DarkPackGenerator.SelectedItem =
                Directory.Exists(ConfigLoader.ConfigLoader.Settings.AppPath + "/Assets/" +
                                 ConfigLoader.ConfigLoader.Settings.Styles.FolderLight)
                    ? ConfigLoader.ConfigLoader.Settings.Styles.FolderLight
                    : "";
            LightPackGenerator.Items = Directory
                .EnumerateDirectories(ConfigLoader.ConfigLoader.Settings.AppPath + "/Assets")
                .Select(enumerateDirectory => new TextBlock {Text = Path.GetFileName(enumerateDirectory)});
        }

        #endregion
        
        #region Workers

        private void FtpGeneratorRefresher()
        {
            while (!_isClosed)
            {
                Thread.Sleep(2000);
                Dispatcher.UIThread.Post(RefreshFtpGenerator);
            }
        }

        private void ShortcutsGeneratorRefresher()
        {
            while (!_isClosed)
            {
                Thread.Sleep(2000);
                Dispatcher.UIThread.Post(RefreshFtpGenerator);
            }
        }

        private void LinksGeneratorRefresher()
        {
            while (!_isClosed)
            {
                Thread.Sleep(2000);
                Dispatcher.UIThread.Post(RefreshLinksGenerator);
            }
        }

        private void DarkPackGeneratorRefresher()
        {
            while (!_isClosed)
            {
                Thread.Sleep(2000);
                Dispatcher.UIThread.Post(RefreshDarkPackGenerator);
            }
        }
        
        private void LightPackGeneratorRefresher() 
        {
            while (!_isClosed)
            {
                Thread.Sleep(2000);
                Dispatcher.UIThread.Post(RefreshLightPackGenerator);
            }
        }
        #endregion
    }
}
