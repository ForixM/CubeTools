using System.ComponentModel;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library;
using Library.ManagerExceptions;
using Library.ManagerWriter;

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

        #endregion

        private bool _userActivation;

        private readonly LocalPointer _localPointer;
        
        #region Init
        public Properties()
        {
            InitializeComponent();
            _localPointer = LocalPointer.NullLocalPointer;

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
        public Properties(LocalPointer localPointer) : this()
        {
            _localPointer = localPointer;
            // TODO _pointer.LoadSize();

            _imageExtension.Source = ResourcesLoader.ResourcesConverter.TypeToIcon(localPointer.Path, localPointer.Type, localPointer.IsDir);
            _fileName.Text = localPointer.Name;
            _type.Text = localPointer.IsDir ? "folder" : localPointer.Type;
            _description.Text = localPointer.Name;
            _path.Text = localPointer.Path;
            _size.Text = localPointer.SizeXaml;
            _created.Text = localPointer.Date;
            _modified.Text = localPointer.LastDate;
            _accessed.Text = localPointer.AccessDate;
            _readOnly.IsChecked = localPointer.ReadOnly;
            _hidden.IsChecked = localPointer.Hidden;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
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
                _localPointer.SetAttributes(false, FileAttributes.ReadOnly);
                //_parentModel!.Refresh();
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
                ManagerWriter.SetAttributes(_localPointer, true, FileAttributes.ReadOnly);
                //_parentModel!.Refresh();
            }
            catch (ManagerException exception)
            {
                //_parentModel?.SelectErrorPopUp(exception);
                _userActivation = true;
                _readOnly.IsChecked = !_readOnly.IsChecked;
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
                ManagerWriter.SetAttributes(_localPointer, false, FileAttributes.Hidden);
            }
            catch (ManagerException exception)
            {
                //_parentModel?.SelectErrorPopUp(exception);
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
                ManagerWriter.SetAttributes(_localPointer, true, FileAttributes.Hidden);
                //_parentModel?.Refresh();
            }
            catch (ManagerException exception)
            {
                //_parentModel?.SelectErrorPopUp(exception);
                _hidden.IsChecked = !_hidden.IsChecked;
            }
        }

        private void OnClose(object? sender, CancelEventArgs e)
        {
            //_parentModel!.Refresh();
        }
        
        private void OnClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }
        
        #endregion


        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }
    }
}