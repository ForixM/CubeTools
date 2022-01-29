﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Manager
{
    public static class ManagerReader
    {
        #region Properties
        // This region contains every function that give information of the file given with the path
        // Basicaly => bool functions

        // IsFunction :
        // Implementation : Check
        public static bool IsFile(string path)
        {
            return (File.Exists(path)) && (!Directory.Exists(path));
        }

        // IsDirectory : Returns if the given path is a directory
        // Implementation : Check
        public static bool IsDirectory(string path)
        {
            return Directory.Exists(path);
        }
        // IsFileHidden : Verify if the file has the property Hidden
        // Implementation : Check
        public static bool IsFileHidden(string path)
        {
            return (File.Exists(path) && (File.GetAttributes(path) & FileAttributes.Hidden) != 0);

        }
        // IsDirHidden : Verify if the directory is hidden
        // Implementation Check
        public static bool IsDirHidden(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                return ((directoryInfo.Attributes & FileAttributes.Hidden) != 0);
            }

            return false;
        }
        // IsFileCompressed : Verify if the file has the property Compressed
        // Implementation : Check
        public static bool IsFileCompressed(string path)
        {
            return (File.Exists(path) && (File.GetAttributes(path) & FileAttributes.Compressed) != 0);
        }
        // IsFileArchived : Verify if the file has the property Archived
        // Implementation : Check
        public static bool IsFileArchived(string path)
        {
            return (File.Exists(path) && (File.GetAttributes(path) & FileAttributes.Archive) != 0);
        }
        // IsASystemFile : Verify if the file has the property File System
        // Implementation : Check
        public static bool IsASystemFile(string path)
        {
            return (File.Exists(path) && (File.GetAttributes(path) & FileAttributes.System) != 0);
        }
        #endregion

        #region Get
        // This region contains all Get function that also give information of
        // files and also directories

        /// <summary>
        ///  Get the dir name of a file or dir contained in it
        ///  Implementation : Check
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Returns the parent dir using</returns>
        public static string GetParent(string path)
        {
            FileInfo fi = new FileInfo(path);
            if (fi.DirectoryName != null)
            {
                return GetPathToName(fi.DirectoryName);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        ///  Get the date with a given path
        ///  Implementation : Check
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Returns the creation date</returns>
        public static string GetFileCreationDate(string path)
        {
            if (Directory.Exists(path))
                return Directory.GetCreationTime(path).ToString();
            else if (File.Exists(path))
                return File.GetCreationTime(path).ToString();
            return "";
        }

        /// <summary>
        /// Get the last time a file has been edited using its path
        /// Implementation : Check
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A string time</returns>
        public static string GetFileLastEdition(string path)
        {
            if (Directory.Exists(path))
                return Directory.GetLastWriteTime(path).ToString();
            else if (File.Exists(path))
                return File.GetLastWriteTime(path).ToString();
            return "";
        }

        /// <summary>
        /// Get the last time a file has been accessed using its path
        /// Implementation : Check
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A string time</returns>
        public static string GetFileAccessDate(string path)
        {
            if (Directory.Exists(path))
                return Directory.GetLastAccessTime(path).ToString();
            else if (File.Exists(path))
                return File.GetLastAccessTime(path).ToString();
            return "";
        }

        /// <summary>
        /// Get the file size in byte
        /// Implementation : Check
        /// </summary>
        /// <param name="path"></param>
        /// <returns>0 or the size of the file</returns>
        public static long GetFileSize(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                return di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
            }
            else if (File.Exists(path))
            {
                FileInfo fi = new FileInfo(path);
                return fi.Length;
            }
            return 0;
        }

        /// <summary>
        /// Reformat an absolute path to the name of the file or dir
        /// Implementation : Check
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A string that represents the name of an absolute path</returns>
        public static string GetPathToName(string path)
        {
            int i = path.Length - 1;
            string res = "";
            while (i >= 0 && path[i] != '/' && path[i] != '\\')
            {
                res = path[i] + res;
                i--;
            }
            return res;
        }

        /// <summary>
        /// Reformat a ame to the absolute path if the given path is correct
        /// Implementation : NOT Check
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetNameToPath(string name)
        {
            if (File.Exists(name))
                return (Path.GetFullPath(name));
            return name;
        }

        /// <summary>
        /// Basicaly returns the extension of a file
        /// Implementation : Check
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileExtension(string path)
        {
            return Path.GetExtension(path);
        }

        #endregion

        #region Saver
        // In this section, every function is related to the save of file and the creation of FileType
        // instance to simplify the interaction with UI

        /// <summary>
        /// Reads the properties of a File, modifies and inits its associated FileType
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileType ReadFileType(string path)
        {
            if (File.Exists(path))
            {
                FileType ft = new FileType(path);
                ReadFileType(ref ft);
                return ft;
            }
            else
            {
                FileType ft = new FileType();
                return ft;
            }
        }

        /// <summary>
        /// Update FileType passed by reference
        /// </summary>
        /// <param name="ft"></param>
        public static void ReadFileType(ref FileType ft)
        {
            if (Directory.Exists(ft.Path))
            {
                ft.Name = GetPathToName(ft.Path);
                ft.ReadOnly = false;
                ft.Hidden = ((File.GetAttributes(ft.Path) & FileAttributes.Hidden) == FileAttributes.Hidden);
                ft.Size = GetFileSize(ft.Path);
                ft.Date = GetFileCreationDate(ft.Path);
                ft.LastDate = GetFileLastEdition(ft.Path);
                ft.AccessDate = GetFileAccessDate(ft.Path);
                ft.Type = "Directory";
                ft.IsDir = true;
            }
            else if (File.Exists(ft.Path))
            {
                ft.Name = Path.GetFileName(ft.Path);
                ft.ReadOnly = ((File.GetAttributes(ft.Path) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);
                ft.Hidden = ((File.GetAttributes(ft.Path) & FileAttributes.Hidden) == FileAttributes.Hidden);
                ft.Size = GetFileSize(ft.Path);
                ft.Date = GetFileCreationDate(ft.Path);
                ft.LastDate = GetFileLastEdition(ft.Path);
                ft.Type = GetFileExtension(ft.Path);
                ft.IsDir = false;
            }
            else
                ft.Dispose();
        }

        #endregion

        #region Algorithm

        // Basic Algorithm for basic Treatment

        /// <summary>
        /// This function takes a path and generate a new path to avoid overwritte an existing file
        /// Implementation : NOT Check
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GenerateFileNameForCopy(string path)
        {
            int i = 0;
            string temp = path;
            while (File.Exists(temp))
            {
                i += 1;
                temp = path + $"({i})";
            }
            return temp;
        }

        /// <summary>
        /// This function takes a path and generate a new path to avoid overwritte an existing dir
        /// Implementation : NOT Check
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GenerateDirectoryNameForCopy(string path)
        {
            int i = 0;
            string temp = path;
            while (Directory.Exists(temp))
            {
                i += 1;
                temp = path + $"({i})";
            }
            return temp;
        }

        // Sort Algorithm

        /// <summary>
        /// Comapres 2 strings and returns true if the first string is greater than the other one
        /// Implemenation : NOT Check
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        private static bool GreaterThan(string s1, string s2)
        {
            int i = 0;
            int j = 0;
            int limit1 = s1.Count();
            int limit2 = s2.Count();
            while (i < limit1 && j < limit2)
            {
                if (s1[i] > s1[j])
                    return true;
                else if (s2[j] > s1[i])
                    return false;
                else
                {
                    i += 1;
                    j += 1;
                }
            }
            return i != limit1;
        }

        /// <summary>
        /// Create a new sorted list using merge algorithm
        /// This functions takes two FileType list sorted and returns a new one sorted with all the elements
        /// Implemenation : NOT Check
        /// </summary>
        /// <param name="ftList"></param>
        /// <returns></returns>
        private static List<FileType> MergeSortFileType(List<FileType> ftList1, List<FileType> ftList2)
        {
            List<FileType> ftReturned = new List<FileType>();
            
            int i = 0;
            int j = 0;
            int limit1 = ftList1.Count;
            int limit2 = ftList2.Count;

            while (i < limit1 && j < limit2)
            {
                if (GreaterThan(ftList1[i].Name, ftList2[j].Name))
                {
                    ftReturned.Add(ftList1[i]);
                    i += 1;
                }
                else
                {
                    ftReturned.Add(ftList2[j]);
                    j += 1;
                }
            }
            if (i != limit1)
            {
                while (i < limit1)
                {
                    ftReturned.Add(ftList1[i]);
                    i += 1;
                }
            }
            else
            {
                while (j < limit2)
                {
                    ftReturned.Add(ftList2[j]);
                    j += 1;
                }
            }
            
            return ftReturned;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ftList"></param>
        /// <returns></returns>
        public static List<FileType> SortByName(List<FileType> ftList)
        {
            return ftList;
        }

        #endregion

        #region CommandLine

        // Implemented Check
        public static void ReadContent(string path)
        {
            if (File.Exists(path))
            {
                using (StreamReader fs = new StreamReader(path))
                {
                    var input = fs.ReadLine();
                    while (input != null)
                    {
                        Console.WriteLine(input);
                        input = fs.ReadLine();
                    }
                }
            }
        }

        #endregion
    }

}