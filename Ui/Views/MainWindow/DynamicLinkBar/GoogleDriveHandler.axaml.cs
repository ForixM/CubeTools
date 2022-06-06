using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Ui.Views.Ftp;

namespace Ui.Views.MainWindow.DynamicLinkBar
{
    public class GoogleDriveHandler : UserControl
    {
        public Local.Local Main;
        public TextBlock Description;
        public Image Image;
        
        public GoogleDriveHandler()
        {
            Main = Local.Local.LastReference;
            InitializeComponent();
            Description = this.FindControl<TextBlock>("Description");
            Image = this.FindControl<Image>("Image");
            Description.Text = "GoogleDrive";
            Image.Source = ResourcesLoader.ResourcesIcons.GoogleDriveIcon;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OpenClient(object? sender, RoutedEventArgs e)
        {
            
        }

    }
}
