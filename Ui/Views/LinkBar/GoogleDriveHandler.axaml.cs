using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Ui.Views.LinkBar
{
    public class GoogleDriveHandler : UserControl
    {
        public OneClient Main;
        public TextBlock Description;
        public Image Image;
        
        public GoogleDriveHandler()
        {
            Main = OneClient.LastReference;
            InitializeComponent();
            Description = this.FindControl<TextBlock>("Description");
            Image = this.FindControl<Image>("Image");
            Description.Text = "GoogleDrive";
            Image.Source = ResourcesLoader.ResourcesIconsCompressed.GoogleDriveCompressed;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OpenClient(object? sender, RoutedEventArgs e)
        {
            
        }

    }
}
