using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Ui.Views.Ftp;

namespace Ui.Views.LinkBar
{
    public class FTPHandler : UserControl
    {
        public OneClient Main;
        public TextBlock Description;
        public Image Image;
        
        public FTPHandler()
        {
            Main = OneClient.LastReference;
            InitializeComponent();
            Description = this.FindControl<TextBlock>("Description");
            Image = this.FindControl<Image>("Image");
            Description.Text = "FTP";
            Image.Source = ResourcesLoader.ResourcesIconsCompressed.FtpCompressed;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OpenClient(object? sender, RoutedEventArgs e) => new LoginFTP().Show();

    }
}
