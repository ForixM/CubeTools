using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Library.ManagerExceptions;
using Library;
using Ui.Views.ActionButtons;
using Ui.Views.Error;

namespace Ui.Views.LinkBar
{
    public class OneDriveHandler : UserControl
    {
        public TextBlock Description;
        public Image Image;

        private ClientUI _main;

        public OneDriveHandler()
        {
            InitializeComponent();
            Description = this.FindControl<TextBlock>("Description");
            Image = this.FindControl<Image>("Image");
            Description.Text = "OneDrive";
            Image.Source = ResourcesLoader.ResourcesIconsCompressed.OneDriveCompressed;
        }
        
        public OneDriveHandler(ClientUI main) : this()
        {
            _main = main;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OpenClient(object? sender, RoutedEventArgs e)
        {
            ClientOneDrive clientRemote = new ClientOneDrive();
            ClientLocal clientLocal = new ClientLocal(Directory.GetCurrentDirectory().Replace('\\','/'));
            clientRemote.Client.authenticated += (o, success) =>
            {
                if (success)
                {
                    clientRemote.Children = clientRemote.ListChildren();
                    Dispatcher.UIThread.Post(() =>
                    {
                        var mainWindow = new MainWindowRemote(clientLocal, clientRemote);
                        mainWindow.RemoteView.ActionView.SetActionButtons(new List<ActionButton>
                        {
                            new CreateFileButton(_main, 0), new CreateFolderButton(_main, 1), new CopyButton(_main, 2),
                            new CutButton(_main, 3), new PasteButton(_main, 4), new RenameButton(_main, 5),
                            new DeleteButton(_main, 6), new DownloadButton(_main, 7)
                        });
                        mainWindow.Show();
                    });
                }
                else new ErrorBase(new ConnectionRefused("OneDrive connection could not be established", "Connection to OneDrive")).Show();
            };
        }
        
    }
}
