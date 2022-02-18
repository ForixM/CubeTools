using System;
using System.Collections.Generic;
using System.IO;
using Manager.ManagerExceptions;

namespace Manager
{
    public class ManagerUI
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
        
        // Getter
        public static DirectoryType directory => _directory;
        public static List<FileType> selected => _selected;
        public static List<FileType> copied => _copied;
        
        #endregion
        
        #region Init
        // This region contains every methods that will help us to generate the attributes of the class

        /// <summary>
        /// Basic Constructor : Generate variables, SetToCurrentDirectory
        /// </summary>
        public ManagerUI()
        {
            string path = "";
            try
            {
                path = Directory.GetCurrentDirectory();
            }
            catch (UnauthorizedAccessException)//TODO Find a solution for cross platform - Add exception
            {
            } 
            finally
            {
                _directory = new DirectoryType(path);
                _selected = new List<FileType>();
                _copied = new List<FileType>();
                // TODO Add all variables missing
            }
        }

        /// <summary>
        /// Constructor with directory given in parameter.
        /// </summary>
        /// <param name="ft">the directory pointer given to generate an instance</param>
        public ManagerUI(FileType ft)
        {
            if (ft.IsDir && Directory.Exists(ft.Path))
            {
                _directory = new DirectoryType(ft.Path);
                _selected = new List<FileType>();
                _copied = new List<FileType>();
                // TODO Add all variables missing
            }
            else
            {
                string path = "";
                try
                {
                    path = Directory.GetCurrentDirectory();
                }
                catch (UnauthorizedAccessException) //TODO Find a solution for cross platform - Add exception
                {
                }
                finally
                {
                    _directory = new DirectoryType(path);
                    _selected = new List<FileType>();
                    _copied = new List<FileType>();
                    // TODO Add all variables missing
                }
            }
        } 
        
        #endregion
        
        #region UI
        
        public static FileType Create(FileType ft)
        {
            return FileType.NullPointer;
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

        public static void Copy(List<FileType> fts, bool clear = true)
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

        public static void Cut(List<FileType> fts)
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
                ManagerWriter.Copy(ft, _directory.Path, false);
            }
            catch (AccessException)
            {
            }
            catch (UnknownException)
            {
            }
        }

        public static void Paste()
        {
            foreach (var ft in _copied)
            {
                Paste(ft);
                _copied.Remove(ft);
            }
        }

        public static FileType Rename()
        {
            return FileType.NullPointer;
        }

        public static void Delete()
        {
            
        }

        public static FileType Search()
        {
            return FileType.NullPointer;
        }

        public static void ChangeDirectory()
        {
            
        }

        public static void Sort()
        {
            
        }

        public static void Refresh()
        {
            
        }
        
        #endregion
    }
}