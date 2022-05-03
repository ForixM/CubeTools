using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CubeTools_UI.Views.Settings
{
    public class SettingsWindow : Window
    {
        //public static ActionBarModel Model;
        
        public SettingsWindow()
        {
            InitializeComponent();
            //Model = new ActionBarModel(this);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
