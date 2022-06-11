using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Library;
using Library.ManagerExceptions;
using Ui.Views.ActionButtons;
using Ui.Views.Error;

namespace Ui.Views.LinkBar
{
    public class GoogleDriveHandler : UserControl
    {
        // public ClientUI Main;
        public TextBlock Description;
        public Image Image;

        private ClientUI _main;
        
        public GoogleDriveHandler()
        {
            InitializeComponent();
            Description = this.FindControl<TextBlock>("Description");
            Image = this.FindControl<Image>("Image");
            Description.Text = "GoogleDrive";
            Image.Source = ResourcesLoader.ResourcesIconsCompressed.GoogleDriveCompressed;
        }

        public GoogleDriveHandler(ClientUI main) : this()
        {
            _main = main;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OpenClient(object? sender, RoutedEventArgs e)
        {
            ClientGoogleDrive clientRemote = new ClientGoogleDrive();
            ClientLocal clientLocal = new ClientLocal(Directory.GetCurrentDirectory().Replace('\\', '/'));
            var mainWindow = new MainWindowRemote(clientLocal, clientRemote);
            mainWindow.RemoteView.ActionView.SetActionButtons(new List<ActionButton>
            {
                new CreateFileButton(_main, 0), new CreateFolderButton(_main, 1), new CopyButton(_main, 2),
                new CutButton(_main, 3), new PasteButton(_main, 4), new RenameButton(_main, 5),
                new DeleteButton(_main, 6), new DownloadButton(_main, 7)
            });
            mainWindow.Show();
        }

    }
}
