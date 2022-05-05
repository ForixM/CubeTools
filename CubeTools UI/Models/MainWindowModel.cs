// System's imports
using System;
using System.Collections.Generic;
using System.IO;
// CubeTools UIs Imports
using CubeTools_UI.Views;
// Libraries Imports
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.Pointers;

namespace CubeTools_UI.Models
{
    public class MainWindowModel : BaseModel
    {

        public MainWindow View;
        public static string CubeToolsPath;

        public bool IsCtrlPressed;
        
        #region Children ViewModels
        
        public readonly ActionBarModel ModelActionBar;
        public readonly LinkBarModel ModelLinkBar;
        public readonly NavigationBarModel ModelNavigationBar;
        public readonly PathsBarModel ModelPathsBar;

        #endregion

        #region Init
        
        public MainWindowModel()
        {
            IsCtrlPressed = false;
            CubeToolsPath = Directory.GetCurrentDirectory();
            // ActionBar : Setting up ModelXaml
            ModelActionBar = ActionBar.Model;
            // LinkBar
            ModelLinkBar = LinkBar.Model;
            // NavigationBar
            ModelNavigationBar = NavigationBar.Model;
            // PathsBar
            ModelPathsBar = PathsBar.Model;
            try
            {
                // Setting up loaded directory
                ModelNavigationBar.DirectoryPointer = new DirectoryType(Directory.GetCurrentDirectory());
            }
            catch (Exception e)
            {
                if (e is ManagerException @managerException)
                {
                    var popup = new Views.ErrorPopUp.ErrorPopUp(this, @managerException);
                }
                else
                {
                    var popup = new Views.ErrorPopUp.SystemErrorPopUp(this, new SystemErrorException("Critical error occured while loading the directory"));
                }
            }
            // Referencing THIS
            ModelNavigationBar.ParentModel = this;
            ModelPathsBar.ParentModel = this;
            ModelLinkBar.ParentModel = this;
            ModelActionBar.ParentModel = this;
            // Setting up current path
            ModelNavigationBar.View.CurrentPathXaml.Text = ModelNavigationBar.DirectoryPointer.Path;
            // Setting up Queue
            ModelNavigationBar.QueuePointers = new List<string>(){ModelNavigationBar.DirectoryPointer.Path};
            ModelNavigationBar.QueueIndex = 0;
            // Setting up Items
            ModelPathsBar.ReloadPath(ModelNavigationBar.DirectoryPointer.ChildrenFiles);
        }

        public MainWindowModel(MainWindow view) : this()
        {
            View = view;
        }

        #endregion

        #region Process
        
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
                    ModelNavigationBar.DirectoryPointer.ChangeDirectory(path);
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
                ModelNavigationBar.View.CurrentPathXaml.Text = ModelNavigationBar.DirectoryPointer.Path;
                // Modified ListBox associated
                ModelPathsBar.ReloadPath(ModelNavigationBar.DirectoryPointer.ChildrenFiles);
                // Adding path to queue
                ModelNavigationBar.QueuePointers.Add(path);
                if (ModelNavigationBar.QueuePointers.Count >= 9)
                {
                    ModelNavigationBar.QueuePointers.RemoveAt(0);
                    ModelNavigationBar.QueueIndex--;
                }
            }
            ModelActionBar.SelectedXaml = new List<PointerItem>();
        }

        /// <summary>
        /// Reload the current directory
        /// </summary>
        public void ReloadPath()
        {
            try
            {
                ModelNavigationBar.DirectoryPointer.SetChildrenFiles();
            }
            catch (Exception e)
            {
                if (e is ManagerException @managerException)
                {
                    var popup = new Views.ErrorPopUp.ErrorPopUp(this, @managerException);
                    popup.Show();
                }
            }
            ModelPathsBar.ReloadPath(ModelNavigationBar.DirectoryPointer.ChildrenFiles);
        }
        #endregion
    }
}