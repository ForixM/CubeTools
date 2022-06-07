using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Library;
using Library.ManagerExceptions;
using ResourcesLoader;
using Ui.Views.Error;
using Ui.Views.Remote;

namespace Ui.Views.Local
{
    public class PointerItem : UserControl
    {
        private Local _main;
        
        #region Variables
        
        public LocalPointer LocalPointer;
        
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

        public PointerItem(LocalPointer localPointer, Local main) : this()
        {
            LocalPointer = localPointer;
            _main = main;
            _name.Text = localPointer.Name;
            _size.Text = localPointer.SizeXaml;
            _icon.Source = ResourcesConverter.TypeToIcon(localPointer.Type, localPointer.IsDir);
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion
        
        #region Events
        
        /// <summary>
        /// On pointer double taped
        /// </summary>
        private void OnDoubleTaped(object? sender, RoutedEventArgs e)
        {
            _main.ActionBarView.SelectedXaml.Clear();
            if (Directory.Exists(LocalPointer.Path)) _main.NavigationBarView.Add(LocalPointer.Path);
            _main.AccessPath(LocalPointer.Path);
        }

        /// <summary>
        /// On pointer taped
        /// </summary>
        private void OnTaped(object? sender, PointerPressedEventArgs e)
        {
            if ((File.Exists(LocalPointer.Path) || Directory.Exists(LocalPointer.Path)) && e.MouseButton is MouseButton.Right)
            {
                var propertiesPopUp = new Information.MoreInformation(LocalPointer);
                propertiesPopUp.Show();
            }
        }

        /// <summary>
        /// On click event
        /// </summary>
        private void OnClick(object? sender, RoutedEventArgs e)
        {
            if (_main.IsRemote)
            {
                if (File.Exists(LocalPointer.Path) || Directory.Exists(LocalPointer.Path))
                {
                    // Remove all not ctrl pressed
                    if (_main.Main is MainWindowRemote mainReference && !mainReference.KeysPressed.Contains(Key.LeftCtrl) && !mainReference.KeysPressed.Contains(Key.RightCtrl))
                        _main.ActionBarView.SelectedXaml.Clear();
                    // Add or Remove
                    if (_main.ActionBarView.SelectedXaml.Contains(this)) _main.ActionBarView.SelectedXaml.Remove(this);
                    else _main.ActionBarView.SelectedXaml.Add(this);
                    // Modify UI
                    foreach (var control in _main.PathsBarView.Generator.Children)
                        ((PointerItem) control).button.Background = new SolidColorBrush(new Color(255, 255, 255, 255));
                    foreach (var control in _main.ActionBarView.SelectedXaml)
                        control.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
                }
                else
                {
                    new ErrorBase(new PathNotFoundException(LocalPointer.Path + " does not exist", "Pointer Local UI")).Show();
                    _main.Refresh();
                }
            }
            else
            {
                // Remove all not ctrl pressed
                if (_main.Main is MainWindow.MainWindow mainReference && !mainReference.KeysPressed.Contains(Key.LeftCtrl) && !mainReference.KeysPressed.Contains(Key.RightCtrl))
                    _main.ActionBarView.SelectedXaml.Clear();
                // Add or Remove
                if (_main.ActionBarView.SelectedXaml.Contains(this)) _main.ActionBarView.SelectedXaml.Remove(this);
                else _main.ActionBarView.SelectedXaml.Add(this);
                // Modify UI
                foreach (var control in _main.PathsBarView.Generator.Children)
                    ((PointerItem) control).button.Background = new SolidColorBrush(new Color(255, 255, 255, 255));
                foreach (var control in _main.ActionBarView.SelectedXaml)
                    control.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
            }
        }

        #endregion
    }
}
