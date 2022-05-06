using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CubeTools_UI.Models;
using CubeTools_UI.Models.Ftp;
using LibraryFTP;
using ResourcesLoader;

namespace CubeTools_UI.Views.Ftp
{
    public class RemotePointer : UserControl
    {
        private RemoteFTPModel RemoteModel;
        
        #region Variables
        
        public IFtpItem Pointer;
        
        private Image _icon;
        private TextBlock _name;
        private TextBlock _size;
        public Button button;

        #endregion

        #region Init
        
        public RemotePointer()
        {
            InitializeComponent();
            _icon = this.FindControl<Image>("Icon");
            _name = this.FindControl<TextBlock>("Name");
            _size = this.FindControl<TextBlock>("Size");
            button = this.FindControl<Button>("Button");
        }

        public RemotePointer(IFtpItem pointer, RemoteFTPModel main) : this()
        {
            RemoteModel = main;
            Pointer = pointer;
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
            RemoteModel.Selected.Clear();
            if (Pointer is FtpFolder folder)
                RemoteModel.ParentModel.View.NavigationBar.Model.Add(folder);
            RemoteModel.ParentModel.View.AccessPathRemote(Pointer, Pointer.IsDir);
        }

        /// <summary>
        /// On pointer taped
        /// </summary>
        private void OnTaped(object? sender, PointerPressedEventArgs e)
        {
            if ((File.Exists(Pointer.Path) || Directory.Exists(Pointer.Path)) && e.MouseButton is MouseButton.Right)
            {
                //var propertiesPopUp = new PropertiesPopUp(Pointer, _main);
                //propertiesPopUp.Show();
            }
        }

        /// <summary>
        /// On click event
        /// </summary>
        private void OnClick(object? sender, RoutedEventArgs e)
        {
            // if (File.Exists(Pointer.Path) || Directory.Exists(Pointer.Path))
            // {
                RemoteModel.Selected.Clear();
                RemoteModel.Selected.Add(this);
                foreach (var control in RemoteModel.ParentModel.View.Remote.Generator.Children)
                    ((RemotePointer) control).button.Background = new SolidColorBrush(new Color(255, 255, 255, 255));
                foreach (var control in RemoteModel.Selected)
                    control.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
            // }
            // else
            // {
                // var pathNotFoundPopUp = new ErrorPopUp.ErrorPopUp();
                // pathNotFoundPopUp.Show();
                // RemoteModel.ParentModel.View.ReloadPathRemote();
            // }
        }

        #endregion
        
    }
}
