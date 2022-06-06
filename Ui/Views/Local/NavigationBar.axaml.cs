using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.DirectoryPointer;
using Library.DirectoryPointer.DirectoryPointerLoaded;
using Ui.Views.Error;
using Ui.Views.Settings;

namespace Ui.Views.Local
{
    public class NavigationBar : UserControl
    {
        #region Model Variables
        
        // A pointer to the current loaded Directory
        public DirectoryPointerLoaded FolderPointer;
        // Queue Pointers : Pointers registered in a queue
        private List<string> _queue;
        // Index Queue : the current index of the queue
        private int _index;
        
        #endregion

        #region Reference Variables
        
        public readonly Local Main;
        public TextBox CurrentPathXaml;
        
        #endregion
        
        #region Init
        public NavigationBar()
        {
            Main = Local.LastReference;
            InitializeComponent();
            CurrentPathXaml = this.FindControl<TextBox>("CurrentPath");
            _index = -1;
            _queue = new List<string>();
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
            // End of the queue
        }

        /// <summary>
        /// The parent is being selected
        /// </summary>
        private void UpArrowClick(object? sender, RoutedEventArgs e)
        {
            string pathParent = ManagerReader.GetParent(FolderPointer.Path);
            if (pathParent != "")
            {
                Main.AccessPath(pathParent);
                Add(FolderPointer.Path);
            }
        }

        /// <summary>
        /// The sync is being pressed
        /// </summary>
        public void SyncClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                Main.NavigationBarView.FolderPointer.SetChildrenFiles();
                Main.Refresh();
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = "Unable to reload files and folders";
                    new ErrorBase(@managerException).ShowDialog<object>(Main.Main);
                }
            }
        }
        
        /// <summary>
        /// The settings is opened
        /// </summary>
        private void SettingsClick(object? sender, RoutedEventArgs e) => new SettingsWindow().Show();
        
        #endregion
        
        #region Process

        /// <summary>
        ///     Add the folder to the queue of the visited folders
        /// </summary>
        /// <param name="folder"></param>
        public void Add(string folder)
        {
            if (_queue.Count - 1 == _index || _index < 0)
                _queue.Add(folder);
            else if (_queue.Count > _index + 1 && folder != _queue[_index + 1])
            {
                _queue.RemoveRange(_index + 1, _queue.Count - _index - 1);
                _queue.Add(folder);
            }

            _index++;
        }
        
        /// <summary>
        ///     Access the path by changing the loaded folder
        /// </summary>
        /// <param name="path">the given path to access</param>
        public void AccessPath(string path)
        {
            try
            {
                if (FolderPointer is null) FolderPointer = new DirectoryPointerLoaded(path);
                else FolderPointer.ChangeDirectory(path);
            }
            catch (ManagerException e)
            {
                new ErrorBase(e).Show();
                //new ErrorBase(e).ShowDialog<object>(Main.Main);
            }
            CurrentPathXaml.Text = FolderPointer.Path;
            //Add(FolderPointer);
        }

        /// <summary>
        ///     Reload the pointer
        /// </summary>
        public void ReloadPath()
        {
            try
            {
                FolderPointer.SetChildrenFiles();
            }
            catch (ManagerException e)
            {
                new ErrorBase(e).ShowDialog<object>(Main.Main);
            }
        }
        
        #endregion
    }
}
