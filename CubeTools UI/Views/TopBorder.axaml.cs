using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CubeTools_UI.Views
{
    public partial class TopBorder : UserControl
    {
        public TopBorder()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
