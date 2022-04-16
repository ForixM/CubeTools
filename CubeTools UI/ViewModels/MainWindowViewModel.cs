// System's imports
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
// CubeTools UI's Imports
using CubeTools_UI.Models;
using CubeTools_UI.Views;
// Libraries Imports
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using Library.Pointers;
// UI's Imports
using MessageBox.Avalonia;
using MessageBox.Avalonia.BaseWindows.Base;
using MessageBox.Avalonia.Enums;
using ReactiveUI;

namespace CubeTools_UI.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        // MODELS
        public MainWindowModel Model;
        // VIEW MODELS
        public ActionBarViewModel ViewModelActionBar;
        public ActionBarViewModel ViewModelActionBarXaml => ViewModelActionBar;
        
        public LinkBarViewModel ViewModelLinkBar;
        public LinkBarViewModel ViewModelLinkBarXaml => ViewModelLinkBar;
        
        public NavigationBarViewModel ViewModelNavigationBar;
        public NavigationBarViewModel ViewModelNavigationBarXaml => ViewModelNavigationBar;
        
        public PathsBarViewModel ViewModelPathsBar;
        public PathsBarViewModel ViewModelPathsBarXaml => ViewModelPathsBar;

        
        private ObservableCollection<FileType> items;
        public ObservableCollection<FileType> Items
        {
            get => ManagerReader.ListToObservable(Model.ModelNavigationBar.DirectoryPointer.ChildrenFiles);
            set
            {
                foreach (var ft in value)
                {
                    items.Add(ft);
                }
                this.RaiseAndSetIfChanged(ref items, value);
            }
        }
        
        // Children
        public string CurrentPath
        {
            get => Model.ModelNavigationBar.DirectoryPointer.Path;
            set
            {
                if (Directory.Exists(value))
                {
                    string val = Model.ModelNavigationBar.DirectoryPointer.Path;
                    this.RaiseAndSetIfChanged(ref val, value);
                    if (Model.ModelNavigationBar.DirectoryPointer.Path != val)
                        ChangeDirectory(ref val);
                }
            }
        }

        // CTOR
        public MainWindowViewModel()
        {
            Model = new MainWindowModel(this);
            // ActionBar : Setting up ModelXaml
            ViewModelActionBar = ActionBar.ViewModel;
            ViewModelActionBar.ModelXaml = Model;
            // LinkBar
            ViewModelLinkBar = LinkBar.ViewModel;
            ViewModelLinkBar.ModelXaml = Model;
            // NavigationBar
            ViewModelNavigationBar = NavigationBar.ViewModel;
            ViewModelNavigationBar.ModelXaml = Model;
            // PathsBar
            ViewModelPathsBar = PathsBar.ViewModel;
            ViewModelPathsBar.ModelXaml = Model;
            // Initialize References of Models
            Model.Initialize();
            try
            {
                // Setting up loaded directory
                Model.ModelNavigationBar.DirectoryPointer = new DirectoryType(Directory.GetCurrentDirectory());
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e, e.Errorstd);
            }
            // Referencing Models
            ViewModelActionBar.ModelXaml = Model;
            ViewModelLinkBar.ModelXaml = Model;
            ViewModelNavigationBar.ModelXaml = Model;
            ViewModelPathsBar.ModelXaml = Model;
            // Referencing SubModels
            ViewModelActionBar.ModelActionBarXaml = Model.ModelActionBar;
            ViewModelLinkBar.ModelLinkBarXaml = Model.ModelLinkBar;
            ViewModelNavigationBar.ModelNavigationBarXaml = Model.ModelNavigationBar;
            ViewModelPathsBar.ModelPathsBar = Model.ModelPathsBar;
            // Referencing THIS
            ViewModelNavigationBar.ParentViewModelXaml = this;
            // Setting up current path
            CurrentPath = Model.ModelNavigationBar.DirectoryPointer.Path;
            Model.ModelNavigationBar.QueuePointers = new List<string>(){CurrentPath};
            Model.ModelNavigationBar.QueueIndex = 0;
            // Setting up paths
            items = new ObservableCollection<FileType>();
            Items = ManagerReader.ListToObservable(Model.ModelNavigationBar.DirectoryPointer.ChildrenFiles);;
        }
        
        /// <summary>
        ///  XAML Method : Display an Error Message Box for information purpose
        /// </summary>
        public IMsBoxWindow<ButtonResult> ErrorMessageBox(ManagerException e, string custom = "", ButtonEnum buttonEnum = ButtonEnum.Ok, Icon icon = Icon.None)
        {
            var content = $"{e.Errorstd}"+ "\n" + $"{custom}";
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
            var messageBox =  MessageBoxManager.GetMessageBoxStandardWindow(e.ErrorType, content, buttonEnum, icon);
            messageBox.Show();
            return messageBox;
        }
        
        public void ChangeDirectory(ref string path)
        {
            // Modify the directory
            Model.ModelNavigationBar.DirectoryPointer.ChangeDirectory(path);
            // Setting up the Path for UI
            CurrentPath = path;
            // Modified ListBox associated
            Items = ManagerReader.ListToObservable(Model.ModelNavigationBar.DirectoryPointer.ChildrenFiles);
            // Adding path to queue
            Model.ModelNavigationBar.QueuePointers.Add(path);
            if (Model.ModelNavigationBar.QueuePointers.Count >= 9)
            {
                Model.ModelNavigationBar.QueuePointers.RemoveAt(0);
                Model.ModelNavigationBar.QueueIndex--;
            }
            Model.ModelActionBar.SelectedXaml = new List<FileType>();
        }

    }
}