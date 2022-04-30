using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CubeTools_UI.Models.Ftp;
using Library.Pointers;

namespace CubeTools_UI.Views.Ftp
{
    public class LocalFTP : UserControl
    {
        public StackPanel Generator;
        public LocalFTPModel FtpModel;

        #region Init
        
        public LocalFTP()
        {
            InitializeComponent();
            Generator = this.FindControl<StackPanel>("LocalGenerator");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion

        public void ReloadPath(List<FileType> ftList)
        {
            Generator.Children.Clear();
            foreach (var ft in ftList)
            {
                var pi = new LocalPointer(ft, FtpModel);
                if (FtpModel.Selected.Contains(pi)) pi.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
                else pi.button.Background = new SolidColorBrush(new Color(255, 255, 255, 255));
                Generator.Children.Add(pi);
            }
        }
    }
}
