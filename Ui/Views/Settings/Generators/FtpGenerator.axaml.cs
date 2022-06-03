using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ui.Views.Settings.Generators.SingleObject;

namespace Ui.Views.Settings.Generators
{
    public class FtpGenerator : UserControl
    {
        public StackPanel Generator;
        
        public FtpGenerator()
        {
            InitializeComponent();
            Generator = this.FindControl<StackPanel>("Generator");
            if (ConfigLoader.ConfigLoader.Settings.Ftp is {Servers: { }})
            {
                foreach (var ftpServer in ConfigLoader.ConfigLoader.Settings.Ftp.Servers)
                    Generator.Children.Add(new FtpServerObject(ftpServer));
            }
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (ConfigLoader.ConfigLoader.Settings.Ftp != null && e.AddedItems.Count == 1 &&
                e.AddedItems[0] is FtpServerObject @server)
                ConfigLoader.ConfigLoader.Settings.Ftp.Server = @server.Server;
        }
    }
}
