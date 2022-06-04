using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Library;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Ui.Views.ErrorPopUp;

namespace Ui.Views.Local
{
    public class Local : UserControl
    {
        public static Local LastReference;
        public readonly MainWindow.MainWindow Main;

        public ActionBar ActionBarView;
        public NavigationBar NavigationBarView;
        public PathsBar PathsBarView;

        public Local()
        {
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
                    if (e is ManagerException @managerException) SelectErrorPopUp(@managerException);
                    else SelectErrorPopUp(new SystemErrorException("System was unable to open your file", "AccessPath"));
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
                NavigationBarView.FolderPointer.SetChildrenFiles();
            }
            catch (Exception e)
            {
                if (e is ManagerException @managerException) SelectErrorPopUp(@managerException);
            }
            PathsBarView.Refresh();
        }

        /// <summary>
        ///  Reload the current directory (not the pointer) by displaying specific pointers
        /// </summary>
        /// <param name="list">the list of pointer to display</param>
        public void Refresh(List<Pointer> list) => PathsBarView.Refresh(list);
        
        /// <summary>
        /// Select and generate the correct popup according to the exception given in parameter
        /// </summary>
        /// <param name="exception"></param>
        public void SelectErrorPopUp(ManagerException exception)
        {
            switch (exception)
            {
                case PathNotFoundException @pathNotFoundException:
                    new PathNotFoundPopUp(@pathNotFoundException).Show();
                    break;
                case AccessException @accessException:
                    new AccessDeniedPopUp(@accessException).Show();
                    break;
                case DiskNotReadyException @diskNotReadyException:
                    new DiskNotReadyPopUp(@diskNotReadyException).Show();
                    break;
                case SystemErrorException @systemErrorException:
                    new SystemErrorPopUp(@systemErrorException).Show();
                    break;
                default:
                    new ErrorPopUp.ErrorPopUp(exception).Show();
                    break;
            }
        }
        #endregion
    }
}