using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ConfigLoader.Settings;

namespace Ui.Views.Ftp
{
    public class FtpConfigDisplayer : UserControl
    {
        public StackPanel Generator;
        
        public FtpConfigDisplayer()
        {
            InitializeComponent();
            Generator = this.FindControl<StackPanel>("Generator");
            if (ConfigLoader.ConfigLoader.Settings.Ftp is {Servers: { }})
            {
                foreach (var ftpServer in ConfigLoader.ConfigLoader.Settings.Ftp.Servers)
                    Generator.Children.Add(new FtpServerObject(ftpServer, this));
            }
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        private void OnConfigurationAdded(object? sender, RoutedEventArgs e)
        {
            var tmp = new OneFtpSettings
            {
                Host = "New Host",
                Name = "New Config",
                Password = "New Password",
                Port = "21"
            };
            ConfigLoader.ConfigLoader.Settings.Ftp!.Servers!.Add(tmp);
            Generator.Children.Add(new FtpServerObject(ConfigLoader.ConfigLoader.Settings.Ftp.Servers.Last(),this));
        }
    }
}
