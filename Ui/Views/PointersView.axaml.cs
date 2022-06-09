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
        public StackPanel Generator;

        public PointersView()
        {
            InitializeComponent();
            Main = ClientUI.LastReference;
            Generator = this.FindControl<StackPanel>("Generator");
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        public void Refresh()
        {
            Debug.Print("this width is: ");
            Generator.Children.Clear();
            foreach (var pi in Main.Client.Children.Select(pointer => new PointerItem(pointer, Main)))
            {
                pi.button.Background = Main.ActionView.SelectedXaml.Contains(pi)
                    ? new SolidColorBrush(new Color(255, 255, 224, 130))
                    : new SolidColorBrush(new Color(0, 255, 255, 255));
                // pi.button.Width = Main.Main.Width - 250;
                Generator.Children.Add(pi);
            }
        }
        
        public void Refresh(IEnumerable<Pointer> list)
        {
            Generator.Children.Clear();
            foreach (var pi in list.Select(pointer => new PointerItem(pointer, Main)))
            {
                pi.button.Background = Main.ActionView.SelectedXaml.Contains(pi)
                    ? new SolidColorBrush(new Color(255, 255, 224, 130))
                    : new SolidColorBrush(new Color(0, 255, 255, 255));
                // pi.button.Width = Main.Main.Width-250;
                Generator.Children.Add(pi);
            }
        }
    }
}
