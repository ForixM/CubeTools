using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;
using Manager;
using Manager.ManagerExceptions;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.Markup.Xaml;

namespace UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        #region Variables
        // This region contains all needed variables for UI purpose
        // A pointer to the current directory of 
        private static DirectoryType _directory;
        // Pointers to specific variables
        // Give all the current selected FT
        private static List<FileType> _selected;
        // Give all the copied FT
        private static List<FileType> _copied;
        // Queue
        private static List<string> _queue;
        // Index Queue
        private static int _indexQueue;
        // Getter
        public static DirectoryType directory => _directory;
        public static List<FileType> selected => _selected;
        public static List<FileType> copied => _copied;
        public static List<string> queue => _queue;
        public static int indexQueue => _indexQueue;

        public ObservableCollection<FileType> Items {
            get
            {
                ObservableCollection<FileType> res = new ObservableCollection<FileType>();
                foreach (var ft in _directory.ChildrenFiles)
                {
                    ft.Icon = "./Assets/folder.ico";
                    res.Add(ft);
                }
                return res;
            }
            set
            {
                _directory.ChildrenFiles.Clear();
                foreach (var ft in value)
                {
                    _directory.ChildrenFiles.Add(ft);
                }
            }
        }

        #endregion

        #region Path

        public string CurrentPath
        {
            get { return _directory.Path; }
            set
            {
                if (Directory.Exists(value))
                {
                    string val = _directory.Path;
                    this.RaiseAndSetIfChanged(ref val, value);
                    ChangeDirectory(ref value);   
                }
            }
        }
        // Path
        public string home;
        public string desktop;
        public string document;
        public string download;
        public string picture;
        public string trash;
        
        #endregion

        #region Init
        public MainWindowViewModel()
        {
            // Variables
            _selected = new List<FileType>();
            _copied = new List<FileType>();
            _queue = new List<string>();
            _indexQueue = 0;
            // Static Path
            home = "C:/";
            desktop = "C:/";
            document = "C:/";
            download = "C:/";
            picture = "C:/";
            trash = "C:/";

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
                _queue.Add(_directory.Path);
                CurrentPath = _directory.Path;
            }
            
            foreach (var i in directory.ChildrenFiles)
            {
                Items.Add(i);
            }
            
            
        }
        
        #endregion
        
        #region View
        // All functions are called in XAML code
        
        // Left Arrow button is clicked
        public void LeftBtnClick()
        {
            if (_indexQueue <= 0)
                return;
            try
            {
                _indexQueue--;
                string val = _queue[_indexQueue];
                ChangeDirectory(ref val);
                _queue[_indexQueue] = val;
                ErrorMessageBox("Path", _directory.Path, "ha", "ha");
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        // Right Arrow button is clicked
        public void RightBtnClick()
        {
            if (_indexQueue >= _queue.Count - 1)
                return;

            try
            {
                _indexQueue++;
                string val = _queue[_indexQueue];
                ChangeDirectory(ref val);
                _queue[_indexQueue] = val;
                ErrorMessageBox("Path", _directory.Path, "ha", "ha");
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        // Up Arrow clicked : GetParent
        public void UpBtnClick()
        {
            string res = "";
            try
            {
                res = ManagerReader.GetParent(_directory.Path);
                _indexQueue++;
                ChangeDirectory(ref res);
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
            string temp = "";
            foreach (var str in _queue)
            {
                temp += str + "   ";
            }
            ErrorMessageBox("Path", _directory.Path,"ha",temp);
        }

        // SyncButton Clicked
        public void SyncBtnClick()
        {
            // TODO Add OneDrive implementation
        }

        // Settings is opened
        public void SettingBtnClick()
        {
        }

        // Create button : 
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

        // Copy button and add them to the list of copied
        public void CopyBtnClick()
        {
            _copied.Clear();
            foreach (var ft in _selected)
            {
                _copied.Add(ft);
            }
        }

        public void CutBtnClick()
        {
            // TODO Implement cut function
        }

        public void PasteBtnClick()
        {
            try
            {
                Copy(_copied);
                Refresh();
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        public void RenameBtnClick()
        {
            
        }

        public void DeleteBtnClick()
        {
            try
            {
                DeleteSelected(_selected);
                Refresh();
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        public void NearbySendBtnClick()
        {
            // TODO Add method for NearBySend
        }

        public void SearchBtnClick()
        {
            // TODO Display the Pointer
        }

        public void GoToHomeBtnClick()
        {
            try
            {
                ChangeDirectory(ref home);
                Refresh();
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        public void GoToDesktopBtnClick()
        {
            try
            {
                ChangeDirectory(ref desktop);
                Refresh();
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        public void GoToDocumentsBtnClick()
        {
            try
            {
                ChangeDirectory(ref document);
                Refresh();
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        public void GoToDownloadBtnClick()
        {
            try
            {
                ChangeDirectory(ref download);
                Refresh();
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        public void GoToPictureBtnClick()
        {
            try
            {
                ChangeDirectory(ref picture);
                Refresh();
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        public void GoToTrashBtnClick()
        {
            try
            {
                ChangeDirectory(ref trash);
                Refresh();
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }

        // When a file is clicked
        public void GoToSubFile()
        {
            try
            {
                if (_selected[0].IsDir)
                {
                    ChangeDirectory(_selected[0]);
                }
                else
                {
                    Open(_selected[0].Path);
                }

                Refresh();
            }
            catch (ManagerException e)
            {
                ErrorMessageBox(e.Errorstd, e.CriticalLevel, e.ErrorType, e.FinalMessage);
            }
        }
        #endregion
        
        #region ViewMethod
        public void ErrorMessageBox (string title, string level, string type, string message)
        {
            string content = 
                $"Level                |    {level}\n" +
                $"Type                |    {type}\n" +
                $"Message          |    {message}\n";
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(title, content);
            messageBoxStandardWindow.Show();
        }
        
        #endregion
        
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected static void Create(string name)
        {
            name = ManagerReader.GetNameToPath(name);
            ManagerWriter.Create(name);
            _directory.ChildrenFiles.Add(ManagerReader.ReadFileType(name));
        }

        protected static bool Copy(FileType ft)
        {
            if (File.Exists(ft.Path) || Directory.Exists(ft.Path))
            {
                _copied.Add(ft);
                return true;
            }
            return false;
        }

        protected static void Copy(List<FileType> fts)
        {
            _copied.Clear();
            foreach (var ft in fts)
            {
                if (!Copy(ft))
                {
                    _selected.Clear();
                    throw new PathNotFoundException("The selected file :" + ft.Name + " does not exist anymore", "Copy");
                }
            }
        }

        protected static void Cut(FileType ft)
        {
            
        }

        protected static void Cut(List<FileType> fts)
        {
            foreach (var ft in fts)
            {
                Cut(ft);
            }
        }

        protected static void PasteOne(FileType ft)
        {
            ManagerWriter.Copy(ref _directory, ft, ft.Path, false);
        }

        protected static void PasteCopied()
        {
            foreach (var ft in _copied)
            {
                PasteOne(ft);
                _copied.Remove(ft);
            }
        }

        protected static FileType Rename()
        {
            return FileType.NullPointer;
        }

        protected static void DeleteSelected(List<FileType> ftList)
        {
            foreach (var ft in ftList)
            {
                DeleteSingle(ft);
            }
        }

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

        protected static FileType Search(string name)
        {
            return ManagerReader.SearchByIndeterminedName(directory, name);
        }

        protected static void ChangeDirectory(FileType ft)
        {
            string val = ft.Path;
            ChangeDirectory(ref val);
        }
        
        protected static void ChangeDirectory(ref string path)
        {
            _directory.ChangeDirectory(path);
            _queue.Add(path);
            if (_queue.Count >= 9)
            {
                _queue.RemoveAt(0);
                _indexQueue--;
            }
            _selected = new List<FileType>();
        }

        protected static void Sort()
        {
            ManagerReader.SortByName(_directory.ChildrenFiles);
        }

        protected static void Refresh()
        {
            _directory.SetChildrenFiles();
        }

        protected static void Open(string path)
        {
            ManagerReader.GetFileExtension(path); // Get the extension and read it
            // TODO Add open function
        }
        
        #endregion
    }
}