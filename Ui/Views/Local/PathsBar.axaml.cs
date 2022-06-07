using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Library;

namespace Ui.Views.Local
{
    public class PathsBar : UserControl
    {
        
        public Local Main;
        public StackPanel Generator;

        public PathsBar()
        {
            InitializeComponent();
            Main = Local.LastReference;
            Generator = this.FindControl<StackPanel>("Generator");
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        public void Refresh()
        {
            var list = Main.NavigationBarView.FolderLocalPointer.ChildrenFiles;
            Generator.Children.Clear();
            int size = list.Count;
            for (int i = 0; i < size; i++)
            {
                var pi = new PointerItem(list[i], Main);
                pi.button.Background = Main.ActionBarView.SelectedXaml.Contains(pi)
                    ? new SolidColorBrush(new Color(255, 255, 224, 130))
                    : new SolidColorBrush(new Color(255, 255, 255, 255));
                Generator.Children.Add(pi);
                size = list.Count;
            }
        }
        
        public void Refresh(List<LocalPointer> list)
        {
            Generator.Children.Clear();
            int size = list.Count;
            for (int i = 0; i < size; i++)
            {
                var pi = new PointerItem(list[i], Main);
                pi.button.Background = Main.ActionBarView.SelectedXaml.Contains(pi)
                    ? new SolidColorBrush(new Color(255, 255, 224, 130))
                    : new SolidColorBrush(new Color(255, 255, 255, 255));
                Generator.Children.Add(pi);
                size = list.Count;
            }
        }
    }
}
