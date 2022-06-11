using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ConfigLoader.Settings;
using Library.ManagerExceptions;
using Library;
using Ui.Views.ActionButtons;
using Ui.Views.Error;

namespace Ui.Views.Ftp
{
    public class LoginFTP : Window
    {

        private FtpConfigDisplayer _configDisplayer;
        private TextBox _ip;
        private TextBox _user;
        private TextBox _mdp;
        private TextBox _port;

        #region Init
        
        public LoginFTP()
        {
            InitializeComponent();
            _ip = this.FindControl<TextBox>("Ip");
            _user = this.FindControl<TextBox>("User");
            _mdp = this.FindControl<TextBox>("Mdp");
            _port = this.FindControl<TextBox>("Port");
            _configDisplayer = this.FindControl<FtpConfigDisplayer>("FtpConfigDisplayer");

            foreach (var setting in ConfigLoader.ConfigLoader.Settings.Ftp!.Servers!)
                _configDisplayer.Generator.Children.Add(new FtpServerObject(setting, _configDisplayer));
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events

        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }

        private void OnCancelClick(object? sender, RoutedEventArgs e) => Close();

        private void OnConnexionClicked(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_ip.Text) || string.IsNullOrEmpty(_user.Text) || string.IsNullOrEmpty(_mdp.Text) ||
                string.IsNullOrEmpty(_port.Text))
            {
                if (string.IsNullOrEmpty(_ip.Text)) _ip.Watermark = "Missing information !";
                else _ip.Watermark = "Enter IP";

                if (string.IsNullOrEmpty(_user.Text)) _user.Watermark = "Missing information !";
                else _user.Watermark = "Enter Username";

                if (string.IsNullOrEmpty(_mdp.Text)) _mdp.Watermark = "Missing information !";
                else _mdp.Watermark = "Enter Password";

                if (string.IsNullOrEmpty(_port.Text)) _port.Watermark = "Missing Information !";
                else _port.Watermark = "Enter Port";
            }
            else
            {
                // Initialize the connexion and the window
                try
                {
                    var mainWindow = new MainWindowRemote(new ClientLocal(), new ClientTransferProtocol(_ip.Text + ":" + _port.Text, _user.Text, _mdp.Text));
                    mainWindow.RemoteView.ActionView.SetActionButtons(new List<ActionButton>
                    {
                        new CreateFileButton(0), new CreateFolderButton(1), new RenameButton(2), new DeleteButton(3),
                        new DownloadButton(4)
                    });
                    mainWindow.Show();
                    Close();
                }
                catch (Exception exception)
                {
                    new ErrorBase(new ConnectionRefused("Invalid Credentials", "LoginFTP")).Show();
                }
            }
        }
        
        #endregion

        #region Config

        private void LoadOneConfiguration(OneFtpSettings oneSetting)
        {
            _ip.Text = oneSetting.Host;
            _user.Text = oneSetting.Name;
            _port.Text = oneSetting.Port;
            _mdp.Text = oneSetting.Password;
        }

        #endregion

        private void OnConfigAdded(object? sender, RoutedEventArgs e)
        {
            ConfigLoader.ConfigLoader.Settings.Ftp!.Servers!.Add(new OneFtpSettings()
            {
                Host = _ip.Text,
                Login = _user.Text,
                Password = _mdp.Text,
                Port = _port.Text
            });
            _configDisplayer.Generator.Children.Add(new FtpServerObject(ConfigLoader.ConfigLoader.Settings.Ftp.Servers.Last(), _configDisplayer));
        }
    }
}