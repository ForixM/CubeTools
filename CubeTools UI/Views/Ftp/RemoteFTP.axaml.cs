using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CubeTools_UI.Models.Ftp;
using LibraryFTP;

namespace CubeTools_UI.Views.Ftp
{
    public class RemoteFTP : UserControl
    {
        public StackPanel Generator;
        public RemoteFTPModel FtpModel;

        #region Init
        
        public RemoteFTP()
        {
            InitializeComponent();
            Generator = this.FindControl<StackPanel>("RemoteGenerator");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion

        public void ReloadPath(List<IFtpItem> ftList)
        {
            Generator.Children.Clear();
            foreach (var ft in ftList)
            {
                var pi = new RemotePointer(ft, FtpModel);
                if (FtpModel.Selected.Contains(pi)) pi.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
                else pi.button.Background = new SolidColorBrush(new Color(255, 255, 255, 255));
                Generator.Children.Add(pi);
            }
        }
    }
}
