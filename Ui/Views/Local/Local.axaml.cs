using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Library;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Ui.Views.Error;
using Ui.Views.Remote;

namespace Ui.Views.Local
{
    public class Local : UserControl
    {
        public static Local LastReference;
        public readonly Window Main;
        public bool IsRemote;

        public ActionBar ActionBarView;
        public NavigationBar NavigationBarView;
        public PathsBar PathsBarView;

        public Local()
        {
            IsRemote = false;
            LastReference = this;
            Main = MainWindow.MainWindow.LastReference;
            InitializeComponent();
            ActionBarView = this.FindControl<ActionBar>("ActionBar");
            NavigationBarView = this.FindControl<NavigationBar>("NavigationBar");
            PathsBarView = this.FindControl<PathsBar>("PathsBar");
            
            string path = Directory.GetCurrentDirectory();
            NavigationBarView.AccessPath(path);
            NavigationBarView.Add(path);
            PathsBarView.Refresh();
        }
        public Local(bool isRemote = false)
        {
            IsRemote = isRemote;
            LastReference = this;
            Main = !isRemote ?  MainWindow.MainWindow.LastReference : MainWindowRemote.LastReference;
            InitializeComponent();
            ActionBarView = this.FindControl<ActionBar>("ActionBar");
            NavigationBarView = this.FindControl<NavigationBar>("NavigationBar");
            PathsBarView = this.FindControl<PathsBar>("PathsBar");
            
            string path = Directory.GetCurrentDirectory();
            NavigationBarView.AccessPath(path);
            NavigationBarView.Add(path);
            PathsBarView.Refresh();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #region Process
        
        /// <summary>
        /// Access the given path by reloading the current directory or accessing a file
        /// </summary>
        /// <param name="path">The given path to access (Either a file or a directory)</param>
        public void AccessPath(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    ManagerReader.AutoLaunchAppProcess(path);
                }
                catch (Exception e)
                {
                    if (e is ManagerException @managerException) new ErrorBase(@managerException).ShowDialog<object>(Main);
                    else new ErrorBase(new SystemErrorException("System was unable to open your file", "AccessPath")).ShowDialog<bool>(Main);
                }
            }
            else
            {
                NavigationBarView.AccessPath(path);
                PathsBarView.Refresh();
            }
        }

        /// <summary>
        /// Reload the current directory
        /// </summary>
        public void Refresh()
        {
            try
            {
                NavigationBarView.FolderLocalPointer.SetChildrenFiles();
            }
            catch (Exception e)
            {
                if (e is ManagerException @managerException) new ErrorBase(@managerException).ShowDialog<object>(Main);
            }
            PathsBarView.Refresh();
        }

        /// <summary>
        ///  Reload the current directory (not the pointer) by displaying specific pointers
        /// </summary>
        /// <param name="list">the list of pointer to display</param>
        public void Refresh(List<LocalPointer> list) => PathsBarView.Refresh(list);
        
        #endregion
    }
}