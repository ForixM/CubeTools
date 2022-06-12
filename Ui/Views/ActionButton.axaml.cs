using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ResourcesLoader;
using Pointer = Library.Pointer;

namespace Ui.Views
{
    public class ActionButton : UserControl
    {
        protected ClientUI _main;

        #region Variables

        public delegate void OnClickEventHandler(object sender);

        public event OnClickEventHandler OnClickEvent;
        
        public Pointer Pointer;
        
        protected Image _icon;
        private TextBlock _name;
        private TextBlock _size;
        public Button button;

        #endregion

        #region Init
        
        public ActionButton()
        {
            InitializeComponent();
            // _main = ClientUI.LastReference;
            _icon = this.FindControl<Image>("Icon");
            _name = this.FindControl<TextBlock>("Name");
            _size = this.FindControl<TextBlock>("Size");
            button = this.FindControl<Button>("Button");
        }

        protected ActionButton(ClientUI main, int def)
        {
            InitializeComponent();
            Grid.SetColumn(this, def);
            _main = main;
            // _main = ClientUI.LastReference;
            _icon = this.FindControl<Image>("Icon");
            _name = this.FindControl<TextBlock>("Name");
            _size = this.FindControl<TextBlock>("Size");
            button = this.FindControl<Button>("Button");
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion
        
        #region Events

        /// <summary>
        /// On click event
        /// </summary>
        protected void OnClick(object? sender, RoutedEventArgs e)
        {
            OnClickEvent?.Invoke(this);
        }

        #endregion
    }
}
