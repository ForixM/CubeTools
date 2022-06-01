using System.ComponentModel;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Library.ManagerWriter;
using Library;
using Ui.Models;
using Pointer = Library.Pointer;

namespace Ui.Views.PopUps
{
    public class PropertiesPopUp : Window
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

        private readonly Pointer _pointer;
        private readonly LocalModel? _parentModel;
        
        #region Init
        public PropertiesPopUp()
        {
            InitializeComponent();
            _pointer = Pointer.NullPointer;

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
            _parentModel = null;
        }
        public PropertiesPopUp(Pointer pointer, LocalModel main) : this()
        {
            _pointer = pointer;
            // TODO _pointer.LoadSize();

            _imageExtension.Source = ResourcesLoader.ResourcesConverter.TypeToIcon(pointer.Type, pointer.IsDir);
            _fileName.Text = pointer.Name;
            _type.Text = pointer.IsDir ? "folder" : pointer.Type;
            _description.Text = pointer.Name;
            _path.Text = pointer.Path;
            _size.Text = pointer.SizeXaml;
            _created.Text = pointer.Date;
            _modified.Text = pointer.LastDate;
            _accessed.Text = pointer.AccessDate;
            _readOnly.IsChecked = pointer.ReadOnly;
            _hidden.IsChecked = pointer.Hidden;
            
            _parentModel = main;
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
                _pointer.SetAttributes(false, FileAttributes.ReadOnly);
                _parentModel!.ReloadPath();
            }
            catch (ManagerException exception)
            {
                _parentModel?.SelectErrorPopUp(exception);
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
                ManagerWriter.SetAttributes(_pointer, true, FileAttributes.ReadOnly);
                _parentModel!.ReloadPath();
            }
            catch (ManagerException exception)
            {
                _parentModel?.SelectErrorPopUp(exception);
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
                ManagerWriter.SetAttributes(_pointer, false, FileAttributes.Hidden);
                _parentModel!.ReloadPath();
            }
            catch (ManagerException exception)
            {
                _parentModel?.SelectErrorPopUp(exception);
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
                ManagerWriter.SetAttributes(_pointer, true, FileAttributes.Hidden);
                _parentModel?.ReloadPath();
            }
            catch (ManagerException exception)
            {
                _parentModel?.SelectErrorPopUp(exception);
                _hidden.IsChecked = !_hidden.IsChecked;
            }
        }

        private void OnClose(object? sender, CancelEventArgs e)
        {
            _parentModel!.ReloadPath();
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