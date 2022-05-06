using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Views.ErrorPopUp;
using Library.ManagerExceptions;
using LibraryFTP;

namespace CubeTools_UI.Views.Ftp
{
    public class LoginFTP : Window
    {

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
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion

        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }

        private void OnCancelClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnConnexionClicked(object? sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(_ip.Text) || String.IsNullOrEmpty(_user.Text) || string.IsNullOrEmpty(_mdp.Text) ||
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
                MainWindowFTP windowFtp = new MainWindowFTP(new ClientFtp(_ip.Text+":"+_port.Text, _user.Text, _mdp.Text));
                // windowFtp.Show();
                if (windowFtp.IsVisible)
                    Close();
                else
                {
                    new NormalErrorPopUp(new ManagerException("Invalid Credentials", "Low-Critical", "CubeTools crashed", "Username or Password are incorrect")).Show();
                }
            }
        }
    }
}