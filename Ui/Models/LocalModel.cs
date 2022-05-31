// System's imports
using System;
using System.Collections.Generic;
using System.IO;
// CubeTools UIs Imports
// Libraries Imports
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library;
using Ui.Views;
using Ui.Views.ErrorPopUp;

namespace Ui.Models
{
    public class LocalModel
    {
        public Local? View;
        public MainWindowModel? ParentModel;

        public bool IsCtrlPressed;
        
        public readonly ActionBarModel ModelActionBar;
        public readonly NavigationBarModel ModelNavigationBar;
        public readonly PathsBarModel ModelPathsBar;

        #region Init
        
        public LocalModel()
        {
            // Control
            IsCtrlPressed = false;
            // Models
            ModelActionBar = ActionBar.LastModel!;
            ModelNavigationBar = NavigationBar.LastModel!;
            ModelPathsBar = PathsBar.LastModel!;
            // Referencing THIS
            ModelNavigationBar.ParentModel = this;
            ModelPathsBar.ParentModel = this;
            ModelActionBar.ParentModel = this;
            // setting up Directory
            try
            {
                ModelNavigationBar.FolderPointer = new DirectoryPointer(Directory.GetCurrentDirectory());
            }
            catch (ManagerException e)
            {
                SelectErrorPopUp(e);
            }
            // Setting up current path
            ModelNavigationBar.View.CurrentPathXaml.Text = ModelNavigationBar.FolderPointer.Path;
            // Setting up Queue
            ModelNavigationBar.Add(new DirectoryPointer(ModelNavigationBar.FolderPointer.Path));
            // Setting up Items
            ModelPathsBar.ReloadPath(ModelNavigationBar.FolderPointer.ChildrenFiles);
        }

        public LocalModel(Local view, MainWindowModel parentModel) : this()
        {
            View = view;
            ParentModel = parentModel;
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
                    if (e is ManagerException @managerException) SelectErrorPopUp(@managerException);
                }
            }
            else
            {
                try
                {
                    ModelNavigationBar.FolderPointer.ChangeDirectory(path);
                }
                catch (Exception e)
                {
                    if (e is ManagerException @managerException) SelectErrorPopUp(@managerException);
                    else SelectErrorPopUp(new SystemErrorException("Critical error occured while loading the folder"));
                }
                // Setting up the Path for UI
                ModelNavigationBar.View.CurrentPathXaml.Text = ModelNavigationBar.FolderPointer.Path;
                // Modified ListBox associated
                ModelPathsBar.ReloadPath(ModelNavigationBar.FolderPointer.ChildrenFiles);
                // Adding path to queue
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
                ModelNavigationBar.FolderPointer.SetChildrenFiles();
            }
            catch (Exception e)
            {
                if (e is ManagerException @managerException) SelectErrorPopUp(@managerException);
            }
            ModelPathsBar.ReloadPath(ModelNavigationBar.FolderPointer.ChildrenFiles);
        }
        
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
                    new ErrorPopUp(exception).Show();
                    break;
            }
        }
        #endregion
    }
}