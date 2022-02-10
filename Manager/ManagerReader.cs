using System;
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
        // IsAReadOnlyFile : Verify if the file has the property Read Only
        // Implementation : Check
        public static bool IsAReadOnlyFile(string path)
        {
            return (File.Exists(path) && (File.GetAttributes(path) & FileAttributes.ReadOnly) != 0);
        }
        // IsDirReadOnly : Verify if the dir has the property Read Only
        // Implementation : Check
        public static bool IsDirReadOnly(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                return ((directoryInfo.Attributes & FileAttributes.ReadOnly) != 0);
            }

            return false;
        }

        #endregion

        #region Get

        // This region contains all Get function that also give information of files and directories

        /// <summary>
        /// Overload 1
        ///  - Action Get the dir name of a file or dir contained in it <br></br>
        ///  - Implementation : Check
        /// </summary>
        /// <param name="path">the filename or directory name</param>
        /// <returns>Returns the parent dir using GetPathToName function</returns>
        public static string GetParent(string path)
        {
            if (File.Exists(path))
                return GetPathToName(new FileInfo(path).DirectoryName);
            if (Directory.Exists(path))
            {
                var directoryInfo = new DirectoryInfo(path).Parent;
                if (directoryInfo != null)
                    return GetPathToName(directoryInfo.FullName);
            }
            return "";
        }

        /// <summary>
        /// Overload 2 : see 
        ///  <see cref="GetParent(string)"/>
        /// </summary>
        /// <param name="path">the filename or directory name</param>
        /// <returns>Returns the parent dir using GetPathToName function</returns>
        public static string GetParent(FileType ft)
        {
            return GetParent(ft.Path);
        }

        /// <summary>
        ///  - Action : Get the date with a given path <br></br>
        ///  - Implementation : Check <br></br>
        /// </summary>
        /// <returns>Returns the creation date either of a file or a directory</returns>
        public static string GetFileCreationDate(string path)
        {
            if (Directory.Exists(path))
                return Directory.GetCreationTime(path).ToString();
            if (File.Exists(path))
                return File.GetCreationTime(path).ToString();
            return "";
        }

        /// <summary>
        /// - Action : Get the last time a file has been edited using its path <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <returns>A string value that represents the last time the file or directory was modified</returns>
        public static string GetFileLastEdition(string path)
        {
            if (Directory.Exists(path))
                return Directory.GetLastWriteTime(path).ToString();
            else if (File.Exists(path))
                return File.GetLastWriteTime(path).ToString();
            return "";
        }

        /// <summary>
        /// Get the last time a file has been accessed using its path <br></br>
        /// Implementation : Check
        /// </summary>
        /// <returns>A string value that represents the last time the file or directory was accessed</returns>
        public static string GetFileAccessDate(string path)
        {
            if (Directory.Exists(path))
                return Directory.GetLastAccessTime(path).ToString();
            if (File.Exists(path))
                return File.GetLastAccessTime(path).ToString();
            return "";
        }

        /// <summary>
        /// - Action : Get the file size in byte <br></br>
        /// - Implementation : Check (prototype)
        /// </summary>
        /// <returns>0 or the size of the file</returns>
        public static long GetFileSize(string path)
        {
            if (Directory.Exists(path))
                return new DirectoryInfo(path).EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
            else if (File.Exists(path))
                return new FileInfo(path).Length;
            return 0;
        }

        /// <summary>
        /// -Action : Get the file size in byte using recursive method <br></br>
        /// - Implementation : Recursion not efficient => DEPRECATED
        /// </summary>
        /// <returns>0 or the size of the file</returns>
        private static long GetFileSizeDeprecated(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                long size = 0;
                foreach (var fi in di.GetFiles())
                    size += fi.Length;
                foreach (var dic in di.GetDirectories())
                    size += GetFileSizeDeprecated(dic.FullName);
                return size;
            }
            else if (File.Exists(path))
                return new FileInfo(path).Length;

            return 0;
        }

        /// <summary>
        /// - Action : Reformat an absolute path to the name of the file or dir <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <returns>A string that represents the name of an absolute path</returns>
        public static string GetPathToName(string path)
        {
            if (File.Exists(path) || Directory.Exists(path))
                return Path.GetFileName(path);
            return "";
        }

        /// <summary>
        /// -Action : Same as GetPathToName but does not give the extension of the file <br></br>
        /// Implementation : Check
        /// </summary>
        /// <returns>A string that represents the name of the file without its extension</returns>
        public static string GetPathToNameNoExtension(string path)
        {
            if (File.Exists(path) || Directory.Exists(path))
                return Path.GetFileNameWithoutExtension(path);
            return "";
        }

        /// <summary>
        /// - Action : Reformat a name to the absolute path if the given name and current directory are correct <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <returns>The full path of a file or directory</returns>
        public static string GetNameToPath(string name)
        {
            if (File.Exists(name) || Directory.Exists(name))
            {
                string fullPath = Path.GetFullPath(name);
                fullPath = fullPath.Replace("\\", "/");
                return fullPath;
            }
            return name;
        }

        /// <summary>
        /// - Action : Returns the extension of a file. If it is a directory, it will return "" <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <returns>Extension of a file</returns>
        public static string GetFileExtension(string path)
        {
            return Path.GetExtension(path);
        }

        /// <summary>
        /// - Action : returns the file name of a absolute path using Path class
        /// - Implementation : NOT Check
        /// </summary>
        /// <param name="full_path">full_path is supposed to exist</param>
        /// <returns>the name of the file with its extension</returns>
        public static string GetFileNameWithExtension(string full_path, string new_extension = "")
        {
            if (new_extension == "")
                return Path.GetFileName(Path.GetFullPath(full_path));
            else
                return $"{Path.GetFileNameWithoutExtension(Path.GetFullPath(full_path))}{new_extension}";
        }

        #endregion

        #region Saver

        // In this section, every function is related to the save of file and the creation of FileType
        // instance to simplify the interaction with UI

        /// <summary>
        /// Reads the properties of a File, modifies and inits its associated FileType <br></br>
        /// Implementation : Check
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileType ReadFileType(string path)
        {
            if (File.Exists(path) || Directory.Exists(path))
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
        /// Implemenation : Check 
        /// </summary>
        /// <param name="ft"></param>
        public static void ReadFileType(ref FileType ft)
        {
            if (Directory.Exists(ft.Path))
            {
                ft.Name = GetPathToName(ft.Path);
                ft.ReadOnly = false;
                ft.Hidden = ((File.GetAttributes(ft.Path) & FileAttributes.Hidden) == FileAttributes.Hidden);
                // Add ft.Archived
                ft.Size = GetFileSize(ft.Path);
                ft.Date = GetFileCreationDate(ft.Path);
                ft.LastDate = GetFileLastEdition(ft.Path);
                ft.AccessDate = GetFileAccessDate(ft.Path);
                ft.Type = "folder";
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

        #region Basics

        // This region contains every algorithm used for basic treatment

        /// <summary>
        /// This function takes a path and generate a new path to avoid overwritte an existing file <br></br>
        /// Implementation : Check
        /// </summary>
        /// <returns>Generate a file name to create a copy</returns>
        public static string GenerateNameForModification(string path)
        {
            int i = 0;
            string res = path;
            string extension = GetFileExtension(path);
            string name = GetPathToNameNoExtension(path);

            while (File.Exists(res) || Directory.Exists(res))
            {
                i += 1;
                res = $"{name}({i}){extension}";
            }
            return res;
        }

        /// <summary>
        /// Comapres 2 strings and returns true if the first string is greater than the other one
        /// Implementation : Check
        /// usage : Sort Algorithm
        /// </summary>
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
                if (s2[j] > s1[i])
                    return false;
                i += 1;
                j += 1;
            }

            return i != limit1;
        }

        /// <summary>
        /// - Action: Verifies if the fist given date is greater than the second one <br></br>
        /// - FORMAT: MONT/DAY/YEAR HOUR/MIN/SEC PM/AM <br></br>
        /// Implementation : Check
        /// </summary>
        /// <returns>Returns if the first one is more recent than the other</returns>
        public static bool MoreRecentThanDate(string date1, string date2)
        {
            string[] date1List = date1.Split(' '); // [1/28/2022, 12:17:56, PM]
            string[] date2List = date2.Split(' ');
            if (date1List.Length != 3 || date1List.Length != 3)
                return false;

            string[] day1 = date1List[0].Split('/');
            string[] day2 = date2List[0].Split('/');
            string[] hour1 = date1List[1].Split(':');
            string[] hour2 = date2List[1].Split(':');

            if (day1.Length != 3 || day2.Length != 3)
                return false;

            // Compare dates
            for (int i = day1.Length - 1; i >= 0; i--)
            {
                if (int.Parse(day1[i]) > int.Parse(day2[i]))
                    return true;
                else if (int.Parse(day1[i]) < int.Parse(day2[i]))
                    return false;
            }

            // Compares Hours format
            string hourFormat = DateTime.Now.ToString().Split(' ')[2];
            if (hourFormat == date1List[2] && hourFormat != date2List[2])
                return true;
            else if (hourFormat != date1List[2] && hourFormat == date2List[2])
                return false;

            // Comapares Hours
            string[] hour = DateTime.Now.ToString().Split(' ')[1].Split(':');
            for (int i = hour.Length - 1; i >= 0; i--)
            {
                if (int.Parse(hour1[i]) > int.Parse(hour2[i]))
                    return true;
                if (int.Parse(hour1[i]) < int.Parse(hour2[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Select FileTypes in a FileType list using a minimum size for their names
        /// Implementation : Check
        /// </summary>
        private static List<FileType> SelectFileTypeByNameSize(List<FileType> fileTypes, int minimumNameSize)
        {
            List<FileType> res = new List<FileType>();
            foreach (FileType ft in fileTypes)
            {
                if (ft.Name.Count() >= minimumNameSize)
                    res.Add(ft);
            }

            return res;
        }

        #endregion

        #region Calculus

        // This region contains every function that deal with algorithm for calculus

        /// <summary>
        /// Get the size and returns its value with a mroe revelant format
        /// Implementation : Check
        /// </summary>
        /// <returns>Returns string for display</returns>
        public static string ByteToPowByte(long size)
        {
            if (size < 1024)
                return $"{size} B";
            if (size < 1048576)
                return $"{size / 1024} KB";
            if (size < 1073741824)
                return $"{size / 1048576} MB";
            return $"{size / 1073741824} GB";
        }

        #endregion

        #region Sort

        // SORT BY STRINGS

        /// <summary>
        /// Create a new sorted list using merge algorithm
        /// This functions takes two FileType list sorted them using their NAMES and returns a new one sorted with all the elements
        /// Implementation : Check
        /// </summary>
        private static List<FileType> MergeSortFileTypeByName(List<FileType> ftList1, List<FileType> ftList2)
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
        /// Create a new sorted list using merge algorithm
        /// This functions takes two FileType list sorted them using their SIZES and returns a new one sorted with all the elements
        /// Implementation : Check
        /// </summary>
        /// <returns></returns>
        private static List<FileType> MergeSortFileTypeBySize(List<FileType> ftList1, List<FileType> ftList2)
        {
            List<FileType> ftReturned = new List<FileType>();

            int i = 0;
            int j = 0;
            int limit1 = ftList1.Count;
            int limit2 = ftList2.Count;

            while (i < limit1 && j < limit2)
            {
                if (ftList1[i].Size > ftList2[j].Size)
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
        /// Create a new sorted list using merge algorithm
        /// This functions takes two FileType list sorted them using their TYPES and returns a new one sorted with all the elements
        /// Implementation : Check
        /// </summary>
        /// <returns></returns>
        private static List<FileType> MergeSortFileTypeByType(List<FileType> ftList1, List<FileType> ftList2)
        {
            List<FileType> ftReturned = new List<FileType>();

            int i = 0;
            int j = 0;
            int limit1 = ftList1.Count;
            int limit2 = ftList2.Count;

            while (i < limit1 && j < limit2)
            {
                if (GreaterThan(ftList1[i].Type, ftList2[j].Type))
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
        /// Create a new sorted list using merge algorithm
        /// This functions takes two FileType list sorted them using their MODIFIED DATES and returns a new one sorted with all the elements
        /// Implementation : NOT Checks
        /// </summary>
        private static List<FileType> MergeSortFileTypeByModifiedDate(List<FileType> ftList1, List<FileType> ftList2)
        {
            List<FileType> ftReturned = new List<FileType>();

            int i = 0;
            int j = 0;
            int limit1 = ftList1.Count;
            int limit2 = ftList2.Count;

            while (i < limit1 && j < limit2)
            {
                if (!MoreRecentThanDate(ftList1[i].Name, ftList2[j].Name))
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

        // Sort functions

        /// <summary>
        /// MergeSort algorithm to sort files (by names)
        /// Implementation : Check
        /// </summary>
        /// <param name="ftList"></param>
        /// <returns>Returns the sorted list of filetype</returns>
        public static List<FileType> SortByName(List<FileType> ftList)
        {
            return DivideAndMergeAlgorithm(ftList, "name");
        }

        /// <summary>
        /// MergeSort algorithm to sort files (by sizes)
        /// Implementation : Check
        /// </summary>
        /// <param name="ftList"></param>
        /// <returns>Returns the sorted list of filetype</returns>
        public static List<FileType> SortBySize(List<FileType> ftList)
        {
            return DivideAndMergeAlgorithm(ftList, "size");
        }

        /// <summary>
        /// MergeSort algorithm to sort files (by types)
        /// Implementation : Check
        /// </summary>
        /// <param name="ftList"></param>
        /// <returns>Returns the sorted list of filetype</returns>
        public static List<FileType> SortByType(List<FileType> ftList)
        {
            return DivideAndMergeAlgorithm(ftList, "type");
        }

        /// <summary>
        /// Sort a FileType list using the last modified date parameter
        /// </summary>
        public static List<FileType> SortByModifiedDate(List<FileType> ftList)
        {
            return DivideAndMergeAlgorithm(ftList, "date");
        }

        // Main functions

        /// <summary>
        /// Main algorithm : recursive method that divides and merge lists
        /// This functions is used for sort algorithm because it is the most efficient algorithm for string treatment
        /// Implementation : Check
        /// </summary>
        /// <returns>Returns the sorted </returns>
        private static List<FileType> DivideAndMergeAlgorithm(List<FileType> ftList, string argument)
        {
            // If list is empty, returns it
            if (ftList.Count() <= 1)
            {
                return ftList;
            }

            // If not empty divide them and call the function again
            List<FileType> ftList1 = new List<FileType>();
            List<FileType> ftList2 = new List<FileType>();
            for (int i = 0; i < ftList.Count / 2; i++)
                ftList1.Add(ftList[i]);
            for (int i = ftList.Count / 2; i < ftList.Count(); i++)
                ftList2.Add(ftList[i]);
            return MergeSortFileType(DivideAndMergeAlgorithm(ftList1, argument),
                DivideAndMergeAlgorithm(ftList2, argument), argument);
        }

        /// <summary>
        /// Sort fileType list using the merge sort algorithm
        /// The string argument gets the wanted value to be sort
        /// </summary>
        /// <returns></returns>
        private static List<FileType> MergeSortFileType(List<FileType> ftList1, List<FileType> ftList2, string argument)
        {
            switch (argument)
            {
                case "size":
                    return MergeSortFileTypeBySize(ftList1, ftList2);
                case "type":
                    return MergeSortFileTypeByType(ftList1, ftList2);
                case "name":
                    return MergeSortFileTypeByName(ftList1, ftList2);
                case "date":
                    return MergeSortFileTypeByModifiedDate(ftList1, ftList2);
                default:
                    return null;
            }
        }

        #endregion

        #region Search

        /// <summary>
        /// Naive research of a fileType
        /// Implementation : Check
        /// </summary>
        /// <returns>return the fileType that has to be find</returns>
        public static FileType SearchByFullName(List<FileType> fileTypes, string fullName)
        {
            foreach (FileType ft in fileTypes)
            {
                if (ft.Name == fullName)
                    return ft;
            }

            return null;
        }

        public static FileType SearchByFullName(DirectoryType directoryType, string fullName)
        {
            return SearchByFullName(directoryType.ChildrenFiles, fullName);
        }

        /// <summary>
        /// Naive research of a fileType using an indeterminatedName which will get the most revelant file
        /// Implemenation : Check
        /// </summary>
        /// <returns>return the fileType that has to be find</returns>
        public static FileType SearchByIndeterminedName(List<FileType> fileTypes, string indeterminedName)
        {
            if (fileTypes == null)
                return null;
            FileType bestFitft = fileTypes[0];
            int _max_occ = bestFitft.Name.Length;
            List<FileType> _list = SelectFileTypeByNameSize(fileTypes, indeterminedName.Length);
            foreach (FileType ft in _list)
            {
                if (ft.Name == indeterminedName)
                    return ft;
                else
                {
                    int _current_occ = 0;
                    while (_current_occ < indeterminedName.Length)
                    {
                        if (ft.Name[_current_occ] == indeterminedName[_current_occ])
                            _current_occ++;
                        else
                            break;
                    }

                    if (_current_occ == indeterminedName.Length)
                        return ft;
                    else if (_current_occ > _max_occ)
                    {
                        bestFitft = ft;
                        _max_occ = _current_occ;
                    }
                }
            }
            return bestFitft;
        }
        
        public static FileType SearchByIndeterminedName(DirectoryType directoryType, string fullName)
        {
            return SearchByIndeterminedName(directoryType.ChildrenFiles, fullName);
        }

        #endregion

        #endregion

        #region CommandLine

        // Implementation : Check
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