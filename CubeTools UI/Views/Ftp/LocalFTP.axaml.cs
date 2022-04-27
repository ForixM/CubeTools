using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Library.Pointers;

namespace CubeTools_UI.Views.Ftp
{
    public class LocalFTP : UserControl
    {
        public StackPanel Generator;

        public LocalFTP()
        {
            InitializeComponent();
            Generator = this.FindControl<StackPanel>("LocalGenerator");
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
                /*
                var pi = new LocalPointer(ft, Model.ParentModel);
                if (Model.ParentModel.ModelActionBar.SelectedXaml.Contains(pi))
                    pi.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
                else
                    pi.button.Background = new SolidColorBrush(new Color(255, 255, 255, 255));
                Generator.Children.Add(pi);
                */
            }
        }
    }
}
