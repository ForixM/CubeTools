using System.ComponentModel;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using Library.ManagerExceptions;
using Library.ManagerWriter;
using Library.Pointers;

namespace CubeTools_UI.Views.PopUps
{
    public class PropertiesPopUp : Window
    {
        #region Children Components

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

        private readonly FileType _pointer;
        private readonly LocalModel? _parentModel;
        
        #region Init
        public PropertiesPopUp()
        {
            InitializeComponent();
            _pointer = FileType.NullPointer;

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
        public PropertiesPopUp(FileType ft, LocalModel main) : this()
        {
            _pointer = ft;
            _pointer.LoadSize();
            
            _fileName.Text = ft.Name;
            _type.Text = ft.Type;
            _description.Text = ft.Name;
            _path.Text = ft.Path;
            _size.Text = ft.SizeXaml + $" ({SpacedLong(ft.Size)} B)";
            _created.Text = ft.Date;
            _modified.Text = ft.LastDate;
            _accessed.Text = ft.AccessDate;
            _readOnly.IsChecked = ft.ReadOnly;
            _hidden.IsChecked = ft.Hidden;
            
            _parentModel = main;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        

        private string SpacedLong(long value)
        {
            string str = "";
            int compteur = 0;
            while (value > 0)
            {
                str = value % 10+str;
                value /= 10;
                compteur++;
                if (compteur == 3)
                {
                    str = " " + str;
                    compteur = 0;
                }
            }

            return str;
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
                ManagerWriter.SetAttributes(_pointer, false, FileAttributes.ReadOnly);
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