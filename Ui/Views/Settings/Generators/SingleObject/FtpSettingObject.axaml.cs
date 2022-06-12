using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ConfigLoader.Settings;

namespace Ui.Views.Settings.Generators.SingleObject
{
    public class FtpSettingObject : UserControl
    {

        private OneFtpSettings _server;
        
        private TextBox _name;
        private TextBox _ip;
        private TextBox _login;
        private TextBox _password;
        private TextBox _port;

        #region Init

        public FtpSettingObject()
        {
            InitializeComponent();
            _name = this.FindControl<TextBox>("Name");
            _ip = this.FindControl<TextBox>("Ip");
            _login = this.FindControl<TextBox>("Login");
            _password = this.FindControl<TextBox>("Password");
            _port = this.FindControl<TextBox>("Port");
        }
        public FtpSettingObject(OneFtpSettings server) : this()
        {
            _server = server;
            
            _name.Text = server.Name;
            _ip.Text = server.Host;
            _login.Text = server.Login;
            _password.Text = server.Password;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        #endregion

        #region Events

        private void OnLoginModified(object? sender, KeyEventArgs e) => _server.Login = ((TextBox) sender!).Text;
        private void OnPasswordModified(object? sender, KeyEventArgs e) => _server.Password = ((TextBox) sender!).Text;
        private void OnPortModified(object? sender, KeyEventArgs e) => _server.Port = ((TextBox) sender!).Text;
        private void OnIPModified(object? sender, KeyEventArgs e) => _server.Host = ((TextBox) sender!).Text;
        private void OnNameModified(object? sender, KeyEventArgs e) => _server.Name = ((TextBox) sender!).Text;

        #endregion
    }
}
