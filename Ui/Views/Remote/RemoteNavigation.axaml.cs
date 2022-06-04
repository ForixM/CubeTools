using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using LibraryClient;

namespace Ui.Views.Remote
{
    public class RemoteNavigation : UserControl
    {
        private int _index;
        private List<RemoteItem> _queue;

        public int Index
        {
            get => _index;
            set => _index = value;
        }

        public List<RemoteItem> Queue => _queue;
        
        public TextBox CurrentPathXaml;
        public MainWindowRemote Main;
        
        #region Init
        
        public RemoteNavigation()
        {
            InitializeComponent();
            CurrentPathXaml = this.FindControl<TextBox>("RemoteCurrentPath");
            _queue = new List<RemoteItem>();
            Main = MainWindowRemote.LastView;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        #endregion
        
        #region Events
        
        private void EditCurrentPath(object? sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            Main.AccessPath(((TextBox) sender!).Text);
        }

        /// <summary>
        /// The last pointer is chosen
        /// </summary>
        private void LeftArrowClick(object? sender, RoutedEventArgs e)
        {
            if (Main.RemoteNavigationView.Index > 0)
            {
                _index--;
                Main.AccessPath(_queue[_index]);
            }
        }

        /// <summary>
        /// The next pointer in the stack is chosen
        /// </summary>
        private void RightArrowClick(object? sender, RoutedEventArgs e)
        {
            if (_index < _queue.Count - 1)
            {
                _index++;
                Main.AccessPath(_queue[_index]);
            }
        }

        /// <summary>
        /// The parent is being selected
        /// </summary>
        private void UpArrowClick(object? sender, RoutedEventArgs e)
        {
            if (Main.Client.CurrentFolder is not null && Main.Client.Root.Path == Main.Client.CurrentFolder.Path)
                Main.AccessPath(Main.Client.CurrentFolder.ParentPath(Main.Client.Root));
        }

        /// <summary>
        /// The sync is being pressed
        /// </summary>
        public void SyncClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                Main.ReloadPath();
            }
            catch (Exception exception)
            {
                if (exception is ManagerException managerException)
                {
                    managerException.Errorstd = "Unable to reload file";
                    Main.SelectErrorPopUp(managerException);
                }
            }
        }

        /// <summary>
        /// Add a folder in the queue
        /// </summary>
        /// <param name="folder"></param>
        public void Add(RemoteItem folder)
        {
            if (_queue.Count - 1 == Index || Index < 0)
            {
                _queue.Add(folder);
            }
            else if (_queue.Count > Index + 1 && folder != _queue[Index + 1])
            {
                _queue.RemoveRange(Index + 1, _queue.Count - Index - 1);
                _queue.Add(folder);
            }

            Index++;
        }

        /// <summary>
        /// Add to the queue
        /// </summary>
        /// <param name="item"></param>
        public void AccessPath(RemoteItem item)
        {
            if (item.IsDir)
            {
                CurrentPathXaml.Text = item.Path;
                Add(item);
            }
        }

        public void Refresh()
        {
            if (Main.Client.CurrentFolder is not null) CurrentPathXaml.Text = Main.Client.CurrentFolder.Path;
        }
        
        #endregion
    }
}
