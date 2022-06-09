using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Library.ManagerExceptions;
using Library;
using Ui.Views.Error;

namespace Ui.Views.LinkBar
{
    public class OneDriveHandler : UserControl
    {
        public ClientUI Main;
        public TextBlock Description;
        public Image Image;
        
        public OneDriveHandler()
        {
            Main = ClientUI.LastReference;
            InitializeComponent();
            Description = this.FindControl<TextBlock>("Description");
            Image = this.FindControl<Image>("Image");
            Description.Text = "OneDrive";
            Image.Source = ResourcesLoader.ResourcesIconsCompressed.OneDriveCompressed;
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
                    Dispatcher.UIThread.Post(() => new MainWindowRemote(clientLocal, clientRemote).Show());
                }
                else new ErrorBase(new ConnectionRefused("OneDrive connection could not be established", "Connection to OneDrive")).Show();
            };
        }
        
    }
}
