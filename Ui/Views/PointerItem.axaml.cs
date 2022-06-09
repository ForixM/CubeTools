using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ResourcesLoader;
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
        public Button button;

        #endregion

        #region Init
        
        public PointerItem()
        {
            InitializeComponent();
            _icon = this.FindControl<Image>("Icon");
            _name = this.FindControl<TextBlock>("Name");
            button = this.FindControl<Button>("Button");
        }

        public PointerItem(Pointer pointer, ClientUI main) : this()
        {
            Pointer = pointer;
            _main = main;
            _name.Text = pointer.Name;
            _icon.Source = ResourcesConverter.TypeToIcon(pointer.Path, pointer.Type, pointer.IsDir);
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion
        
        #region Events
        
        /// <summary>
        /// On pointer double taped
        /// </summary>
        private void OnDoubleTaped(object? sender, RoutedEventArgs e)
        {
            _main.ActionView.SelectedXaml.Clear();
            if (Directory.Exists(Pointer.Path)) _main.NavigationView.Add(Pointer);
            _main.AccessPath(Pointer);
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
                if (!main.KeysPressed.Contains(Key.LeftCtrl) && !main.KeysPressed.Contains(Key.RightCtrl))
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
                if (mainRemote.LocalView.ActionView.SelectedXaml.Count > 0 && _main.Equals(mainRemote.RemoteView))
                {
                    mainRemote.LocalView.ActionView.SelectedXaml.Clear();
                    foreach (var control in mainRemote.LocalView.PointersView.Generator.Children)
                        ((PointerItem) control).button.Background = new SolidColorBrush(new Color(0, 255, 255, 255));
                    foreach (var control in mainRemote.LocalView.ActionView.SelectedXaml)
                        control.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
                }
                else if (mainRemote.RemoteView.ActionView.SelectedXaml.Count > 0 && _main.Equals(mainRemote.LocalView))
                {
                    mainRemote.RemoteView.ActionView.SelectedXaml.Clear();
                    foreach (var control in mainRemote.RemoteView.PointersView.Generator.Children)
                        ((PointerItem) control).button.Background = new SolidColorBrush(new Color(0, 255, 255, 255));
                    foreach (var control in mainRemote.RemoteView.ActionView.SelectedXaml)
                        control.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
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

        #endregion
    }
}
