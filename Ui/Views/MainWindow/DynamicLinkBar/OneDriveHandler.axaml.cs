using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Library.ManagerExceptions;
using LibraryClient;
using Ui.Views.Error;
using Ui.Views.Remote;

namespace Ui.Views.MainWindow.DynamicLinkBar
{
    public class OneDriveHandler : UserControl
    {
        public Local.Local Main;
        public TextBlock Description;
        public Image Image;
        
        public OneDriveHandler()
        {
            Main = Local.Local.LastReference;
            InitializeComponent();
            Description = this.FindControl<TextBlock>("Description");
            Image = this.FindControl<Image>("Image");
            Description.Text = "OneDrive";
            Image.Source = ResourcesLoader.ResourcesIcons.OneDriveIcon;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OpenClient(object? sender, RoutedEventArgs e)
        {
            ClientOneDrive client = new ClientOneDrive(ClientType.ONEDRIVE);
            client.Client.authenticated += (o, success) =>
            {
                if (success) Dispatcher.UIThread.Post(() => new MainWindowRemote(client).Show());
                else new ErrorBase(new ConnectionRefused("OneDrive connection could not be established", "Connection to OneDrive")).Show();
            };
        }
        
    }
}
