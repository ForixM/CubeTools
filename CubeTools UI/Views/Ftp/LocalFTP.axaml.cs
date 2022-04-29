using System.Collections.Generic;
using System.IO;
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

        public LocalFTP(MainWindowFTP main)
        {
            InitializeComponent();
            Generator = this.FindControl<StackPanel>("LocalGenerator");
            FtpModel = new LocalFTPModel(main.Model, Directory.GetCurrentDirectory());
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void ReloadPath(List<FileType> ftList)
        {
            Generator.Children.Clear();
            foreach (var ft in ftList)
            {
                var pi = new LocalPointer(ft, FtpModel);
                if (FtpModel.SelectedLocal.Contains(pi)) pi.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
                else pi.button.Background = new SolidColorBrush(new Color(255, 255, 255, 255));
                Generator.Children.Add(pi);
            }
        }
    }
}
