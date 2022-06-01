using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Library;
using Ui.Models;

namespace Ui.Views
{
    public class PathsBar : UserControl
    {
        public readonly PathsBarModel Model;
        public static PathsBarModel? LastModel;
        public StackPanel Generator;

        public PathsBar()
        {
            InitializeComponent();
            Generator = this.FindControl<StackPanel>("Generator");
            Model = new PathsBarModel(this);
            LastModel = Model;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        public void ReloadPath(List<Pointer> ftList)
        {
            Generator.Children.Clear();
            int size = ftList.Count;
            for (int i = 0; i < size; i++)
            {
                var pi = new PointerItem(ftList[i], Model.ParentModel);
                pi.button.Background = (Model.ParentModel.ModelActionBar.SelectedXaml.Contains(pi))
                    ? new SolidColorBrush(new Color(255, 255, 224, 130))
                    : new SolidColorBrush(new Color(255, 255, 255, 255));;
                Generator.Children.Add(pi);
                size = ftList.Count;
            }
        }
    }
}
