using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CubeTools_UI.Views
{
    public partial class GoogleDriveFileList : UserControl
    {
        public GoogleDriveFileList()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
