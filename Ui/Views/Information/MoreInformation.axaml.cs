using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library;
using Library.ManagerExceptions;
using Ui.Views.Error;

namespace Ui.Views.Information
{
    public class MoreInformation : Window
    {

        private readonly ClientUI? _main;
        private readonly PointerItem? _itemXaml;
        private Image _starIcon;
        
        #region Init
        public MoreInformation()
        {
            InitializeComponent();
            SystemDecorations = SystemDecorations.BorderOnly;
            _starIcon = this.FindControl<Image>("StarIcon");
        }
        public MoreInformation(PointerItem item, ClientUI main) : this()
        {
            Position = new PixelPoint(-(int) new MouseDevice().GetPosition(item).X, 50-(int) new MouseDevice().GetPosition(item).Y);
            _itemXaml = item;
            _main = main;
            if (_main.Client.Type is not ClientType.LOCAL) this.FindControl<Button>("Compress").IsVisible = false;
            _starIcon.Source = (ConfigLoader.ConfigLoader.Settings.Links.ContainsValue(_itemXaml.Pointer.Path))
                ? ResourcesLoader.ResourcesIconsCompressed.StarredCompressed
                : ResourcesLoader.ResourcesIconsCompressed.StarCompressed;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events

        private void Copy(object? sender, RoutedEventArgs e)
        {
            _main!.ActionView.Copy(sender, e);
            Close();
        }

        private void Cut(object? sender, RoutedEventArgs e)
        {
            _main!.ActionView.Cut(sender, e);
            Close();
        }

        private void Rename(object? sender, RoutedEventArgs e)
        {
            _main!.ActionView.Rename(sender, e);
            Close();
        }

        private void Delete(object? sender, RoutedEventArgs e)
        {
            _main!.ActionView.Delete(sender, e);
            Close();
        }

        private void OpenWith(object? sender, RoutedEventArgs e)
        {
            _main!.AccessPath(_itemXaml!.Pointer);
            Close();
        }

        private void OpenWithDefault(object? sender, RoutedEventArgs e)
        {
            _main!.AccessPath(_itemXaml!.Pointer);
            Close();
        }

        private void CopyAsPath(object? sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current!.Clipboard!.SetTextAsync(_itemXaml!.Pointer.Path);
            }
            catch (Exception) {}
        }
        
        private void OnPropertiesOpen(object? sender, RoutedEventArgs e)
        {
            try
            {
                new Properties.Properties(_itemXaml!.Pointer, _main!.Client).Show();
            }
            catch (Exception) {}
        }
        
        private void CompressPressed(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (_main!.Client.Type is ClientType.LOCAL)
                {
                    //_main.Client.Compress();
                }
            }
            catch (ManagerException exception)
            {
                new ErrorBase(exception).Show();
            }
        }

        private void AddToFavorite(object? sender, RoutedEventArgs e)
        {
            if (_itemXaml is null || ConfigLoader.ConfigLoader.Settings.Links is null) return;
            
            if (!ConfigLoader.ConfigLoader.Settings.Links.ContainsValue(_itemXaml.Pointer.Path))
            {
                if (ConfigLoader.ConfigLoader.Settings.Links.ContainsKey(_itemXaml.Pointer.Name))
                {
                    int i = 1;
                    string tmp = _itemXaml.Pointer.Name;
                    while (ConfigLoader.ConfigLoader.Settings.Links.ContainsKey(tmp))
                    {
                        tmp = tmp.Remove(tmp.Length - 1, 1) + i;
                        i++;
                    }

                    ConfigLoader.ConfigLoader.Settings.Links.Add(tmp, _itemXaml.Pointer.Path);
                }
                else ConfigLoader.ConfigLoader.Settings.Links.Add(_itemXaml.Pointer.Name, _itemXaml.Pointer.Path);
                _starIcon.Source = ResourcesLoader.ResourcesIconsCompressed.StarredCompressed;
            }
            else
            {
                foreach (var key in ConfigLoader.ConfigLoader.Settings.Links.Keys.Where(key => ConfigLoader.ConfigLoader.Settings.Links[key] == _itemXaml.Pointer.Path))
                {
                    ConfigLoader.ConfigLoader.Settings.Links.Remove(key);
                    break;
                }

                _starIcon.Source = ResourcesLoader.ResourcesIconsCompressed.StarCompressed;
            }
        }
        
        #endregion


        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }

        private void OnPointerLeave(object? sender, PointerEventArgs e) => Close();
    }
}