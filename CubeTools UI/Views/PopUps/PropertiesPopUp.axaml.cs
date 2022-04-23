using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using Library.ManagerExceptions;
using Library.ManagerReader;
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
        
        private readonly FileType _pointer;
        private MainWindowViewModel ParentViewModel;
        
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

            ParentViewModel = null;
        }
        public PropertiesPopUp(FileType ft, ObservableCollection<FileType> items, MainWindowViewModel main) : this()
        {
            _pointer = ft;
            _fileName.Text = ft.Name;
            _type.Text = ft.Type;
            _description.Text = ft.Name;
            _path.Text = ft.Path;
            _size.Text = ft.SizeXaml;
            _created.Text = ft.Date;
            _modified.Text = ft.LastDate;
            _accessed.Text = ft.AccessDate;
            _readOnly.IsChecked = ft.ReadOnly;
            _hidden.IsChecked = ft.Hidden;

            ParentViewModel = main;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        #endregion

        #region Events
        
        private void ReadOnlyUnchecked(object? sender, RoutedEventArgs e)
        {
            try
            {
                ManagerWriter.SetAttributes(_pointer, false, FileAttributes.ReadOnly);
                ParentViewModel.ReloadPath();
            }
            catch (ManagerException exception)
            {
                var popup = new ErrorPopUp.ErrorPopUp(ParentViewModel, exception);
                popup.Show();
                _readOnly.IsChecked = !_readOnly.IsChecked;
            }
        }

        private void ReadOnlyChecked(object? sender, RoutedEventArgs e)
        {
            try
            {
                ManagerWriter.SetAttributes(_pointer, true, FileAttributes.ReadOnly);
                ParentViewModel?.ReloadPath();
            }
            catch (ManagerException exception)
            {
                var popup = new ErrorPopUp.ErrorPopUp(ParentViewModel, exception);
                popup.Show();
                _readOnly.IsChecked = !_readOnly.IsChecked;
            }
        }
        
        private void HiddenUnchecked(object? sender, RoutedEventArgs e)
        {
            try
            {
                ManagerWriter.SetAttributes(_pointer, false, FileAttributes.Hidden);
                ParentViewModel.ReloadPath();
            }
            catch (ManagerException exception)
            {
                var popup = new ErrorPopUp.ErrorPopUp(ParentViewModel, exception);
                popup.Show();
                _readOnly.IsChecked = !_readOnly.IsChecked;
            }
        }
        
        private void HiddenChecked(object? sender, RoutedEventArgs e)
        {
            try
            {
                ManagerWriter.SetAttributes(_pointer, true, FileAttributes.Hidden);
                ParentViewModel.ReloadPath();
            }
            catch (ManagerException exception)
            {
                var popup = new ErrorPopUp.ErrorPopUp(ParentViewModel, exception);
                popup.Show();
                _readOnly.IsChecked = !_readOnly.IsChecked;
            }
        }

        private void OnClose(object? sender, CancelEventArgs e)
        {
            ParentViewModel.ReloadPath();
        }
        
        #endregion

    }
}