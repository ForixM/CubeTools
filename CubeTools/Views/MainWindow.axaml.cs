using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Manager;


namespace CubeTools.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Manager.DirectoryType dir = new DirectoryType("C:/");
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
