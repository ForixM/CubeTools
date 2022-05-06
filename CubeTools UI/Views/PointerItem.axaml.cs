using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CubeTools_UI.Models;
using CubeTools_UI.Views.PopUps;
using Library.ManagerExceptions;
using Library.Pointers;
using ResourcesLoader;

namespace CubeTools_UI.Views
{
    public class PointerItem : UserControl
    {
        private LocalModel _main;
        
        #region Variables
        
        public FileType Pointer;
        
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

        public PointerItem(FileType pointer, LocalModel main) : this()
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
            _main.ModelActionBar.SelectedXaml.Clear();
            _main.AccessPath(Pointer.Path, Pointer.IsDir);
        }

        /// <summary>
        /// On pointer taped
        /// </summary>
        private void OnTaped(object? sender, PointerPressedEventArgs e)
        {
            if ((File.Exists(Pointer.Path) || Directory.Exists(Pointer.Path)) && e.MouseButton is MouseButton.Right)
            {
                var propertiesPopUp = new PropertiesPopUp(Pointer, _main);
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
                if (!_main.IsCtrlPressed)
                    _main.ModelActionBar.SelectedXaml.Clear();
                _main.ModelActionBar.SelectedXaml.Add(this);
                foreach (var control in _main.ModelPathsBar.View.Generator.Children)
                    ((PointerItem) control).button.Background = new SolidColorBrush(new Color(255, 255, 255, 255));
                foreach (var control in _main.ModelActionBar.SelectedXaml)
                    control.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
            }
            else
            {
                new ErrorPopUp.PathNotFoundPopUp(new PathNotFoundException(Pointer.Path + " does not exist")).Show();
                _main.ReloadPath();
            }
        }

        #endregion
        
    }
}
