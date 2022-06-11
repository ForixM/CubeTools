using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ConfigLoader.Settings;
using Library.ManagerExceptions;
using Library;
using Ui.Views.ActionButtons;
using Ui.Views.Error;
using Pointer = Library.Pointer;

namespace Ui.Views.Ftp
{
    public class LoginFTP : Window
    {

        private bool _isClosed;
        
        private StackPanel _ftpServersGenerator;
        private StackPanel _ftpRecentServersGenerator;
        
        public TextBox Ip;
        public TextBox User;
        public TextBox Mdp;
        public TextBox Port;

        private ClientUI _main;

        #region Init
        
        public LoginFTP()
        {
            InitializeComponent();
            Ip = this.FindControl<TextBox>("Ip");
            User = this.FindControl<TextBox>("User");
            Mdp = this.FindControl<TextBox>("Mdp");
            Port = this.FindControl<TextBox>("Port");
            // Load User's Servers
            _ftpServersGenerator = this.FindControl<StackPanel>("FtpServersGenerator");
            foreach (var server in ConfigLoader.ConfigLoader.Settings.Ftp!.Servers!)
                _ftpServersGenerator.Children.Add(new FtpServerObject(server, this, _ftpServersGenerator));
            // Load Recent Servers
            _ftpRecentServersGenerator = this.FindControl<StackPanel>("FtpRecentServersGenerator");
            foreach (var server in ConfigLoader.ConfigLoader.Settings.Ftp!.LastServers!)
                _ftpRecentServersGenerator.Children.Add(new FtpServerObject(server, this, _ftpRecentServersGenerator));
            
            // Launching Workers
            new Thread(FtpServersRefresher).Start();
            new Thread(FtpRecentServersRefresher).Start();
        }

        public LoginFTP(ClientUI main) : this()
        {
            _main = main;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events

        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }
        
        private void OnClosing(object? sender, CancelEventArgs e) => _isClosed = true;

        private void OnCancelClick(object? sender, RoutedEventArgs e) => Close();

        private void OnConnexionClicked(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Ip.Text) || string.IsNullOrEmpty(User.Text) || string.IsNullOrEmpty(Mdp.Text) ||
                string.IsNullOrEmpty(Port.Text))
            {
                if (string.IsNullOrEmpty(Ip.Text)) Ip.Watermark = "Missing information !";
                else Ip.Watermark = "Enter IP";

                if (string.IsNullOrEmpty(User.Text)) User.Watermark = "Missing information !";
                else User.Watermark = "Enter Username";

                if (string.IsNullOrEmpty(Mdp.Text)) Mdp.Watermark = "Missing information !";
                else Mdp.Watermark = "Enter Password";

                if (string.IsNullOrEmpty(Port.Text)) Port.Watermark = "Missing Information !";
                else Port.Watermark = "Enter Port";
            }
            else
            {
                // Initialize the connexion and the window
                try
                {
                    var mainWindow = new MainWindowRemote(new ClientLocal(), new ClientTransferProtocol(Ip.Text + ":" + Port.Text, User.Text, Mdp.Text));
                    mainWindow.RemoteView.ActionView.SetActionButtons(new List<ActionButton>
                    {
                        new CreateFileButton(mainWindow.RemoteView, 0),
                        new CreateFolderButton(mainWindow.RemoteView, 1), new RenameButton(mainWindow.RemoteView, 2),
                        new DeleteButton(mainWindow.RemoteView, 3), new DownloadButton(mainWindow.RemoteView, 4)
                    });
                    mainWindow.Show();
                    ConfigLoader.ConfigLoader.Settings.Ftp.LastServers.Add(new OneFtpSettings("New Recent", Ip.Text, User.Text, Mdp.Text, Port.Text));
                    ConfigLoader.ConfigLoader.SaveConfiguration();
                    Close();
                }
                catch (ManagerException exception)
                {
                    new ErrorBase(new ConnectionRefused("Invalid Credentials", "LoginFTP")).Show();
                }
            }
        }
        
        #endregion

        #region Config
        
        private void OnConfigAdded(object? sender, RoutedEventArgs e)
        {
            ConfigLoader.ConfigLoader.Settings.Ftp!.Servers!.Add(new OneFtpSettings("New Config", Ip.Text, User.Text,
                Mdp.Text, Port.Text));
            _ftpServersGenerator.Children.Add(new FtpServerObject(ConfigLoader.ConfigLoader.Settings.Ftp.Servers.Last(), this, _ftpServersGenerator));
            ConfigLoader.ConfigLoader.SaveConfiguration();
        }

        #endregion

        #region Workers

        private void FtpServersRefresher()
        {
            int last = ConfigLoader.ConfigLoader.Settings.Ftp!.Servers!.Count;
            while (!_isClosed)
            {
                Thread.Sleep(1500);
                if (ConfigLoader.ConfigLoader.Settings.Ftp.Servers.Count != last)
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        _ftpServersGenerator.Children.Clear();
                        foreach (var server in ConfigLoader.ConfigLoader.Settings.Ftp!.Servers!)
                            _ftpServersGenerator.Children.Add(new FtpServerObject(server, this,
                                _ftpServersGenerator));
                    });
                }
                last = ConfigLoader.ConfigLoader.Settings.Ftp.Servers.Count;
            }
        }

        private void FtpRecentServersRefresher()
        {
            int last = ConfigLoader.ConfigLoader.Settings.Ftp!.LastServers!.Count;
            while (!_isClosed)
            {
                Thread.Sleep(1500);
                if (ConfigLoader.ConfigLoader.Settings.Ftp.LastServers.Count != last)
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        _ftpRecentServersGenerator.Children.Clear();
                        foreach (var server in ConfigLoader.ConfigLoader.Settings.Ftp!.Servers!)
                            _ftpRecentServersGenerator.Children.Add(new FtpServerObject(server, this,
                                _ftpRecentServersGenerator));
                    }, DispatcherPriority.Background);
                }
                last = ConfigLoader.ConfigLoader.Settings.Ftp.LastServers.Count;
            }

        }

        #endregion

        private void OnKeyReleased(object? sender, KeyEventArgs e)
        {
            if (_main.Main is MainWindow window)
            {
                window.KeysPressed.Remove(e.Key);
            }
            else if (_main.Main is MainWindowRemote remote)
            {
                remote.KeysPressed.Remove(e.Key);
            }
        }
    }
}