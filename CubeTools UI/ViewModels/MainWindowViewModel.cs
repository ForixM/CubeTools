// System's imports

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using Library.Pointers;
using MessageBox.Avalonia;
using ReactiveUI;
// Library's imports
// CubeTools UI's imports


namespace CubeTools_UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Init

        public MainWindowViewModel()
        {
            // Variables
            selected = new List<FileType>();
            copied = new List<FileType>();
            queue = new List<string>();
            indexQueue = 0;
            // TODO Implement algorithm to detect static path
            home = "C:/Users/mateo";
            desktop = "C:/Users/mateo/Desktop";
            document = "C:/Users/mateo/OneDrive";
            download = "C:/Users/mateo/Downloads";
            picture = "C:/Users/mateo/Pictures";
            trash = "C:/$Recycle.Bin";

            try
            {
                _directory = new DirectoryType(Directory.GetCurrentDirectory());
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
            finally
            {
                queue.Add(_directory.Path);
                CurrentPath = _directory.Path;
            }

            items = new ObservableCollection<FileType>();
            foreach (var i in directory.ChildrenFiles) items.Add(i);
        }

        #endregion

        #region ViewMethods

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void ErrorMessageBox(string title, string level, string type, string message)
        {
            var content =
                $"Level : {level}\n" +
                $"Type : {type}\n\n" +
                $"Message : {message}\n";
            var messageBoxStandardWindow = MessageBoxManager.GetMessageBoxStandardWindow(title, content);
            messageBoxStandardWindow.Show();
        }

        #endregion

        #region OtherMethods

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T">The given type</typeparam>
        /// <returns></returns>
        private static ObservableCollection<T> ListToObservable<T>(List<T> list)
        {
            var res = new ObservableCollection<T>();
            foreach (var e in list) res.Add(e);

            return res;
        }

        #endregion

        #region Variables

        // This region contains all needed variables for CubeTools UI purpose

        #region BasicVariables

        // This region contains all variable needed for treatment of pointers and so forth
        // PRIVATE
        // A pointer to the current loaded Directory
        // Consider using it most of the time because used in CLI version
        private static DirectoryType _directory;

        // Pointers to specific variables
        // Give all the current selected FT

        // Give all the copied FT

        // Queue of path that has been loaded (no more than 8 path, better for memory)

        // Index Queue : the current index of the queue

        // PUBLIC : USING GETTERS
        public static DirectoryType directory => _directory;
        public static List<FileType> selected { get; private set; }

        public static List<FileType> copied { get; private set; }

        public static List<string> queue { get; private set; }

        public static int indexQueue { get; private set; }

        #endregion

        #region StaticPathVariable

        // Static Path
        private readonly string home;
        private readonly string desktop;
        private readonly string document;
        private readonly string download;
        private readonly string picture;
        private readonly string trash;

        #endregion

        #region BindingVariables

        // This region contains all BindingVariables : XAML Code purpose

        // The files and folders loaded in XAML code
        // items stores the values
        private ObservableCollection<FileType> items;

        // Items help to coordinate every thing
        // 
        public ObservableCollection<FileType> Items
        {
            get => ListToObservable(_directory.ChildrenFiles);
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
            get => _directory.Path;
            set
            {
                // If the given value exists, deal with it
                if (Directory.Exists(value))
                {
                    // If CurrentPath has been set by user, we have to change the directory
                    if (_directory.Path != value)
                        ChangeDirectory(value);
                    // Modify the value in XAML
                    var res = value;
                    this.RaiseAndSetIfChanged(ref res, value);
                }
            }
        }

        #endregion

        #endregion

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
            if (indexQueue <= 0)
                return;

            // Get the index before
            indexQueue--;
            try
            {
                // Change the directory
                ChangeDirectory(queue[indexQueue]);
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        /// <summary>
        ///     - Action : Get the next value stored after the current one in the queue.<br></br>
        ///     - XAML : Change the directory and reload page<br></br>
        ///     - Implementation : NOT CHECK
        /// </summary>
        public void RightBtnClick()
        {
            if (indexQueue >= queue.Count - 1)
                return;
            // Modifying selected items
            indexQueue++;
            try
            {
                // Getting value in the queue
                ChangeDirectory(queue[indexQueue]);
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        /// <summary>
        ///     - Action : Get the parent directory and set it in the env<br></br>
        ///     - XAML : Change the current directory to its parent<br></br>
        ///     - Implementation : NOT CHECK - //TODO Resolve Bugs
        /// </summary>
        public void UpBtnClick()
        {
            indexQueue++;
            try
            {
                ChangeDirectory(ManagerReader.GetParent(_directory.Path));
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
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
        public void CreateBtnClick()
        {
            try
            {
                Create("New File");
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void CopyBtnClick()
        {
            copied.Clear();
            foreach (var ft in selected) copied.Add(ft);
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void CutBtnClick()
        {
            // TODO Implement cut function
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void PasteBtnClick()
        {
            try
            {
                Copy(copied);
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void RenameBtnClick()
        {
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void DeleteBtnClick()
        {
            try
            {
                DeleteSelected(selected);
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void NearbySendBtnClick()
        {
            // TODO Add method for NearBySend
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void SearchBtnClick()
        {
            // TODO Display the Pointer
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
                ChangeDirectory(home);
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
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
                ChangeDirectory(desktop);
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
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
                ChangeDirectory(document);
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
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
                ChangeDirectory(download);
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
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
                ChangeDirectory(picture);
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
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
                ChangeDirectory(trash);
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
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
                if (selected[0].IsDir)
                    ChangeDirectory(selected[0]);
                else
                    Open(selected[0].Path);
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        #endregion

        #region ProcessMethods

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected static void Create(string name)
        {
            name = ManagerReader.GetNameToPath(name);
            ManagerWriter.Create(name);
            _directory.ChildrenFiles.Add(ManagerReader.ReadFileType(name));
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="ft"></param>
        /// <returns></returns>
        protected static bool Copy(FileType ft)
        {
            if (File.Exists(ft.Path) || Directory.Exists(ft.Path))
            {
                copied.Add(ft);
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
        protected static void Copy(List<FileType> fts)
        {
            copied.Clear();
            foreach (var ft in fts)
                if (!Copy(ft))
                {
                    selected.Clear();
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
        protected static void Cut(FileType ft)
        {
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="fts"></param>
        protected static void Cut(List<FileType> fts)
        {
            foreach (var ft in fts) Cut(ft);
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="ft"></param>
        protected static void PasteOne(FileType ft)
        {
            ManagerWriter.Copy(ref _directory, ft, ft.Path);
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        protected static void PasteCopied()
        {
            foreach (var ft in copied)
            {
                PasteOne(ft);
                copied.Remove(ft);
            }
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <returns></returns>
        protected static FileType Rename()
        {
            return FileType.NullPointer;
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="ftList"></param>
        protected static void DeleteSelected(List<FileType> ftList)
        {
            foreach (var ft in ftList) DeleteSingle(ft);
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="ft"></param>
        protected static void DeleteSingle(FileType ft)
        {
            if (ft.IsDir)
            {
                ManagerWriter.DeleteDir(ft);
                _directory.Remove(ft.Path);
            }
            else
            {
                ManagerWriter.Delete(ft);
                _directory.Remove(ft.Path);
            }
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected static FileType Search(string name)
        {
            return ManagerReader.SearchByIndeterminedName(directory, name);
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="ft"></param>
        protected void ChangeDirectory(FileType ft)
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
        private void ChangeDirectory(string path)
        {
            // Changing directory
            try
            {
                // Modify the directory
                _directory.ChangeDirectory(path);
                _directory.Path = path;
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }

            // Setting up the Path for CubeTools UI
            CurrentPath = path;
            // Modified ListBox associated
            Items = ListToObservable(_directory.ChildrenFiles);
            // Adding path to queue
            queue.Add(path);
            if (queue.Count >= 9)
            {
                queue.RemoveAt(0);
                indexQueue--;
            }

            selected = new List<FileType>();
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        protected static void Sort()
        {
            ManagerReader.SortByName(_directory.ChildrenFiles);
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="path"></param>
        protected static void Open(string path)
        {
            ManagerReader.GetFileExtension(path); // Get the extension and read it
            // TODO Add open function
        }

        #endregion
    }
}