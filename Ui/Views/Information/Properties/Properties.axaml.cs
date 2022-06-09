using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Library;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using ResourcesLoader;
using Ui.Views.Error;
using Pointer = Library.Pointer;

namespace Ui.Views.Information.Properties
{
    public class Properties : Window
    {
        #region Children Components

        private Image _imageExtension;
        private TextBlock _fileName;
        private TextBlock _type;
        private TextBlock _description;
        private TextBlock _path;
        private TextBlock _size;
        private TextBlock _created;
        private TextBlock _modified;
        private TextBlock _accessed;
        private CheckBox _readOnly;
        private CheckBox _hidden;
        private Image _starIcon;
        private Button _star;

        #endregion

        private bool _userActivation;

        private readonly Pointer _pointer;
        private readonly Client _client;
        
        #region Init
        public Properties()
        {
            InitializeComponent();
            _pointer = LocalPointer.NullLocalPointer;
            _star = this.FindControl<Button>("Star");
            _starIcon = this.FindControl<Image>("StarIcon");
            _imageExtension = this.FindControl<Image>("ImageExtension");
            _fileName = this.FindControl<TextBlock>("FileName");
            _type = this.FindControl<TextBlock>("Type");
            _description = this.FindControl<TextBlock>("Description");
            _path = this.FindControl<TextBlock>("Path");
            _size = this.FindControl<TextBlock>("Size");
            _created = this.FindControl<TextBlock>("Created");
            _modified = this.FindControl<TextBlock>("Modified");
            _accessed = this.FindControl<TextBlock>("Accessed");
            _readOnly = this.FindControl<CheckBox>("Read-Only");
            _hidden = this.FindControl<CheckBox>("Hidden");

            _userActivation = false;
        }
        public Properties(Pointer pointer, Client client) : this()
        {
            _pointer = pointer;
            _client = client;
            // 
            _imageExtension.Source = ResourcesLoader.ResourcesConverter.TypeToIcon(_pointer.Path, _pointer.Type, _pointer.IsDir);
            _fileName.Text = _pointer.Name;
            _type.Text = _pointer.IsDir ? "folder" : _pointer.Type;
            _description.Text = _pointer.Name;
            _path.Text = _pointer.Path;
            // 
            switch (client.Type)
            {
                case ClientType.LOCAL :
                    InitializeLocal();
                    break;
                case ClientType.FTP :
                    InitializeFTP();
                    break;
                case ClientType.ONEDRIVE :
                    InitializeOneDrive();
                    break;
                case ClientType.GOOGLEDRIVE:
                    InitializeGoogleDrive();
                    break;
            }
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void InitializeLocal()
        {
            try
            {
                Dispatcher.UIThread.Post(
                    () =>
                    {
                        try
                        {
                            _size.Text = ManagerReader.ByteToPowByte(_client.GetItemSize(_pointer));
                        }
                        catch (ManagerException e)
                        {
                            _size.Text = "Unknown";
                            new ErrorBase(e).Show();
                        }
                    },
                    DispatcherPriority.Background);
                _created.Text = _client.GetItemCreationDate(_pointer);
                _modified.Text = _client.GetItemLastEditionDate(_pointer);
                _accessed.Text = _client.GetItemLastEditionDate(_pointer);
                _readOnly.IsChecked = _client.GetItemReadOnlyProperty(_pointer);
                _hidden.IsChecked = _client.GetItemHiddenProperty(_pointer);
                _star.IsEnabled = true;
                _starIcon.Source = ConfigLoader.ConfigLoader.Settings.Links.ContainsValue(_pointer.Path) ? ResourcesIcons.StarredIcon : ResourcesIcons.StarIcon;
            }
            catch (ManagerException e)
            {
                new ErrorBase(e).Show();
            }
        }
        private void InitializeFTP()
        {
            try
            {
                Dispatcher.UIThread.Post(() =>
                {
                    try
                    {
                        _size.Text = ManagerReader.ByteToPowByte(_client.GetItemSize(_pointer));
                    }
                    catch(ManagerException e) { new ErrorBase(e).Show();}
                });
                _modified.Text = _client.GetItemLastEditionDate(_pointer);
                _readOnly.IsEnabled = false;
                _hidden.IsEnabled = false;
                _star.IsEnabled = false;
                _starIcon.Source = ResourcesIcons.StarIcon;
            }
            catch (Exception e) {}
        }
        // TODO Mehdi
        private void InitializeOneDrive()
        {
            try
            {
                _size.Text = ManagerReader.ByteToPowByte(_client.GetItemSize(_pointer));
                /*
                _created.Text = _client.GetItemCreationDate(_pointer);
                _modified.Text = _client.GetItemLastEditionDate(_pointer);
                _accessed.Text = _client.GetItemLastEditionDate(_pointer);
                _readOnly.IsChecked = _client.GetItemReadOnlyProperty(_pointer);
                _hidden.IsChecked = _client.GetItemHiddenProperty(_pointer);
                 */
            }
            catch (Exception e) {}
            _star.IsEnabled = false;
            _starIcon.Source = ResourcesIcons.StarIcon;
            _readOnly.IsEnabled = false;
            _hidden.IsEnabled = false;
        }
        // TODO Max
        private void InitializeGoogleDrive()
        {
            try
            {
                _size.Text = ManagerReader.ByteToPowByte(_client.GetItemSize(_pointer));
                /*
                _created.Text = _client.GetItemCreationDate(_pointer);
                _modified.Text = _client.GetItemLastEditionDate(_pointer);
                _accessed.Text = _client.GetItemLastEditionDate(_pointer);
                _readOnly.IsChecked = _client.GetItemReadOnlyProperty(_pointer);
                _hidden.IsChecked = _client.GetItemHiddenProperty(_pointer);
                 */
            }
            catch (Exception e) {}
            _starIcon.Source = ResourcesIcons.StarIcon;
            _star.IsEnabled = false;
            _readOnly.IsEnabled = false;
            _hidden.IsEnabled = false;
        }
        
        
        #endregion

        #region Events
        
        private void ReadOnlyUnchecked(object? sender, RoutedEventArgs e)
        {
            if (_userActivation)
            {
                _userActivation = false;
                return;
            }
            try
            {
                _client.SetReadOnlyProperty(_pointer, false);
            }
            catch (ManagerException exception)
            {
                //_parentModel?.SelectErrorPopUp(exception);
                _userActivation = true;
                _readOnly.IsChecked = !_readOnly.IsChecked;
            }
        }

        private void ReadOnlyChecked(object? sender, RoutedEventArgs e)
        {
            if (_userActivation)
            {
                _userActivation = false;
                return;
            }

            try
            {
                _client.SetReadOnlyProperty(_pointer, true);
            }
            catch (ManagerException exception)
            {
                _userActivation = true;
                _hidden.IsChecked = !_hidden.IsChecked;
            }
        }
        
        private void HiddenUnchecked(object? sender, RoutedEventArgs e)
        {
            if (_userActivation)
            {
                _userActivation = false;
                return;
            }
            try
            {
                _client.SetHiddenProperty(_pointer, false);
            }
            catch (ManagerException exception)
            {
                _userActivation = true;
                _hidden.IsChecked = !_hidden.IsChecked;
            }
        }
        
        private void HiddenChecked(object? sender, RoutedEventArgs e)
        {
            if (_userActivation)
            {
                _userActivation = false;
                return;
            }
            try
            {
                _client.SetHiddenProperty(_pointer, true);
            }
            catch (ManagerException exception)
            {
                _userActivation = true;
                _hidden.IsChecked = !_hidden.IsChecked;
            }
        }

        private void OnFavoriteClicked(object? sender, RoutedEventArgs e)
        {
            string? currentKey = ConfigLoader.ConfigLoader.Settings.Links?.Keys.Where(key => ConfigLoader.ConfigLoader.Settings.Links[key] == _pointer.Path).FirstOrDefault();
            if (currentKey is not null)
            {
                ConfigLoader.ConfigLoader.Settings.Links!.Remove(currentKey);
                _starIcon.Source = ResourcesLoader.ResourcesIcons.StarIcon;
            }
            else
            {
                // TODO Mateo
                string key = "Link";
                while (ConfigLoader.ConfigLoader.Settings.Links.ContainsKey(key)) key += " - New";
                ConfigLoader.ConfigLoader.Settings.Links.Add(key, _pointer.Path);
                ConfigLoader.ConfigLoader.SaveConfiguration();
                _starIcon.Source = ResourcesLoader.ResourcesIcons.StarredIcon;
            }
        }
        
        private void OnClose(object? sender, CancelEventArgs e) => _client.Refresh();

        private void OnClick(object? sender, RoutedEventArgs e) => Close();
        
        #endregion


        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }
        
    }
}