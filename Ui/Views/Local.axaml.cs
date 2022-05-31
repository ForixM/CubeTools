using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ui.Models;

namespace Ui.Views
{
    public class Local : UserControl
    {
        public static LocalModel LastModel;
        public readonly LocalModel Model;
        public readonly MainWindowModel ParentModel;

        public Local()
        {
            InitializeComponent();
            Model = new LocalModel(this, ParentModel);
            LastModel = Model;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
    }
}