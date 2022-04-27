using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CubeTools_UI.Models;
using Library.Pointers;

namespace CubeTools_UI.Views
{
    public class PathsBar : UserControl
    {
        public static PathsBarModel Model;
        public StackPanel Generator;

        public PathsBar()
        {
            InitializeComponent();
            Generator = this.FindControl<StackPanel>("Generator");
            Model = new PathsBarModel(this);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void ReloadPath(List<FileType> ftList)
        {
            Generator.Children.Clear();
            int size = ftList.Count;
            for (int i = 0; i < size; i++)
            {
                var pi = new PointerItem(ftList[i], Model.ParentModel);
                if (Model.ParentModel.ModelActionBar.SelectedXaml.Contains(pi))
                    pi.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
                else
                    pi.button.Background = new SolidColorBrush(new Color(255, 255, 255, 255));
                Generator.Children.Add(pi);
                size = ftList.Count;
            }
        }
    }
}
