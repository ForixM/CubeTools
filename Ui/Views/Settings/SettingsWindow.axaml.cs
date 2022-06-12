using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ConfigLoader.Settings;
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
        public Window _main;
        
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
            new Thread(LinksGeneratorRefresher).Start();
            new Thread(DarkPackGeneratorRefresher).Start();
            new Thread(LightPackGeneratorRefresher).Start();
        }
        
        public SettingsWindow(Window main) : this()
        {
            this._main = main;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        #endregion

        #region Events

        private void Save(object? sender, RoutedEventArgs e)
        {
            ConfigLoader.ConfigLoader.SaveConfiguration();
            ConfigLoader.ConfigLoader.LoadConfiguration(ConfigLoader.ConfigLoader.Settings.LoadedJson);
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
                DarkPackGenerator.SelectedItem = @block;
                DarkPackGenerator.PlaceholderText = @block.Text;
                ConfigLoader.ConfigLoader.Settings.Styles.FolderDark = @block.Text;
                RefreshDarkPackGenerator();
            }
        }

        private void OnKeyReleased(object? sender, KeyEventArgs e)
        {
            if (_main is MainWindow window)
            {
                MainWindow.KeysPressed.Remove(e.Key);
            } 
            else if (_main is MainWindowRemote remote)
            {
                remote.KeysPressed.Remove(e.Key);
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
                Directory.Exists(ConfigLoader.ConfigLoader.AppPath + "/Assets/" +
                                 ConfigLoader.ConfigLoader.Settings.Styles.FolderDark)
                    ? ConfigLoader.ConfigLoader.Settings.Styles.FolderDark
                    : "";
            DarkPackGenerator.Items = Directory
                .EnumerateDirectories(ConfigLoader.ConfigLoader.AppPath + "/Assets")
                .Select(enumerateDirectory => new TextBlock {Text = Path.GetFileName(enumerateDirectory)});
        }

        private void RefreshLightPackGenerator()
        {
            DarkPackGenerator.SelectedItem =
                Directory.Exists(ConfigLoader.ConfigLoader.AppPath + "/Assets/" +
                                 ConfigLoader.ConfigLoader.Settings.Styles.FolderLight)
                    ? ConfigLoader.ConfigLoader.Settings.Styles.FolderLight
                    : "";
            LightPackGenerator.Items = Directory
                .EnumerateDirectories(ConfigLoader.ConfigLoader.AppPath + "/Assets")
                .Select(enumerateDirectory => new TextBlock {Text = Path.GetFileName(enumerateDirectory)});
        }

        #endregion
        
        #region Workers

        private void FtpGeneratorRefresher()
        {
            var last = ConfigLoader.ConfigLoader.Settings.Ftp.Servers;
            while (!_isClosed)
            {
                Thread.Sleep(2000);
                if (!AreFtpDictionaryEqual(ConfigLoader.ConfigLoader.Settings.Ftp.Servers,last))
                    Dispatcher.UIThread.Post(RefreshFtpGenerator);
                last = FtpSettingsDeepCopy(ConfigLoader.ConfigLoader.Settings.Ftp.Servers);
            }
        }

        private void LinksGeneratorRefresher()
        {
            var last = ConfigLoader.ConfigLoader.Settings.Links;
            while (!_isClosed)
            {
                Thread.Sleep(2000);
                if (!AreLinksDictionaryEqual(last,ConfigLoader.ConfigLoader.Settings.Links))
                    Dispatcher.UIThread.Post(RefreshLinksGenerator);
                last = LinksDeepCopy(ConfigLoader.ConfigLoader.Settings.Links);
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

        #region Dictionnary

        private static bool AreLinksDictionaryEqual(Dictionary<string, string> dictionary1,
            Dictionary<string, string> dictionary2)
        {
            if (dictionary1.Count != dictionary2.Count) return false;
            foreach (var (key, value) in dictionary1)
            {
                if (!dictionary2.ContainsKey(key) || dictionary2[key] != value) return false;
            }
            return true;
        }

        private static bool AreFtpDictionaryEqual(List<OneFtpSettings> dictionary1, List<OneFtpSettings> dictionary2)
        {
            if (dictionary1.Count != dictionary2.Count) return false;
            return !dictionary1
                .Where((t, i) => 
                t.Host != dictionary2[i].Host || 
                t.Login != dictionary2[i].Login || 
                t.Name != dictionary2[i].Name || 
                t.Password != dictionary2[i].Password || 
                t.Port != dictionary2[i].Port)
                .Any();
        }
        
        private static Dictionary<string, string> LinksDeepCopy(Dictionary<string, string> dictionary) => dictionary.Keys.ToDictionary(key => key, key => dictionary[key]);

        private static List<OneFtpSettings> FtpSettingsDeepCopy(List<OneFtpSettings> servers)
            => servers.Select(oneFtpSetting => new OneFtpSettings(oneFtpSetting.Name, oneFtpSetting.Host, oneFtpSetting.Login, oneFtpSetting.Password, oneFtpSetting.Port)).ToList();

        #endregion

        private void OnUninstalledClicked(object? sender, RoutedEventArgs e)
        {
            if (File.Exists(ConfigLoader.ConfigLoader.AppPath + "/unins000.exe")) Process.Start(ConfigLoader.ConfigLoader.AppPath + "/unins000.exe");
            Environment.Exit(0);
        }
    }
}
