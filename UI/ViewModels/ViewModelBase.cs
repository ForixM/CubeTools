using System;
using System.Collections.Generic;
using System.IO;
using DynamicData;
using Manager;
using Manager.ManagerExceptions;
using ReactiveUI;

namespace UI.ViewModels
{
    public class ViewModelBase : ReactiveObject
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
        
        #endregion

        #region Path
        
        // Path
        public string home;
        public string desktop;
        public string document;
        public string download;
        public string picture;
        public string trash;
        
        #endregion

        #region Init
        public ViewModelBase()
        {
            // Variables
            _selected = new List<FileType>();
            _copied = new List<FileType>();
            _queue = new List<string>();
            _indexQueue = 0;
            // Path
            home = "C:/";
            desktop = "C:/";
            document = "C:/";
            download = "C:/";
            picture = "C:/";
            trash = "C:/";
            
            try
            {
                _directory = new DirectoryType();
            }
            catch (Exception e) //TODO Find a solution for cross platform - Add exception
            {
                return;
            }
            finally
            {
                _queue.Add(_directory.Path);
            }
        }
        
        #endregion
        
        #region View
        // All functions are called in XAML code
        
        // Left Arrow button is clicked
        public void LeftBtnClick()
        {
            if (_indexQueue > 0)
                _indexQueue--;
            ChangeDirectory(_queue[_indexQueue]);
        }

        // Right Arrow button is clicked
        public void RightBtnClick()
        {
            if (_indexQueue < _queue.Count-1)
                _indexQueue++;
            ChangeDirectory(_queue[_indexQueue]);
        }

        // Up Arrow clicked : GetParent
        public void UpBtnClick()
        {
            string res = "";
            try
            {
                res = ManagerReader.GetParent(_directory.Path);
            }
            catch (Exception e)
            {
                return;
            }
            ChangeDirectory(res);
            _queue.Add(res);
            _indexQueue++;
        }

        public void SyncBtnClick()
        {
            // TODO Add OneDrive implementation
        }

        public void SettingBtnClick()
        {
            // TODO display parameter
        }

        // Create button
        public void CreateBtnClick()
        {
            
        }

        // Copy button
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
            Copy(_copied);
        }

        public void RenameBtnClick()
        {

        }

        public void DeleteBtnClick()
        {
            DeleteSelected(_selected);
        }

        public void NearbySendBtnClick()
        {
            
        }

        public void SearchBtnClick()
        {
            // TODO Display the Pointer
        }

        public void GoToHomeBtnClick()
        {
            ChangeDirectory();
        }

        public void GoToDesktopBtnClick()
        {

        }

        public void GoToDocumentsBtnClick()
        {

        }

        public void GoToDownloadBtnClick()
        {

        }

        public void GoToPictureBtnClick()
        {

        }

        public void GoToMusicBtnClick()
        {

        }

        public void GoToTrashBtnClick()
        {

        }

        public void GoToSubFile()
        {
            
        }
        #endregion
        
        #region Methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static void Create(string name)
        {
            ManagerWriter.Create(name);
            
        }
        
        private static bool Copy(FileType ft, bool clear = false)
        {
            if (File.Exists(ft.Path) || Directory.Exists(ft.Path))
            {
                if (clear)
                    _copied.Clear();
                _copied.Add(ft);
                return true;
            }
            return false;
        }

        private static void Copy(List<FileType> fts, bool clear = true)
        {
            if (clear)
                _copied.Clear();
            foreach (var ft in fts)
            {
                if (!Copy(ft, false))
                {
                    _selected.Clear();
                    throw new PathNotFoundException("The selected file :" + ft.Name + " does not exist anymore");
                }
            }
        }

        private static void Cut(FileType ft)
        {
            
        }

        private static void Cut(List<FileType> fts)
        {
            foreach (var ft in fts)
            {
                Cut(ft);
            }
        }

        private static void Paste(FileType ft)
        {
            try
            {
                try
                {
                    ManagerWriter.Copy(ref _directory, ft, ft.Path, false);
                }
                catch (ReplaceException)
                {
                    try
                    {
                        ManagerWriter.Copy(ref _directory, ft, ft.Path, true);
                    }
                    catch (AccessException)
                    {
                        // TODO Implement access exception for copy
                    }
                    catch (SystemException)
                    {
                        // TODO Implement system exception for copy
                    }
                    catch (Exception)
                    {
                        // TODO Implement Unknown exception for copy
                    }
                }
            }
            catch (AccessException)
            {
            }
            catch (CorruptedDirectoryException)
            {

            }
            catch (CorruptedPointerException)
            {
                
            }
            catch (Exception)
            {
            }
        }

        private static void Paste()
        {
            foreach (var ft in _copied)
            {
                Paste(ft);
                _copied.Remove(ft);
            }
        }

        private static FileType Rename()
        {
            return FileType.NullPointer;
        }

        private static void DeleteSelected(List<FileType> ftList)
        {
            foreach (var ft in ftList)
            {
                DeleteSingle(ft);
            }
        }

        private static void DeleteSingle(FileType ft)
        {
            if (ft.IsDir)
            {
                try
                {
                    ManagerWriter.Delete(ref _directory, ft);
                }
                catch (Exception e)
                {
                    return;
                }
            }
            else
            {
                try
                {
                    ManagerWriter.DeleteDir(ft, true);
                }
                catch (Exception e)
                {
                    return;
                }
            }
        }

        private static FileType Search(string name)
        {
            return ManagerReader.SearchByIndeterminedName(directory, name);
        }

        private static void ChangeDirectory(FileType ft)
        {
            try
            {
                _directory.ChangeDirectory(ft.Path);
            }
            catch (Exception e)
            {
                return;
            }
        }
        
        private static void ChangeDirectory(string path)
        {
            try
            {
                _directory.ChangeDirectory(path);
            }
            catch (Exception e)
            {
                return;
            }
        }

        private static void Sort()
        {
            ManagerReader.SortByName(_directory.ChildrenFiles);
        }

        private static void Refresh()
        {
            try
            {
                _directory.SetChildrenFiles();
            }
            catch (Exception e)
            {
                return;
            }
        }
        
        #endregion
    }
}