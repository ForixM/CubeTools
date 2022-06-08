using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Ui.Views.Error;
using Ui.Views.Settings;
using Pointer = Library.Pointer;

namespace Ui.Views
{
    public class NavigationView : UserControl
    {
        #region Model Variables
        
        // Queue Pointers : Pointers registered in a queue
        private List<Pointer> _queue;
        // Index Queue : the current index of the queue
        private int _index;
        
        #endregion

        #region Reference Variables
        
        public readonly OneClient Main;
        public TextBox CurrentPathXaml;
        
        #endregion
        
        #region Init
        public NavigationView()
        {
            Main = OneClient.LastReference;
            InitializeComponent();
            CurrentPathXaml = this.FindControl<TextBox>("CurrentPath");
            _index = -1;
            _queue = new List<Pointer>();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        #endregion
        
        #region Events
        
        private void EditCurrentPath(object? sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            if (((TextBox) sender).Text == "")
            {
                Main.PointersView = new Menu();
            } 
            else
            {
                Main.AccessPath(((TextBox) sender!).Text);
            }
        }

        /// <summary>
        /// The last pointer is chosen
        /// </summary>
        private void LeftArrowClick(object? sender, RoutedEventArgs e)
        {
            if (_index > 0)
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
            if (Main.Client.CurrentFolder is not null && Main.Client.Root.Path != Main.Client.CurrentFolder.Path)
            {
                Main.AccessPath(Main.Client.GetParentReference(Main.Client.CurrentFolder));
                if (Main.Client.CurrentFolder is { } folder)
                    Main.NavigationView.Add(folder);
            }
        }

        /// <summary>
        /// The sync is being pressed
        /// </summary>
        public void SyncClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                Main.Refresh();
            }
            catch (Exception exception)
            {
                if (exception is ManagerException managerException)
                {
                    managerException.Errorstd = "Unable to reload file";
                    new ErrorBase(managerException).ShowDialog<object>(Main.Main);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Refresh()
        {
            if (Main.Client.CurrentFolder is not null) CurrentPathXaml.Text = Main.Client.CurrentFolder.Path;
        }

        private void SettingsClick(object? sender, RoutedEventArgs e) => new SettingsWindow().Show();
        
        #endregion
        
        #region Process

        /// <summary>
        ///     Access the path by changing the loaded pointer
        /// </summary>
        /// <param name="pointer">the given path to access</param>
        public void AccessPath(Pointer pointer)
        {
            try
            {
                Main.Client.AccessPath(pointer);
            }
            catch (ManagerException e)
            {
                new ErrorBase(e).Show();
                //new ErrorBase(e).ShowDialog<object>(Main.Main);
            }
            CurrentPathXaml.Text = Main.Client.CurrentFolder!.Path;
        }

        /// <summary>
        ///     Reload the pointer
        /// </summary>
        public void ReloadPath()
        {
            try
            {
                Main.Client.Refresh();
                Main.NavigationView.ReloadPath();
            }
            catch (ManagerException e)
            {
                new ErrorBase(e).ShowDialog<object>(Main.Main);
            }
        }
        
        /// <summary>
        /// Add a folder in the queue
        /// </summary>
        /// <param name="folder">the folder to add</param>
        public void Add(Pointer folder)
        {
            if (_queue.Count - 1 == _index || _index < 0) _queue.Add(folder);
            else if (_queue.Count > _index + 1 && folder != _queue[_index + 1])
            {
                _queue.RemoveRange(_index + 1, _queue.Count - _index - 1);
                _queue.Add(folder);
            }

            _index++;
        }
        
        #endregion
    }
}
