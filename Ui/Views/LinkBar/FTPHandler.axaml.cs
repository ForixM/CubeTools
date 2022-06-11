using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Ui.Views.Ftp;

namespace Ui.Views.LinkBar
{
    public class FTPHandler : UserControl
    {
        public TextBlock Description;
        public Image Image;
        private ClientUI _main;
        
        public FTPHandler()
        {
            InitializeComponent();
            Description = this.FindControl<TextBlock>("Description");
            Image = this.FindControl<Image>("Image");
            Description.Text = "FTP";
            Image.Source = ResourcesLoader.ResourcesIconsCompressed.FtpCompressed;
        }

        public FTPHandler(ClientUI main) : this()
        {
            _main = main;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OpenClient(object? sender, RoutedEventArgs e) => new LoginFTP(_main).Show();

    }
}
