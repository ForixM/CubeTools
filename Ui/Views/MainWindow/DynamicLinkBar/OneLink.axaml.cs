using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace Ui.Views.MainWindow.DynamicLinkBar
{
    public class OneLink : UserControl
    {
        public Local.Local Main;
        public string Path;
        public TextBlock Description;
        public Image Image;
        
        public OneLink()
        {
            Main = Local.Local.LastReference;
            Path = "";
            InitializeComponent();
            Description = this.FindControl<TextBlock>("Description");
            Image = this.FindControl<Image>("Image");
        }

        public OneLink(string link, string name, IImage image) : this()
        {
            Path = link;
            Description.Text = name;
            Image.Source = image;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OpenLink(object? sender, RoutedEventArgs e) => Main.AccessPath(Path);
        
    }
}
