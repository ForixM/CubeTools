using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Library;

namespace Ui.Views
{
    public class PointersView : UserControl
    {
        
        public ClientUI Main;
        // public StackPanel Generator;

        public Grid Generator;

        public PointersView()
        {
            InitializeComponent();
            Main = ClientUI.LastReference;
            Generator = this.FindControl<Grid>("Grid");
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        public void Refresh()
        {
            Generator.Children.Clear();
            Generator.RowDefinitions.Clear();
            int i = 0;
            foreach (var pi in Main.Client.Children.Select(pointer => new PointerItem(pointer, Main, this)))
            {
                pi.button.Background = Main.ActionView.SelectedXaml.Contains(pi)
                    ? new SolidColorBrush(new Color(255, 255, 224, 130))
                    : new SolidColorBrush(new Color(0, 255, 255, 255));
                Generator.RowDefinitions.Add(new RowDefinition(new GridLength(50)));
                Grid.SetRow(pi, i);
                Generator.Children.Add(pi);
                i++;
            }
        }
        
        public void Refresh(IEnumerable<Pointer> list)
        {
            Generator.Children.Clear();
            Generator.RowDefinitions.Clear();
            int i = 0;
            foreach (var pi in list.Select(pointer => new PointerItem(pointer, Main, this)))
            {
                pi.button.Background = Main.ActionView.SelectedXaml.Contains(pi)
                    ? new SolidColorBrush(new Color(255, 255, 224, 130))
                    : new SolidColorBrush(new Color(0, 255, 255, 255));
                Generator.RowDefinitions.Add(new RowDefinition(new GridLength(50)));
                Grid.SetRow(pi, i);
                Generator.Children.Add(pi);
                i++;
            }
        }
    }
}
