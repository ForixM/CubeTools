using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CubeTools_UI.Views.Ftp
{
    public class LoginFTP : Window
    {

        #region Init
        
        public LoginFTP()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion

    }
}