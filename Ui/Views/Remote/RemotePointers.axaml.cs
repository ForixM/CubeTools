using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using LibraryClient;

namespace Ui.Views.Remote
{
    public class RemotePointers : UserControl
    {
        public StackPanel Generator;
        public MainWindowRemote Main;

        #region Init
        
        public RemotePointers()
        {
            InitializeComponent();
            Generator = this.FindControl<StackPanel>("RemoteGenerator");
        }

        public RemotePointers(MainWindowRemote main) : this()
        {
            Main = main;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        public void ReloadPath(List<RemoteItem> ftList)
        {
            Generator.Children.Clear();
            foreach (var ft in ftList)
            {
                var pi = new RemotePointer(ft, Main);
                // TODO CHECK
                if (Main.RemoteActionView.Selected.Contains(pi.Pointer)) pi.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
                else pi.button.Background = new SolidColorBrush(new Color(255, 255, 255, 255));
                Generator.Children.Add(pi);
            }
        }
    }
}
