// System's imports
using System;
using System.Collections.Generic;
using System.IO;
// CubeTools UI's Imports
using CubeTools_UI.Views;
// Libraries Imports
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.Pointers;
// UI's Imports
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;

namespace CubeTools_UI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;
        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }
        
        #region Children ViewModels
        
        public ActionBarViewModel ViewModelActionBar;
        public ActionBarViewModel ViewModelActionBarXaml => ViewModelActionBar;
        
        public LinkBarViewModel ViewModelLinkBar;
        public LinkBarViewModel ViewModelLinkBarXaml => ViewModelLinkBar;
        
        public NavigationBarViewModel ViewModelNavigationBar;
        public NavigationBarViewModel ViewModelNavigationBarXaml => ViewModelNavigationBar;
        
        public PathsBarViewModel ViewModelPathsBar;
        public PathsBarViewModel ViewModelPathsBarXaml => ViewModelPathsBar;

        #endregion

        // CTOR
        public MainWindowViewModel()
        {
            // ActionBar : Setting up ModelXaml
            ViewModelActionBar = ActionBar.ViewModel;
            // LinkBar
            ViewModelLinkBar = LinkBar.ViewModel;
            // NavigationBar
            ViewModelNavigationBar = NavigationBar.Vie;
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
                    ErrorMessageBox(@managerException, "A critical error occured while loading the directory");
                ErrorMessageBox(new SystemErrorException("Critical error occured"), "A critical error occured while loading the directory");
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
            ViewModelPathsBar.Items = ManagerReader.ListToObservable(ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
        }
        
        /// <summary>
        ///  XAML Method : Display an Error Message Box for information purpose
        /// </summary>
        public void ErrorMessageBox(ManagerException e, string custom = "", ButtonEnum buttonEnum = ButtonEnum.Ok, Icon icon = Icon.None)
        {
            switch (e)
            {
                case PathNotFoundException :
                    icon = Icon.Warning;
                    buttonEnum = ButtonEnum.Ok;
                    custom = "The given path could not be found by CubeTools";
                    break;
                case AccessException :
                    icon = Icon.Forbidden;
                    buttonEnum = ButtonEnum.Ok;
                    custom = "The given path could not be accessed by CubeTools"; 
                    break;
                case CorruptedDirectoryException :
                case CorruptedPointerException :
                    icon = Icon.Error;
                    buttonEnum = ButtonEnum.Ok;
                    custom = "An error occured while accessing your files"; 
                    break;
                case InUseException :
                    icon = Icon.Forbidden;
                    buttonEnum = ButtonEnum.YesNo;
                    custom = "One of the given files you've selected are being used by another process" + "\n" + "Would you like to try again ?"; 
                    break;
                case PathFormatException :
                    icon = Icon.Forbidden;
                    buttonEnum = ButtonEnum.Ok;
                    custom = "The given path is incorrect, make sure it does not contain one of the invalid characters"; 
                    break;
            }
            var button = new ButtonDefinition();
            button.Type = ButtonType.Default;
            button.Name = "OK";

            var parameters = new MessageBoxCustomParams();
            parameters.Icon = icon;
            parameters.Height = 200;
            parameters.Width = 200;
            parameters.CanResize = true;
            parameters.ContentHeader = e.Errorstd;
            parameters.ContentTitle = e.Errorstd;
            parameters.ContentMessage = $"Message : {e.ErrorType}"+ "\n" + $"Level : {e.CriticalLevel}" + "\n\n" + $"{custom}";
            parameters.ShowInCenter = true;
            parameters.ButtonDefinitions = new[] {button};
            var messageBox =  MessageBoxManager.GetMessageBoxCustomWindow(parameters);
            messageBox.Show();
        }

        /// <summary>
        /// XAML Method : Display a Properties Box for information purpose
        /// </summary>
        /// <param name="ft"></param>
        /// <returns></returns>
        public async void PropertiesBox(FileType ft)
        {
            var parameters = new MessageBoxCustomParams();
            parameters.Icon = Icon.Info;
            parameters.CanResize = false;
            parameters.Height = 200;
            parameters.Width = 200;
            parameters.ContentTitle = $"{ft.Name} Properties\n";
            parameters.ContentMessage = $"File : {ft.Name}\n" +
                                        $"Type of file : {ft.Type}\n" +
                                        $"Open with : ...\n" +
                                        $"Size : {ft.SizeXaml}\n" +
                                        $"Created : {ft.Date}\n" +
                                        $"Modified : {ft.LastDate}\n" +
                                        $"Accessed : {ft.AccessDate}\n" +
                                        $"Attributes : Compressed[{ft.Compressed}] Archived[{ft.Archived}]\n" +
                                        $"             Hidden[{ft.Hidden} Read-Only[{ft.ReadOnly}]\n";
            parameters.ShowInCenter = true;
            //parameters.WindowIcon = new WindowIcon(new Bitmap("../Assets"));
            var messageBox = MessageBoxManager.GetMessageBoxCustomWindow(parameters);
            await messageBox.Show();
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
                        ErrorMessageBox(@managerException);
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
                        ErrorMessageBox(@managerException, "A critical error occured while loading the directory");
                    ErrorMessageBox(new SystemErrorException("Critical error occured"),
                        "A critical error occured while loading the directory");
                }

                // Setting up the Path for UI
                ViewModelNavigationBar.AttachedView.CurrentPathXaml.Text = ViewModelNavigationBar.DirectoryPointer.Path;
                // Modified ListBox associated
                ViewModelPathsBar.AttachedView.ItemsXaml.Items = ManagerReader.ListToObservable(ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
                // Adding path to queue
                ViewModelNavigationBar.QueuePointers.Add(path);
                if (ViewModelNavigationBar.QueuePointers.Count >= 9)
                {
                    ViewModelNavigationBar.QueuePointers.RemoveAt(0);
                    ViewModelNavigationBar.QueueIndex--;
                }
            }
            ViewModelActionBar.SelectedXaml = new List<FileType>();
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
                    ErrorMessageBox(@managerException);
            }
            ViewModelPathsBar.Items = ManagerReader.ListToObservable(ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
        }
    }
}