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
using Ui.Views.Settings;

namespace Ui.Views.Local
{
    public class NavigationBar : UserControl
    {
        #region Model Variables
        
        // A pointer to the current loaded Directory
        public DirectoryPointerLoaded FolderPointer;
        // Queue Pointers : Pointers registered in a queue
        public List<DirectoryPointer> QueuePointers;
        // Index Queue : the current index of the queue
        public int QueueIndex;
        
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
            QueueIndex = -1;
            QueuePointers = new List<DirectoryPointer>();
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
            if (QueueIndex > 0)
            {
                QueueIndex--;
                Main.AccessPath(QueuePointers[QueueIndex].Path);
            }
        }

        /// <summary>
        /// The next pointer in the stack is chosen
        /// </summary>
        private void RightArrowClick(object? sender, RoutedEventArgs e)
        {
            if (QueueIndex < QueuePointers.Count - 1)
            {
                QueueIndex++;
                Main.AccessPath(QueuePointers[QueueIndex].Path);
            }
            // End of the queue
        }

        /// <summary>
        /// The parent is being selected
        /// </summary>
        private void UpArrowClick(object? sender, RoutedEventArgs e)
        {
            Main.AccessPath(ManagerReader.GetParent(Main.NavigationBarView.FolderPointer.Path));
            Add(new DirectoryPointerLoaded(Main.NavigationBarView.FolderPointer.Path));
        }

        /// <summary>
        /// The sync is being pressed
        /// </summary>
        public void SyncClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                Main.NavigationBarView.FolderPointer.SetChildrenFiles();
                Main.ReloadPath();
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = "Unable to reload files and folders";
                    Main.SelectErrorPopUp(@managerException);
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
        public void Add(DirectoryPointer folder)
        {
            if (QueuePointers.Count - 1 == QueueIndex || QueueIndex < 0)
            {
                QueuePointers.Add(folder);
            }
            else if (QueuePointers.Count > QueueIndex + 1 && folder != QueuePointers[QueueIndex + 1])
            {
                QueuePointers.RemoveRange(QueueIndex + 1, QueuePointers.Count - QueueIndex - 1);
                QueuePointers.Add(folder);
            }

            QueueIndex++;
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
                FolderPointer.ChangeDirectory(path);
            }
            catch (ManagerException e)
            {
                Main.SelectErrorPopUp(e);
            }
            CurrentPathXaml.Text = FolderPointer.Path;
            Add(FolderPointer);
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
                Main.SelectErrorPopUp(e);
            }
        }
        
        #endregion
    }
}
