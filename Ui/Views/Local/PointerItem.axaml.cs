using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Library.DirectoryPointer.DirectoryPointerLoaded;
using Library.ManagerExceptions;
using ResourcesLoader;
using Pointer = Library.Pointer;

namespace Ui.Views.Local
{
    public class PointerItem : UserControl
    {
        private Local _main;
        
        #region Variables
        
        public Pointer Pointer;
        
        private Image _icon;
        private TextBlock _name;
        private TextBlock _size;
        public Button button;

        #endregion

        #region Init
        
        public PointerItem()
        {
            InitializeComponent();
            _icon = this.FindControl<Image>("Icon");
            _name = this.FindControl<TextBlock>("Name");
            _size = this.FindControl<TextBlock>("Size");
            button = this.FindControl<Button>("Button");
        }

        public PointerItem(Pointer pointer, Local main) : this()
        {
            Pointer = pointer;
            _main = main;
            _name.Text = pointer.Name;
            _size.Text = pointer.SizeXaml;
            _icon.Source = ResourcesConverter.TypeToIcon(pointer.Type, pointer.IsDir);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion
        
        #region Events
        
        /// <summary>
        /// On pointer double taped
        /// </summary>
        private void OnDoubleTaped(object? sender, RoutedEventArgs e)
        {
            _main.ActionBarView.SelectedXaml.Clear();
            if (Directory.Exists(Pointer.Path)) _main.NavigationBarView.Add(Pointer.Path);
            _main.AccessPath(Pointer.Path);
        }

        /// <summary>
        /// On pointer taped
        /// </summary>
        private void OnTaped(object? sender, PointerPressedEventArgs e)
        {
            if ((File.Exists(Pointer.Path) || Directory.Exists(Pointer.Path)) && e.MouseButton is MouseButton.Right)
            {
                var propertiesPopUp = new Properties.Properties(Pointer);
                propertiesPopUp.Show();
            }
        }

        /// <summary>
        /// On click event
        /// </summary>
        private void OnClick(object? sender, RoutedEventArgs e)
        {
            if (File.Exists(Pointer.Path) || Directory.Exists(Pointer.Path))
            {
                if (!_main.Main.KeysPressed.Contains(Key.LeftCtrl) && !_main.Main.KeysPressed.Contains(Key.RightCtrl))
                    _main.ActionBarView.SelectedXaml.Clear();
                _main.ActionBarView.SelectedXaml.Add(this);
                foreach (var control in _main.PathsBarView.Generator.Children)
                    ((PointerItem) control).button.Background = new SolidColorBrush(new Color(255, 255, 255, 255));
                foreach (var control in _main.ActionBarView.SelectedXaml)
                    control.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
            }
            else
            {
                new ErrorPopUp.PathNotFoundPopUp(new PathNotFoundException(Pointer.Path + " does not exist")).Show();
                _main.Refresh();
            }
        }

        #endregion
        
    }
}
