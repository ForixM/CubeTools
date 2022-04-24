// System's imports
using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
// CubeTools UIs Imports
using CubeTools_UI.Views;
// Libraries Imports
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.Pointers;

namespace CubeTools_UI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {

        public MainWindow AttachedView;
        public static string CubeToolsPath;
        
        #region Children ViewModels
        
        public readonly ActionBarViewModel ViewModelActionBar;
        public readonly LinkBarViewModel ViewModelLinkBar;
        public readonly NavigationBarViewModel ViewModelNavigationBar;
        public readonly PathsBarViewModel ViewModelPathsBar;

        #endregion

        // CTOR
        public MainWindowViewModel()
        {
            CubeToolsPath = Directory.GetCurrentDirectory();
            // ActionBar : Setting up ModelXaml
            ViewModelActionBar = ActionBar.ViewModel;
            // LinkBar
            ViewModelLinkBar = LinkBar.ViewModel;
            // NavigationBar
            ViewModelNavigationBar = NavigationBar.ViewModel;
            // PathsBar
            ViewModelPathsBar = PathsBar.ViewModel;
            try
            {
                // Setting up loaded directory
                ViewModelNavigationBar.DirectoryPointer = new DirectoryType(Directory.GetCurrentDirectory());
            }
            catch (Exception e)
            {
                if (e is ManagerException @managerException)
                {
                    var popup = new Views.ErrorPopUp.ErrorPopUp(this, @managerException);
                    popup.Show();
                }
                else
                {
                    var popup = new Views.ErrorPopUp.ErrorPopUp(this, new SystemErrorException("Critical error occured while loading the directory"));
                    popup.Show();
                }
            }
            // Referencing THIS
            ViewModelNavigationBar.ParentViewModel = this;
            ViewModelPathsBar.ParentViewModel = this;
            ViewModelLinkBar.ParentViewModel = this;
            ViewModelActionBar.ParentViewModel = this;
            // Setting up current path
            ViewModelNavigationBar.AttachedView.CurrentPathXaml.Text = ViewModelNavigationBar.DirectoryPointer.Path;
            // Setting up Queue
            ViewModelNavigationBar.QueuePointers = new List<string>(){ViewModelNavigationBar.DirectoryPointer.Path};
            ViewModelNavigationBar.QueueIndex = 0;
            // Setting up Items
            ViewModelPathsBar.ReloadPath(ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
        }

        public MainWindowViewModel(MainWindow attachedView) : this()
        {
            AttachedView = attachedView;
        }

        /// <summary>
        /// Access the given path by reloading the current directory or accessing a file
        /// </summary>
        /// <param name="path">The given path to access (Either a file or a directory)</param>
        /// <param name="isdir">Whether it is a directory or not</param>
        public void AccessPath(string path, bool isdir=true)
        {
            if (!isdir)
            {
                try
                {
                    ManagerReader.AutoLaunchAppProcess(path);
                }
                catch (Exception e)
                {
                    if (e is ManagerException @managerException)
                    {
                        var popup = new Views.ErrorPopUp.ErrorPopUp(this, managerException);
                        popup.Show();
                    }
                }
            }
            else
            {
                try
                {
                    ViewModelNavigationBar.DirectoryPointer.ChangeDirectory(path);
                }
                catch (Exception e)
                {
                    if (e is ManagerException @managerException)
                    {
                        var popup = new Views.ErrorPopUp.ErrorPopUp(this, @managerException);
                        popup.Show();
                    }
                    else
                    {
                        var popup = new Views.ErrorPopUp.ErrorPopUp(this, new SystemErrorException("Critical error occured while loading the directory"));
                        popup.Show();
                    }
                }
                // Setting up the Path for UI
                ViewModelNavigationBar.AttachedView.CurrentPathXaml.Text = ViewModelNavigationBar.DirectoryPointer.Path;
                // Modified ListBox associated
                ViewModelPathsBar.ReloadPath(ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
                // Adding path to queue
                ViewModelNavigationBar.QueuePointers.Add(path);
                if (ViewModelNavigationBar.QueuePointers.Count >= 9)
                {
                    ViewModelNavigationBar.QueuePointers.RemoveAt(0);
                    ViewModelNavigationBar.QueueIndex--;
                }
            }
            ViewModelActionBar.SelectedXaml = new List<PointerItem>();
        }

        /// <summary>
        /// Reload the current directory
        /// </summary>
        public void ReloadPath()
        {
            try
            {
                ViewModelNavigationBar.DirectoryPointer.SetChildrenFiles();
            }
            catch (Exception e)
            {
                if (e is ManagerException @managerException)
                {
                    var popup = new Views.ErrorPopUp.ErrorPopUp(this, @managerException);
                    popup.Show();
                }
            }
            ViewModelPathsBar.ReloadPath(ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
        }
    }
}