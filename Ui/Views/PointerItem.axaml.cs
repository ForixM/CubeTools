using System;
using System.IO;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Library;
using Library.FilePointer;
using Library.ManagerExceptions;
using Library.ManagerReader;
using ResourcesLoader;
using Ui.Views.Error;
using Pointer = Library.Pointer;

namespace Ui.Views
{
    public class PointerItem : UserControl
    {
        private ClientUI _main;
        
        #region Variables
        
        public Pointer Pointer;
        
        private Image _icon;
        private TextBlock _name;
        private TextBlock _lastModified;
        private TextBlock _size;
        private Grid infoGrid;
        public Button button;

        #endregion

        #region Init
        
        public PointerItem()
        {
            InitializeComponent();
            _icon = this.FindControl<Image>("Icon");
            _name = this.FindControl<TextBlock>("Name");
            _lastModified = this.FindControl<TextBlock>("LastModified");
            _size = this.FindControl<TextBlock>("Size");
            button = this.FindControl<Button>("Button");
            infoGrid = this.FindControl<Grid>("infoGrid");
        }

        public PointerItem(Pointer pointer, ClientUI main, PointersView view) : this()
        {
            Pointer = pointer;
            _main = main;
            _name.Text = pointer.Name;
            _icon.Source = ResourcesConverter.TypeToIcon(pointer.Path, pointer.Type, pointer.IsDir);
            _lastModified.Text = pointer.LastModified;
            if (pointer is FileLocalPointer localPointer)
            {
                _size.Text = localPointer.SizeXaml;
            }
            else
            {
                _size.Text = pointer.IsDir ? "" : ManagerReader.ByteToPowByte(pointer.Size);
            }
            button.HorizontalAlignment = HorizontalAlignment.Stretch;
            _name.HorizontalAlignment = HorizontalAlignment.Left;
            button.CornerRadius = new CornerRadius(8);
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion
        
        #region Events
        
        /// <summary>
        /// On pointer double taped
        /// </summary>
        private void OnDoubleTaped(object? sender, RoutedEventArgs e)
        {
            if (_main.Client.Type is ClientType.LOCAL)
            {
                _main.ActionView.SelectedXaml.Clear();
                /*if (Directory.Exists(Pointer.Path)) */
                _main.NavigationView.Add(Pointer);
                _main.AccessPath(Pointer);
            }
            else
            {
                if (Pointer.IsDir)
                {
                    _main.ActionView.SelectedXaml.Clear();
                    /*if (Directory.Exists(Pointer.Path)) */
                    _main.NavigationView.Add(Pointer);
                    _main.AccessPath(Pointer);
                }
                else
                {
                    try
                    {
                        _main.Client.DownloadFile(_main.Client, Pointer,
                            ((MainWindowRemote) _main.Main).LocalView.Client.CurrentFolder);
                        ((MainWindowRemote) _main.Main).LocalView.AccessPath(
                            ((MainWindowRemote) _main.Main).LocalView.Client.CurrentFolder.Path + "/" + Pointer.Name);
                        ((MainWindowRemote) _main.Main).LocalView.Refresh();
                    }
                    catch (Exception)
                    {
                        new ErrorBase(new ManagerException("Download error", Level.Normal, "Download error",
                            $"Could not download this item: {Pointer.Name}")).Show();
                    }
                }
            }
        }

        /// <summary>
        /// On pointer taped
        /// </summary>
        private void OnTaped(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
                new Information.MoreInformation(this, _main).Show();
            else _main.Refresh();
        }

        /// <summary>
        /// On click event
        /// </summary>
        private void OnClick(object? sender, RoutedEventArgs e)
        {
            if (_main.Main is MainWindow main)
            {
                // Remove all not ctrl pressed
                if (!MainWindow.KeysPressed.Contains(Key.LeftCtrl) && !MainWindow.KeysPressed.Contains(Key.RightCtrl))
                    _main.ActionView.SelectedXaml.Clear();
                // Add or Remove
                if (_main.ActionView.SelectedXaml.Contains(this)) _main.ActionView.SelectedXaml.Remove(this);
                else _main.ActionView.SelectedXaml.Add(this);
                // Modify UI
                foreach (var control in _main.PointersView.Generator.Children)
                    ((PointerItem) control).button.Background = new SolidColorBrush(new Color(0, 255, 255, 255));
                foreach (var control in _main.ActionView.SelectedXaml)
                    control.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
            }
            else if (_main.Main is MainWindowRemote mainRemote)
            {
                // Remove all not ctrl pressed
                if (mainRemote.localFocused && _main.Client.Type is not ClientType.LOCAL)
                {
                    RefreshButtons(mainRemote.LocalView);
                    mainRemote.localFocused = false;
                }
                else if (!mainRemote.localFocused && _main.Client.Type is ClientType.LOCAL)
                {
                    RefreshButtons(mainRemote.RemoteView);
                    mainRemote.localFocused = true;
                }
                else if (!mainRemote.KeysPressed.Contains(Key.LeftCtrl) && !mainRemote.KeysPressed.Contains(Key.RightCtrl))
                    _main.ActionView.SelectedXaml.Clear();
                // Add or Remove
                if (_main.ActionView.SelectedXaml.Contains(this)) _main.ActionView.SelectedXaml.Remove(this);
                else _main.ActionView.SelectedXaml.Add(this);
                // Modify UI
                foreach (var control in _main.PointersView.Generator.Children)
                    ((PointerItem) control).button.Background = new SolidColorBrush(new Color(0, 255, 255, 255));
                foreach (var control in _main.ActionView.SelectedXaml)
                    control.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
            }
        }

        private void RefreshButtons(ClientUI view)
        {
            view.ActionView.SelectedXaml.Clear();
            foreach (var control in view.PointersView.Generator.Children)
                ((PointerItem) control).button.Background = new SolidColorBrush(new Color(0, 255, 255, 255));
            foreach (var control in view.ActionView.SelectedXaml)
                control.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
        }

        #endregion
    }
}
