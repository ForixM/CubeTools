using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using Manager.ManagerExceptions;

namespace Manager
{
    public static class ManagerReader
    {
        #region Properties

        // This region contains every function that give information of the file given with the path
        // Basicaly => bool functions

        /// <summary>
        /// - Action : Verify if the given path is file or not <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the path of the file</param>
        /// <returns>whether it is a file or not</returns>
        public static bool IsFile(string path)
        {
            return (File.Exists(path)) && (!Directory.Exists(path));
        }

        /// <summary>
        /// - Action : Verify if the given path is a directory or not <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the path to test</param>
        /// <returns>whether it is a directory or not</returns>
        public static bool IsDirectory(string path)
        {
            return (Directory.Exists(path) && !(File.Exists(path)));
        }

        /// <summary>
        /// - Action : Verify whether a file or a directory is hidden or not
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the given file or dir</param>
        /// <returns>hidden or not</returns>
        public static bool IsFileHidden(string path)
        {
            return HasAttribute(FileAttributes.Hidden, path);
        }


        /// <summary>
        /// - Action : Verify whether a file or directory is compressed
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the given file or dir</param>
        /// <returns>compressed or not</returns>
        public static bool IsFileCompressed(string path)
        {
            return HasAttribute(FileAttributes.Compressed, path);
        }

        /// <summary>
        /// - Action : Verify whether a file or a directory is archived
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the given file or dir</param>
        /// <returns>archived or not</returns>
        public static bool IsFileArchived(string path)
        {
            return HasAttribute(FileAttributes.Archive, path);
        }

        /// <summary>
        /// - Action : Verify whether a file or directory is part of a the system
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the given file or dir</param>
        /// <returns>part of system or not</returns>
        public static bool IsASystemFile(string path)
        {
            return HasAttribute(FileAttributes.System, path);
        }

        /// <summary>
        /// - Action : Verify whether a file or directory is in readOnly
        /// </summary>
        /// <param name="path">the given file or dir</param>
        /// <returns></returns>
        public static bool IsReadOnly(string path)
        {
            return HasAttribute(FileAttributes.ReadOnly, path);
        }

        /// <summary>
        /// - Action : Verify whether the given file or directory has fa attribute <br></br>
        /// - Implementation : Not Check
        /// </summary>
        /// <param name="fa">the attribute to test</param>
        /// <param name="path">the path to test</param>
        /// <returns></returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="UnknownException">Error could not be identified</exception>
        public static bool HasAttribute(FileAttributes fa, string path)
        {
            if (File.Exists(path))
            {
                FileAttributes fas;
                try
                {
                    fas = File.GetAttributes(path);
                }
                catch (IOException)
                {
                    throw new InUseException("the file to access " + path + " is used by an external program",
                        "HasAttribute");
                }
                catch (UnauthorizedAccessException)
                {
                    throw new AccessException("the file to access " + path + " cannot be read", "HasAttribute");
                }

                return (fas & fa) != 0;
            }

            else
            {
                try
                {
                    return (new DirectoryInfo(path).Attributes & fa) != 0;
                }
                catch (Exception e)
                {
                    if (e is SecurityException || e is UnauthorizedAccessException)
                        throw new AccessException("the directory to access # " + path + " # cannot be read",
                            "HasAttribute");
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
        }

        #endregion

        #region Get

        // This region contains all Get function that also give information of files and directories

        /// <summary>
        /// Overload 1
        ///  - Action Get the parent directory name <br></br>
        ///  - Implementation : Check
        /// </summary>
        /// <param name="path">the given path</param>
        /// <returns>the parent string name</returns>
        /// <exception cref="AccessException">the path cannot be accessed</exception>
        /// <exception cref="UnknownException">An error occured</exception>
        public static string GetParent(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    return new FileInfo(path).DirectoryName;
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException or SecurityException)
                        throw new AccessException(path + " access denied", "GetParent");
                    throw new UnknownException("Error while trying to get the Directory of " + path, "GetParent");
                }
            }
            else
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(path).Parent;
                    if (dir != null)
                    {
                        string parent = dir.Name;
                        return parent;
                    }

                    return "";
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException or SecurityException)
                        throw new AccessException(path + " access denied", "GetParent");
                    throw new UnknownException("Error while trying to get the Directory of " + path, "GetParent");
                }
            }
        }

        /// <summary>
        /// Overload 2 : see 
        /// - Action : <see cref="GetParent(string)"/>
        /// - Implementation : Check
        /// </summary>
        /// <param name="ft">the pointer to the path</param>
        /// <returns>Returns the parent dir using GetPathToName function</returns>
        public static string GetParent(FileType ft)
        {
            return GetParent(ft.Path);
        }

        /// <summary>
        /// - Action : Get the date of creation with a given path <br></br>
        ///  - Implementation : Check <br></br>
        /// </summary>
        /// <param name="path">the file or directory</param>
        /// <returns>the creation date</returns>
        /// <exception cref="AccessException">the path cannot be read</exception>
        public static string GetFileCreationDate(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    return Directory.GetCreationTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new AccessException(path + " cannot be accessed", "GetFileCreationDate");
                }
            }

            else
            {
                try
                {
                    return File.GetCreationTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new AccessException(path + " cannot be accessed","GetFileCreationDate");
                }
            }
        }

        /// <summary>
        /// - Action : Get the date of last edition with a given path <br></br>
        ///  - Implementation : Check <br></br>
        /// </summary>
        /// <param name="path">the file or directory</param>
        /// <returns>the last edition date</returns>
        /// <exception cref="AccessException">the path cannot be read</exception>
        public static string GetFileLastEdition(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    return Directory.GetLastWriteTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new AccessException(path + " cannot be accessed", "GetFileLastEdition");
                }
            }
            else
            {
                try
                {
                    return File.GetLastWriteTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new AccessException(path + " cannot be accessed","GetFileLastEdition");
                }
            }
        }

        /// <summary>
        /// - Action : Get the date of access with a given path<br></br>
        /// - Implementation : Check <br></br>
        /// </summary>
        /// <param name="path">the file or directory</param>
        /// <returns>the access date</returns>
        /// <exception cref="AccessException">the path cannot be read</exception>
        public static string GetFileAccessDate(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    return Directory.GetLastAccessTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new AccessException(path + " cannot be accessed", "GetFileCreationDate");
                }
            }

            else
            {
                try
                {
                    return File.GetLastAccessTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new AccessException(path + " cannot be accessed","GetFileCreationDate");
                }
            }
        }

        /// <summary>
        /// - Action : Get the file size in byte <br></br>
        /// - Implementation : Check (prototype)
        /// </summary>
        /// <returns>0 or the size of the file</returns>
        /// <exception cref="AccessException">the path cannot be accessed</exception>
        public static long GetFileSize(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    return new DirectoryInfo(path).EnumerateFiles("*.*", SearchOption.AllDirectories)
                        .Sum(fi => fi.Length);
                }
                catch (SecurityException)
                {
                    throw new AccessException(path + " cannot be read", "GetFileSize");
                }
            }
            else
            {
                try
                {
                    return new FileInfo(path).Length;
                }
                catch (SecurityException)
                {
                    throw new AccessException(path + " cannot be read", "GetFileSize");
                }
            }
        }

        /// <summary>
        /// - Action : Reformat an absolute path to the name of the file or dir <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <returns>A string that represents the name of an absolute path</returns>
        public static string GetPathToName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// -Action : Same as GetPathToName but does not give the extension of the file <br></br>
        /// Implementation : Check
        /// </summary>
        /// <returns>A string that represents the name of the file without its extension</returns>
        public static string GetPathToNameNoExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// - Action : Reformat a name to the absolute path if the given name and current directory are correct <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <returns>The full path of a file or directory</returns>
        /// <exception cref="AccessException">The given path cannot be read</exception>
        public static string GetNameToPath(string name)
        {
            try
            {
                string fullPath = Path.GetFullPath(name);
                fullPath = fullPath.Replace("\\", "/");
                return fullPath;
            }
            catch (SecurityException)
            {
                throw new AccessException(name + " cannot be read", "GetNameToPath");
            }
        }

        /// <summary>
        /// - Action : Returns the extension of a file. If it is a directory, it will return "" <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the path to get extension of</param>
        /// <returns>Extension of a file</returns>
        public static string GetFileExtension(string path)
        {
            return Path.GetExtension(path);
        }

        #endregion

        #region Saver

        // In this section, every function is related to the save of file and the creation of FileType
        // instance to simplify the interaction with UI

        /// <summary>
        /// Overload 1
        /// - Action : Reads the properties of a File, modifies and inits its associated FileType <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the path to save into a fileType</param>
        /// <returns>the file type associated</returns>
        public static FileType ReadFileType(string path) // TODO Verify exceptions
        {
            FileType ft = new FileType(path);
            ReadFileType(ref ft);
            return ft;
        }

        /// <summary>
        /// Overload 2
        /// - Action : Update FileType passed by reference <br></br>
        /// - Implementation : Check <br></br>
        /// </summary>
        /// <param name="ft">the fileType to update</param>
        /// <exception cref="AccessException">the data cannot be read</exception>
        /// <exception cref="UnknownException">an unknown exception occured</exception>
        /// <exception cref="InUseException">the data in being used by another program</exception>
        public static void ReadFileType(ref FileType ft)
        {
            ft.Name = GetPathToName(ft.Path);
            if (Directory.Exists(ft.Path))
            {
                ft.Type = "";
                ft.IsDir = true;
                ft.Size = 0; // TODO Deal with directorySize
            }
            else
            {
                ft.Type = GetFileExtension(ft.Path);
                ft.IsDir = false;
                ft.Size = GetFileSize(ft.Path);
            }
            ft.ReadOnly = HasAttribute(FileAttributes.ReadOnly, ft.Path);
            ft.Hidden = HasAttribute(FileAttributes.Hidden, ft.Path);
            ft.Archived = HasAttribute(FileAttributes.Archive, ft.Path);
            ft.Date = GetFileCreationDate(ft.Path);
            ft.LastDate = GetFileLastEdition(ft.Path);
            ft.AccessDate = GetFileAccessDate(ft.Path);
        }

        #endregion

        #region Algorithm

        #region Basics

        // This region contains every algorithm used for basic treatment

        /// <summary>
        /// - Action : This function takes a path and generate a new path to avoid overwrite an existing file <br></br>
        /// - Implementation : Not Check <br></br>
        /// </summary>
        /// <returns>Generate a file name</returns>
        public static string GenerateNameForModification(string path) // TODO Verify Exceptions
        {
            int i = 0;
            string res = path;
            string dir = "";
            if (File.Exists(path))
            {
                DirectoryInfo parent = new FileInfo(path).Directory;
                if (parent != null)
                    dir = parent.FullName.Replace('\\', '/');
            }
            else if (Directory.Exists(path))
            {
                DirectoryInfo parent = new DirectoryInfo(path).Parent;
                if (parent != null)
                    dir = parent.FullName.Replace('\\', '/');
            }
            else
                return path;

            string extension = GetFileExtension(path);
            string name = GetPathToNameNoExtension(path);

            while (File.Exists(res) || Directory.Exists(res))
            {
                i += 1;
                res = $"{dir}/{name}({i}){extension}";
            }

            return res;
        }

        /// <summary>
        /// - Action : verify whether the path is correct <br></br>
        /// - Implementation : Check 
        /// </summary>
        /// <param name="path">the path to test</param>
        /// <returns>correct or not</returns>
        public static bool IsPathCorrect(string path)
        {
            string name = Path.GetFileName(path);
            if (name.Length > 165 || path.Length > 255)
                return false;
            if (name.Contains('/') || name.Contains('\\') || name.Contains(':') || name.Contains('*') ||
                name.Contains('"') || name.Contains('<') || name.Contains('<') || name.Contains('>') ||
                name.Contains('|') || path.Contains('\\') || path.Contains('*') ||
                path.Contains('"') || path.Contains('<') || path.Contains('<') || path.Contains('>') ||
                path.Contains('|'))
                return false;
            return true;
        }

        /// <summary>
        /// - Compares 2 strings and returns true if the first string is greater than the other one <br></br>
        /// - Implementation : Check <br></br>
        /// - Usage : Sort Algorithm
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
        /// - Implementation : Check
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
            string hourFormat = DateTime.Now.ToString(CultureInfo.CurrentCulture).Split(' ')[2];
            if (hourFormat == date1List[2] && hourFormat != date2List[2])
                return true;
            else if (hourFormat != date1List[2] && hourFormat == date2List[2])
                return false;

            // Comapares Hours
            string[] hour = DateTime.Now.ToString(CultureInfo.CurrentCulture).Split(' ')[1].Split(':');
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
        /// - Action : Select FileTypes in a FileType list using a minimum size for their names <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="fileTypes">List of fileType supposed to not be corrupted</param>
        /// <param name="minimumNameSize">the size of name required</param>
        /// <returns>the selected pointers with conditions</returns>
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
        /// MergeSort algorithm to sort files (by names) <br></br>
        /// Implementation : Check (Working)
        /// </summary>
        /// <param name="ftList">the lit of pointer to sort</param>
        /// <returns>Returns the sorted list of filetype</returns>
        public static List<FileType> SortByName(List<FileType> ftList) // TODO Missing real tests for implementation
        {
            return DivideAndMergeAlgorithm(ftList, "name");
        }

        /// <summary>
        /// MergeSort algorithm to sort files (by sizes)
        /// Implementation : Check (Working)
        /// </summary>
        /// <param name="ftList">the lit of pointer to sort</param>
        /// <returns>Returns the sorted list of filetype</returns>
        public static List<FileType> SortBySize(List<FileType> ftList) // TODO Missing real tests for implementation
        {
            return DivideAndMergeAlgorithm(ftList, "size");
        }

        /// <summary>
        /// MergeSort algorithm to sort files (by types)
        /// Implementation : Check (Working)
        /// </summary>
        /// <param name="ftList">the lit of pointer to sort</param>
        /// <returns>Returns the sorted list of filetype</returns>
        public static List<FileType> SortByType(List<FileType> ftList) // TODO Missing real tests for implementation
        {
            return DivideAndMergeAlgorithm(ftList, "type");
        }

        /// <summary>
        /// MergeSort algorithm to sort files (by modifiedDate) <br></br>
        /// Implementation : Check (Working)
        /// </summary>
        /// <param name="ftList">the lit of pointer to sort</param>
        /// <returns>the sorted list of filetype</returns>
        public static List<FileType>
            SortByModifiedDate(List<FileType> ftList) // TODO Missing real tests for implementation
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
        /// - Action : Naive research of a fileType
        /// - Implementation : Check
        /// </summary>
        /// <returns>return the fileType that has to be find, null if it does not exists</returns>
        public static FileType SearchByFullName(List<FileType> fileTypes, string fullName)
        {
            foreach (FileType ft in fileTypes)
            {
                if (ft.Name == fullName)
                    return ft;
            }

            return SearchByIndeterminedName(fileTypes, fullName);
        }

        public static FileType SearchByFullName(DirectoryType directoryType, string fullName)
        {
            return SearchByFullName(directoryType.ChildrenFiles, fullName);
        }

        /// <summary>
        /// Naive research of a fileType using an indeterminatedName which will get the most relevant file <br></br>
        /// Implementation : Check
        /// </summary>
        /// <returns>return the fileType that has to be find</returns>
        public static FileType SearchByIndeterminedName(List<FileType> fileTypes, string indeterminedName)
        {
            if (fileTypes == null)
                return null;
            FileType bestFitft = fileTypes[0];
            int maxOcc = bestFitft.Name.Length;
            List<FileType> list = SelectFileTypeByNameSize(fileTypes, indeterminedName.Length);
            foreach (FileType ft in list)
            {
                if (ft.Name == indeterminedName)
                    return ft;
                else
                {
                    int currentOcc = 0;
                    while (currentOcc < indeterminedName.Length)
                    {
                        if (ft.Name[currentOcc] == indeterminedName[currentOcc])
                            currentOcc++;
                        else
                            break;
                    }

                    if (currentOcc == indeterminedName.Length)
                        return ft;
                    else if (currentOcc > maxOcc)
                    {
                        bestFitft = ft;
                        maxOcc = currentOcc;
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
                Console.WriteLine(new StreamReader(path).ReadToEnd());
            }
        }

        #endregion
    }
}