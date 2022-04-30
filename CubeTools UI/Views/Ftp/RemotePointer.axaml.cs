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

        public Bitmap TypeToIcon(string type)
        {
            if (Pointer.IsDir)
                return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsIconsCompressed/Folder.ico");
            switch (type)
            {
                case "jpg":
                case "jpeg":
                case "png" :
                case "ico":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/jpg.ico");
                case "txt":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsIconsCompressed/File.ico");
                case "exe":
                case "iso":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/exe.ico");
                case "docx":
                case "odt":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/docx.ico");
                case "pdf":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/pdf.ico");
                case "py":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/py.ico");
                case "cs":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/csharp.ico");
                case "c++":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/c++.ico");
                case "java":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/java.ico");
                case "html":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/html.ico");
                case "pptx":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/pptx.ico");
                case "xlsx":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/xsls.ico");
                case "gitignore":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/gitignore.ico");
                case "zip":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/zip.ico");
                case "rar":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/rar.ico");
                case "key":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/key.ico");
                case "mp3":
                case "alac":
                case "flac":    
                case "wav":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/mp3.ico");
                case "mp4":
                case "mov":
                case "mpeg":
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/mp4.ico");
                default:
                    return new Bitmap(MainWindowModel.CubeToolsPath + "/../../../Assets/CubeToolsAppsExtensionsCompressed/None.ico");
            }
        }

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
            _icon.Source = TypeToIcon(pointer.Type);
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
            RemoteModel.ParentModel.View.AccessPathRemote(Pointer.Path, Pointer.IsDir);
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
            if (File.Exists(Pointer.Path) || Directory.Exists(Pointer.Path))
            {
                RemoteModel.Selected.Clear();
                RemoteModel.Selected.Add(this);
                foreach (var control in RemoteModel.ParentModel.View.Remote.Generator.Children)
                    ((RemotePointer) control).button.Background = new SolidColorBrush(new Color(255, 255, 255, 255));
                foreach (var control in RemoteModel.Selected)
                    control.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
            }
            else
            {
                var pathNotFoundPopUp = new ErrorPopUp.ErrorPopUp();
                pathNotFoundPopUp.Show();
                RemoteModel.ParentModel.View.ReloadPathRemote();
            }
        }

        #endregion
        
    }
}
