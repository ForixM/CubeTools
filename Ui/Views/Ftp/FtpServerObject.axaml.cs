using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ConfigLoader.Settings;

namespace Ui.Views.Ftp
{
    public class FtpServerObject : UserControl
    {

        public OneFtpSettings? Server;
        private FtpConfigDisplayer _configDisplayer;
        
        private TextBlock _name;
        private TextBlock _ip;

        public FtpServerObject()
        {
            InitializeComponent();
            _name = this.FindControl<TextBlock>("Name");
            _ip = this.FindControl<TextBlock>("Ip");
        }
        public FtpServerObject(OneFtpSettings server, FtpConfigDisplayer configDisplayer) : this()
        {
            Server = server;
            _configDisplayer = configDisplayer;
            _name.Text = server.Name + " : ";
            _ip.Text = server.Host;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OnRemoved(object? sender, RoutedEventArgs e)
        {
            ConfigLoader.ConfigLoader.Settings.Ftp.Servers.Remove(Server);
            _configDisplayer.Generator.Children.Remove(this);
        }
    }
}
