// System's imports

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using Library.Pointers;
using MessageBox.Avalonia;
using MessageBox.Avalonia.BaseWindows.Base;
using MessageBox.Avalonia.Enums;
using ReactiveUI;
// Library's imports
// CubeTools UI's imports


namespace CubeTools_UI.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        // Reference to self useful to generate sub-views and get the DataContext
        public MainWindowViewModel ReferenceToSelf => this;
        // A pointer to the current loaded Directory
        // Consider using it most of the time because used in CLI version
        public DirectoryType DirectoryPointer;
        // Give all the current selected FT
        public List<FileType> Selected;
        // Give all the copied FT
        public List<FileType> Copied;
        // Queue of path that has been loaded (no more than 8 path, better for memory)
        public List<string> QueuePointers;
        // Index Queue : the current index of the queue
        public int QueueIndex;
        // Static Paths
        public readonly string HomePath;
        public readonly string DesktopPath;
        public readonly string DocumentPath;
        public readonly string DownloadPath;
        public readonly string PicturePath;
        public readonly string TrashPath;

        // The files and folders loaded in XAML code
        // items stores the values
        private ObservableCollection<FileType> items;

        // Items help to coordinate every thing
        // 
        public ObservableCollection<FileType> Items
        {
            get => ManagerReader.ListToObservable(DirectoryPointer.ChildrenFiles);
            set
            {
                items.Clear();
                foreach (var ft in value) items.Add(ft);
                this.RaiseAndSetIfChanged(ref items, value);
                
            }
        }

        // The currentPath associated to the loaded directory's path
        // Can either set the directory's path if the user enters a path or returns the directory path's value.
        public string CurrentPath
        {
            get => DirectoryPointer.Path;
            set
            {
                // If the given value exists, deal with it
                if (Directory.Exists(value))
                {
                    // If CurrentPath has been set by user, we have to change the directory
                    if (DirectoryPointer.Path != value)
                        ChangeDirectory(value);
                    // Modify the value in XAML
                    var res = value;
                    this.RaiseAndSetIfChanged(ref res, value);
                }
            }
        }
        
        public MainWindowViewModel()
        {
            // Variables
            Selected = new List<FileType>();
            Copied = new List<FileType>();
            QueuePointers = new List<string>();
            QueueIndex = 0;
            // TODO Implement algorithm to detect static path
            HomePath = "C:/Users/mateo";
            DesktopPath = "C:/Users/mateo/Desktop";
            DocumentPath = "C:/Users/mateo/OneDrive";
            DownloadPath = "C:/Users/mateo/Downloads";
            PicturePath = "C:/Users/mateo/Pictures";
            TrashPath = "C:/$Recycle.Bin";
            try
            {
                DirectoryPointer = new DirectoryType(Directory.GetCurrentDirectory());
            }
            catch (ManagerException e)
            {
                //ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
            finally
            {
                QueuePointers.Add(DirectoryPointer.Path);
                CurrentPath = DirectoryPointer.Path;
            }

            items = new ObservableCollection<FileType>();
            foreach (var i in DirectoryPointer.ChildrenFiles) items.Add(i);
        }
        

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public IMsBoxWindow<ButtonResult> ErrorMessageBox(ManagerException e, string custom = "", ButtonEnum buttonEnum = ButtonEnum.Ok, Icon icon = Icon.None)
        {
            var content = $"{e.Errorstd}"+ "\n" + $"{custom}";
            switch (e)
            {
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

        #region BindingMethods

        // All functions are called in XAML code

        /// <summary>
        ///     - Action : Get last value stored before the current one in the queue and set the directory<br></br>
        ///     - XAML : Change the directory and reload page<br></br>
        ///     - Implementation : NOT CHECK <br></br>
        /// </summary>
        public void LeftBtnClick()
        {
            // End of the queue
            if (QueueIndex <= 0)
                return;

            // Get the index before
            QueueIndex--;
            try
            {
                // Change the directory
                ChangeDirectory(QueuePointers[QueueIndex]);
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e, "Impossible to get the last directory : maybe it does not exist anymore");
            }
        }

        /// <summary>
        ///     - Action : Get the next value stored after the current one in the queue.<br></br>
        ///     - XAML : Change the directory and reload page<br></br>
        ///     - Implementation : NOT CHECK
        /// </summary>
        public void RightBtnClick()
        {
            if (QueueIndex >= QueuePointers.Count - 1)
                return;
            // Modifying selected items
            QueueIndex++;
            try
            {
                // Getting value in the queue
                ChangeDirectory(QueuePointers[QueueIndex]);
            }
            catch (ManagerException e)
            {
                //ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        /// <summary>
        ///     - Action : Get the parent directory and set it in the env<br></br>
        ///     - XAML : Change the current directory to its parent<br></br>
        ///     - Implementation : NOT CHECK - //TODO Resolve Bugs
        /// </summary>
        public void UpBtnClick()
        {
            QueueIndex++;
            try
            {
                ChangeDirectory(ManagerReader.GetParent(DirectoryPointer.Path));
            }
            catch (ManagerException e)
            {
                //ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void SyncBtnClick()
        {
            // TODO Add OneDrive implementation
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void SettingBtnClick()
        {
        }

        

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void GoToHomeBtnClick()
        {
            try
            {
                ChangeDirectory(HomePath);
            }
            catch (ManagerException e)
            {
                //ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void GoToDesktopBtnClick()
        {
            try
            {
                ChangeDirectory(DesktopPath);
            }
            catch (ManagerException e)
            {
                //ErrorMessageBox(e, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void GoToDocumentsBtnClick()
        {
            try
            {
                ChangeDirectory(DocumentPath);
            }
            catch (ManagerException e)
            {
                //ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void GoToDownloadBtnClick()
        {
            try
            {
                ChangeDirectory(DownloadPath);
            }
            catch (ManagerException e)
            {
                //ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void GoToPictureBtnClick()
        {
            try
            {
                ChangeDirectory(PicturePath);
            }
            catch (ManagerException e)
            {
                //ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void GoToTrashBtnClick()
        {
            try
            {
                ChangeDirectory(TrashPath);
            }
            catch (ManagerException e)
            {
                //ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void GoToSubFile()
        {
            try
            {
                if (Selected[0].IsDir)
                    ChangeDirectory(Selected[0]);
                else
                    Open(Selected[0].Path);
            }
            catch (ManagerException e)
            {
                //ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        #endregion

        #region ProcessMethods

        

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="ft"></param>
        /// <returns></returns>
        private bool Copy(FileType ft)
        {
            if (File.Exists(ft.Path) || Directory.Exists(ft.Path))
            {
                Copied.Add(ft);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="fts"></param>
        /// <exception cref="PathNotFoundException"></exception>
        public void Copy(List<FileType> fts)
        {
            Copied.Clear();
            foreach (var ft in fts)
                if (!Copy(ft))
                {
                    Selected.Clear();
                    throw new PathNotFoundException("The selected file :" + ft.Name + " does not exist anymore",
                        "Copy");
                }
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="ft"></param>
        private static void Cut(FileType ft)
        {
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="fts"></param>
        public static void Cut(List<FileType> fts)
        {
            foreach (var ft in fts) Cut(ft);
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="ft"></param>
        private void PasteOne(FileType ft)
        {
            ManagerWriter.Copy(ref DirectoryPointer, ft, ft.Path);
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void PasteCopied()
        {
            foreach (var ft in Copied)
            {
                PasteOne(ft);
                Copied.Remove(ft);
            }
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <returns></returns>
        public static FileType Rename()
        {
            return FileType.NullPointer;
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="ft"></param>
        private void DeleteSingle(FileType ft)
        {
            if (ft.IsDir)
            {
                ManagerWriter.DeleteDir(ft);
                DirectoryPointer.Remove(ft.Path);
            }
            else
            {
                ManagerWriter.Delete(ft);
                DirectoryPointer.Remove(ft.Path);
            }
        }
        
        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="ftList"></param>
        public void DeleteSelected(List<FileType> ftList)
        {
            foreach (var ft in ftList) DeleteSingle(ft);
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected FileType Search(string name)
        {
            return FileType.NullPointer;
            //return ManagerReader.SearchByUndeterminedName(directory, name);
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="ft"></param>
        public void ChangeDirectory(FileType ft)
        {
            var val = ft.Path;
            ChangeDirectory(val);
        }

        /// <summary>
        ///     - Action : Change the directory, load items, load currentPath<br></br>
        ///     - XAML : Modify the Path in the <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="path"></param>
        public void ChangeDirectory(string path)
        {
            // Changing directory
            try
            {
                // Modify the directory
                DirectoryPointer.ChangeDirectory(path);
                DirectoryPointer.Path = path;
            }
            catch (ManagerException e)
            {
                //ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }

            // Setting up the Path for CubeTools UI
            CurrentPath = path;
            // Modified ListBox associated
            Items = ManagerReader.ListToObservable(DirectoryPointer.ChildrenFiles);
            // Adding path to queue
            QueuePointers.Add(path);
            if (QueuePointers.Count >= 9)
            {
                QueuePointers.RemoveAt(0);
                QueueIndex--;
            }

            Selected = new List<FileType>();
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void Sort()
        {
            ManagerReader.SortByName(DirectoryPointer.ChildrenFiles);
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="path"></param>
        public void Open(string path)
        {
            ManagerReader.GetFileExtension(path); // Get the extension and read it
            // TODO Add open function
        }

        #endregion
    }
}