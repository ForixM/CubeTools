using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ConfigLoader.Settings;

namespace Ui.Views.Ftp
{
    public class FtpServerObject : UserControl
    {

        private OneFtpSettings? _server;
        private LoginFTP _loginFtp;
        private StackPanel _container;
        
        private TextBlock _name;
        private TextBlock _ip;

        public FtpServerObject()
        {
            InitializeComponent();
            _name = this.FindControl<TextBlock>("Name");
            _ip = this.FindControl<TextBlock>("Ip");
        }
        public FtpServerObject(OneFtpSettings server, LoginFTP loginFtp, StackPanel container) : this()
        {
            _server = server;
            _loginFtp = loginFtp;
            _name.Text = server.Name + " : ";
            _ip.Text = server.Host;
            _container = container;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void RemoveClicked(object? sender, RoutedEventArgs e)
        {
            ConfigLoader.ConfigLoader.Settings.Ftp.Servers.Remove(_server);
            _container.Children.Remove(this);
            ConfigLoader.ConfigLoader.SaveConfiguration();
        }

        private void LoadClicked(object? sender, RoutedEventArgs e)
        {
            if (_server is null) return;
            
            _loginFtp.Mdp.Text = _server.Password;
            _loginFtp.Ip.Text = _server.Host;
            _loginFtp.User.Text = _server.Login;
            _loginFtp.Port.Text = _server.Port;
        }
    }
}
