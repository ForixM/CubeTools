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
        public ClientUI Main;
        public TextBlock Description;
        public Image Image;
        
        public GoogleDriveHandler()
        {
            Main = ClientUI.LastReference;
            InitializeComponent();
            Description = this.FindControl<TextBlock>("Description");
            Image = this.FindControl<Image>("Image");
            Description.Text = "GoogleDrive";
            Image.Source = ResourcesLoader.ResourcesIconsCompressed.GoogleDriveCompressed;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OpenClient(object? sender, RoutedEventArgs e)
        {
            ClientGoogleDrive clientRemote = new ClientGoogleDrive();
            ClientLocal clientLocal = new ClientLocal(Directory.GetCurrentDirectory().Replace('\\', '/'));
            var mainWindow = new MainWindowRemote(clientLocal, clientRemote);
            mainWindow.RemoteView.ActionView.SetActionButtons(new List<ActionButton>
            {
                new CreateFileButton(), new CreateFolderButton(), new CopyButton(), new CutButton(), new PasteButton(),
                new RenameButton(), new DeleteButton(), new DownloadButton()
            });
            mainWindow.Show();
        }

    }
}
