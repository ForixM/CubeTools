using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
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
            // Generator.HorizontalAlignment = HorizontalAlignment.Stretch;
            // Generator.MaxWidth = Main.Main.Width - 250;
            Thread thread = new Thread(HandleSize);
            thread.Start();
        }

        private void HandleSize()
        {
            Dispatcher.UIThread.Post(() => Generator.MaxWidth = Main.Main.Width, DispatcherPriority.Render);
            Thread.Sleep(100);
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

            var temp = Main.Main.Width;
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
