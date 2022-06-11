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
                new CreateFileButton(mainWindow.RemoteView, 0), new CreateFolderButton(mainWindow.RemoteView, 1), new CopyButton(mainWindow.RemoteView, 2),
                new CutButton(mainWindow.RemoteView, 3), new PasteButton(mainWindow.RemoteView, 4), new RenameButton(mainWindow.RemoteView, 5),
                new DeleteButton(mainWindow.RemoteView, 6), new DownloadButton(mainWindow.RemoteView, 7)
            });
            mainWindow.Show();
        }

    }
}
