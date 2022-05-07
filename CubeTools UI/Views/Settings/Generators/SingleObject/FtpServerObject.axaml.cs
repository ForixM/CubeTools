using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ConfigLoader.Settings;

namespace CubeTools_UI.Views.Settings.Generators.SingleObject
{
    public class FtpServerObject : UserControl
    {

        public OneFtpSettings? Server;
        
        private TextBlock _name;
        private TextBox _ip;
        private TextBox _login;
        private TextBox _password;
        private TextBox _port;

        public FtpServerObject()
        {
            InitializeComponent();
            _name = this.FindControl<TextBlock>("Name");
            _ip = this.FindControl<TextBox>("Ip");
            _login = this.FindControl<TextBox>("Login");
            _password = this.FindControl<TextBox>("Password");
            _port = this.FindControl<TextBox>("Port");
        }
        public FtpServerObject(OneFtpSettings server) : this()
        {
            Server = server;
            
            _name.Text = server.Name;
            _ip.Text = server.Host;
            _login.Text = server.Login;
            _password.Text = server.Password;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (ConfigLoader.ConfigLoader.Settings.Styles != null && e.AddedItems.Count == 1 &&
                e.AddedItems[0] is TextBlock @block)
                ConfigLoader.ConfigLoader.Settings.Styles.Pack = @block.Text;
        }

        private void OnLoginModified(object? sender, TextInputEventArgs e) => Server.Login = ((TextBox) sender!).Text;
        private void OnPasswordModified(object? sender, TextInputEventArgs e) => Server.Password = ((TextBox) sender!).Text;
        private void OnPortModified(object? sender, TextInputEventArgs e) => Server.Port = ((TextBox) sender!).Text;
        private void OnIPModified(object? sender, TextInputEventArgs e) => Server.Host = ((TextBox) sender!).Text;
    }
}
