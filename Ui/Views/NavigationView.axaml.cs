using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Themes.Fluent;
using Avalonia.Threading;
using Library;
using Library.ManagerExceptions;
using Ui.Views.Error;
using Ui.Views.Settings;
using Pointer = Library.Pointer;

namespace Ui.Views
{
    public class NavigationView : UserControl
    {
        #region Model Variables
        
        // Queue Pointers : Pointers registered in a queue
        private List<Pointer> _queue;
        // Index Queue : the current index of the queue
        private int _index;
        
        #endregion

        #region Reference Variables
        
        public ClientUI Main;
        public TextBox CurrentPathXaml;
        private Image _themeIcon;
        
        #endregion
        
        #region Init
        public NavigationView()
        {
            // Main = ClientUI.LastReference;
            InitializeComponent();
            // UI
            CurrentPathXaml = this.FindControl<TextBox>("CurrentPath");
            CurrentPathXaml.Text = "";
            _themeIcon = this.FindControl<Image>("ThemeIcon");
            // Variables
            _index = -1;
            _queue = new List<Pointer>();
            // Workers
            new Thread(RefreshDarkLightTheme).Start();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        #endregion
        
        #region Events
        
        private void EditCurrentPath(object? sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            if (Main.Client.Type == ClientType.ONEDRIVE)
            {
                Main.AccessPath("/drive/root:"+((TextBox) sender!).Text);
            }
            else
            {
                Main.AccessPath(((TextBox) sender!).Text);
            }
        }

        /// <summary>
        /// The last pointer is chosen
        /// </summary>
        private void LeftArrowClick(object? sender, RoutedEventArgs e)
        {
            if (_index > 0)
            {
                _index--;
                Main.AccessPath(_queue[_index]);
            }
        }

        /// <summary>
        /// The next pointer in the stack is chosen
        /// </summary>
        private void RightArrowClick(object? sender, RoutedEventArgs e)
        {
            if (_index < _queue.Count - 1)
            {
                _index++;
                Main.AccessPath(_queue[_index]);
            }
        }

        /// <summary>
        /// The parent is being selected
        /// </summary>
        private void UpArrowClick(object? sender, RoutedEventArgs e)
        {
            if (Main.Client.CurrentFolder is not null && Main.Client.Root.Path != Main.Client.CurrentFolder.Path)
            {
                try
                {
                    Main.AccessPath(Main.Client.GetParentReference(Main.Client.CurrentFolder));
                }
                catch (ManagerException exception)
                {
                    exception.Errorstd = "Unable to access this path";
                    new ErrorBase(exception).Show();
                }

                if (Main.Client.CurrentFolder is { } folder)
                    Main.NavigationView.Add(folder);
            }
        }

        /// <summary>
        /// The sync is being pressed
        /// </summary>
        public void SyncClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                Main.Refresh();
            }
            catch (Exception exception)
            {
                if (exception is ManagerException managerException)
                {
                    managerException.Errorstd = "Unable to reload file";
                    new ErrorBase(managerException).ShowDialog<object>(Main.Main);
                }
            }
        }

        private void SettingsClick(object? sender, RoutedEventArgs e) => new SettingsWindow(Main.Main).Show();
        
        #endregion
        
        #region Process

        /// <summary>
        ///     Access the path by changing the loaded pointer
        /// </summary>
        /// <param name="pointer">the given path to access</param>
        public void AccessPath(Pointer pointer) => CurrentPathXaml.Text = Main.Client.CurrentFolder!.Path.Replace("/drive/root:", "");

        /// <summary>
        ///     Reload the pointer
        /// </summary>
        public void Refresh() => CurrentPathXaml.Text = Main.Client.CurrentFolder!.Path.Replace("/drive/root:", "");
        
        /// <summary>
        /// Add a folder in the queue
        /// </summary>
        /// <param name="folder">the folder to add</param>
        public void Add(Pointer folder)
        {
            if (_queue.Count - 1 == _index || _index < 0) _queue.Add(folder);
            else if (_queue.Count > _index + 1 && folder != _queue[_index + 1])
            {
                _queue.RemoveRange(_index + 1, _queue.Count - _index - 1);
                _queue.Add(folder);
            }

            _index++;
        }

        private void DarkLightClicked(object? sender, RoutedEventArgs e)
        {
            ConfigLoader.ConfigLoader.Settings.Styles.IsLight = !ConfigLoader.ConfigLoader.Settings.Styles.IsLight;
            ConfigLoader.ConfigLoader.SaveConfiguration();
        }

        #endregion
        
        #region Workers

        private void RefreshDarkLightTheme()
        {
            Dispatcher.UIThread.Post(() =>
            {
                ((FluentTheme) App.Current.Styles[0]).Mode = ConfigLoader.ConfigLoader.Settings.Styles.IsLight
                    ? FluentThemeMode.Light 
                    : FluentThemeMode.Dark;
                _themeIcon.Source = ConfigLoader.ConfigLoader.Settings.Styles.IsLight
                    ? ResourcesLoader.ResourcesIconsCompressed.DarkCompressed
                    : ResourcesLoader.ResourcesIconsCompressed.LightCompressed;
            });
            
            bool last = ConfigLoader.ConfigLoader.Settings.Styles.IsLight;
            while (Main is null) Thread.Sleep(500);
            while (Main.Main is MainWindow {IsClosed: false})
            {
                Thread.Sleep(250);
                if (last != ConfigLoader.ConfigLoader.Settings.Styles.IsLight)
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        ((FluentTheme) App.Current.Styles[0]).Mode = ConfigLoader.ConfigLoader.Settings.Styles.IsLight
                            ? FluentThemeMode.Light 
                            : FluentThemeMode.Dark;
                        
                        _themeIcon.Source = ConfigLoader.ConfigLoader.Settings.Styles.IsLight
                            ? ResourcesLoader.ResourcesIconsCompressed.DarkCompressed
                            : ResourcesLoader.ResourcesIconsCompressed.LightCompressed;
                    });
                }

                last = ConfigLoader.ConfigLoader.Settings.Styles.IsLight;
            }
            while (Main.Main is MainWindowRemote {IsClosed: false})
            {
                Thread.Sleep(250);
                if (last != ConfigLoader.ConfigLoader.Settings.Styles.IsLight)
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        ((FluentTheme) App.Current.Styles[0]).Mode = ConfigLoader.ConfigLoader.Settings.Styles.IsLight
                            ? FluentThemeMode.Light 
                            : FluentThemeMode.Dark;
                        _themeIcon.Source = ConfigLoader.ConfigLoader.Settings.Styles.IsLight
                            ? ResourcesLoader.ResourcesIconsCompressed.DarkCompressed
                            : ResourcesLoader.ResourcesIconsCompressed.LightCompressed;
                    });
                }
                last = ConfigLoader.ConfigLoader.Settings.Styles.IsLight;
            }
        }
        
        #endregion
    }
}
