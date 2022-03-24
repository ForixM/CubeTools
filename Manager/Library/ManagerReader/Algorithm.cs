// System's imports

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using Library.ManagerExceptions;
using Library.Pointers;
using Microsoft.Win32;
// Windows
// Imports of Library Project
// Imports of Class Library ConfigLoader

namespace Library.ManagerReader
{
    public static partial class ManagerReader
    {
        #region Basics

        // This region contains every algorithm used for basic treatment

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : This function takes a path and generate a new path to avoid overwrite an existing file <br></br>
        ///     - Implementation : Not Check <br></br>
        /// </summary>
        /// <param name="path">the path of the file/folder</param>
        /// <returns>Generate a file name</returns>
        /// <exception cref="PathNotFoundException">the data cannot be read</exception>
        /// <exception cref="AccessException">the file / folder or parent folder cannot be accessed</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static string GenerateNameForModification(string path)
        {
            // If the given path does not exist
            if (!File.Exists(path) && !Directory.Exists(path))
                throw new PathNotFoundException("the given path " + path + " does not exist",
                    "GenerateNameForModification");

            var res = path;
            var dir = GetParent(path); // Access Exception, PathNotFoundException, ManagerException
            var i = 0;
            // If it is a File, deal with extension
            if (File.Exists(path))
            {
                var name = GetPathToNameNoExtension(path);
                var extension = GetFileExtension(path);
                while (File.Exists(res) || Directory.Exists(res))
                {
                    i += 1;
                    res = $"{dir}/{name}({i}){extension}";
                }

                return res;
            }
            else
            {
                var name = GetPathToName(path);
                while (File.Exists(res) || Directory.Exists(res))
                {
                    i += 1;
                    res = $"{dir}/{name}({i})";
                }

                return res;
            }
        }

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : verify whether the path is correct <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="path">the path to test</param>
        /// <returns>correct or not</returns>
        public static bool IsPathCorrect(string path)
        {
            var name = GetPathToName(path);
            if (name.Length > 165 || path.Length > 255)
                return false;
            foreach (var c in Path.GetInvalidPathChars())
                if (name.Contains(c))
                    return false;
            return true;
        }

        public static bool IsPathCorrect(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed",
                    "IsPathCorrect");
            return IsPathCorrect(ft.Path);
        }

        /// <summary>
        ///     - Compares 2 strings and returns true if the first string is greater than the other one <br></br>
        ///     - Implementation : Check <br></br>
        ///     - Usage : Sort Algorithm
        /// </summary>
        private static bool GreaterThan(string s1, string s2)
        {
            var i = 0;
            var j = 0;
            var limit1 = s1.Count();
            var limit2 = s2.Count();
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
        ///     - Action: Verifies if the fist given date is greater than the second one <br></br>
        ///     - Format : MONT/DAY/YEAR HOUR/MIN/SEC PM or AM <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <returns>Returns if the first one is more recent than the other</returns>
        public static bool MoreRecentThanDate(string date1, string date2)
        {
            var date1List = date1.Split(' '); // [1/28/2022, 12:17:56, PM]
            var date2List = date2.Split(' ');
            if (date1List.Length != 3 || date1List.Length != 3)
                return false;

            var day1 = date1List[0].Split('/');
            var day2 = date2List[0].Split('/');
            var hour1 = date1List[1].Split(':');
            var hour2 = date2List[1].Split(':');

            if (day1.Length != 3 || day2.Length != 3)
                return false;

            // Compare dates
            for (var i = day1.Length - 1; i >= 0; i--)
                if (int.Parse(day1[i]) > int.Parse(day2[i]))
                    return true;
                else if (int.Parse(day1[i]) < int.Parse(day2[i]))
                    return false;

            // Compares Hours format
            var hourFormat = DateTime.Now.ToString(CultureInfo.CurrentCulture).Split(' ')[2];
            if (hourFormat == date1List[2] && hourFormat != date2List[2])
                return true;
            if (hourFormat != date1List[2] && hourFormat == date2List[2])
                return false;

            // Comapares Hours
            var hour = DateTime.Now.ToString(CultureInfo.CurrentCulture).Split(' ')[1].Split(':');
            for (var i = hour.Length - 1; i >= 0; i--)
            {
                if (int.Parse(hour1[i]) > int.Parse(hour2[i]))
                    return true;
                if (int.Parse(hour1[i]) < int.Parse(hour2[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        ///     - Action : Select FileTypes in a FileType list using a minimum size for their names <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="fileTypes">List of fileType supposed to not be corrupted</param>
        /// <param name="minimumNameSize">the size of name required</param>
        /// <returns>the selected pointers with conditions</returns>
        private static List<FileType> SelectFileTypeByNameSize(List<FileType> fileTypes, int minimumNameSize)
        {
            var res = new List<FileType>();
            foreach (var ft in fileTypes)
                if (ft.Name.Count() >= minimumNameSize)
                    res.Add(ft);

            return res;
        }

        #endregion

        #region Calculus

        // This region contains every function that deal with algorithm for calculus

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Get the size and returns its value with a more relevant format <br></br>
        ///     - Implementation : Check <br></br>
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

        /// <summary>
        ///     - Type : High Level <br></br>
        ///     -> <see cref="ByteToPowByte(long)" />
        /// </summary>
        /// <returns>Returns string for display</returns>
        /// <exception cref="CorruptedPointerException">the given pointer is corrupteds</exception>
        public static string ByteToPowByte(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed",
                    "ByteToPowByte");
            return ByteToPowByte(ft.Size);
        }

        public static int FastReaderFiles(string path)
        {
            
            try
            {
                return new DirectoryInfo($"{path}")
                    .EnumerateFileSystemInfos("*", SearchOption.AllDirectories)
                    .Count();
            }
            catch (Exception e)
            {
                if (e is SecurityException)
                    throw new AccessException($"{path} could not be accessed", "FastReaderSize");
                throw new ManagerException("", "", "", "", ""); // TODO EDIT EXCEPTION : FAST READER FILES
            }
        }

        #endregion

        #region Sort

        // SORT BY STRINGS

        /// <summary>
        ///     Create a new sorted list using merge algorithm
        ///     This functions takes two FileType list sorted them using their NAMES and returns a new one sorted with all the
        ///     elements
        ///     Implementation : Check
        /// </summary>
        private static List<FileType> MergeSortFileTypeByName(List<FileType> ftList1, List<FileType> ftList2)
        {
            var ftReturned = new List<FileType>();

            var i = 0;
            var j = 0;
            var limit1 = ftList1.Count;
            var limit2 = ftList2.Count;

            while (i < limit1 && j < limit2)
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

            if (i != limit1)
                while (i < limit1)
                {
                    ftReturned.Add(ftList1[i]);
                    i += 1;
                }
            else
                while (j < limit2)
                {
                    ftReturned.Add(ftList2[j]);
                    j += 1;
                }

            return ftReturned;
        }

        /// <summary>
        ///     - Action : Create a new sorted list using merge algorithm <br></br>
        ///     This functions takes two FileType list sorted them using their SIZES and returns a new one sorted with all the
        ///     elements <br></br>
        ///     - Implementation : Check <br></br>
        /// </summary>
        /// <returns></returns>
        private static List<FileType> MergeSortFileTypeBySize(List<FileType> ftList1, List<FileType> ftList2)
        {
            var ftReturned = new List<FileType>();

            var i = 0;
            var j = 0;
            var limit1 = ftList1.Count;
            var limit2 = ftList2.Count;

            while (i < limit1 && j < limit2)
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

            if (i != limit1)
                while (i < limit1)
                {
                    ftReturned.Add(ftList1[i]);
                    i += 1;
                }
            else
                while (j < limit2)
                {
                    ftReturned.Add(ftList2[j]);
                    j += 1;
                }

            return ftReturned;
        }

        /// <summary>
        ///     - Action : Create a new sorted list using merge algorithm <br></br>
        ///     This functions takes two FileType list sorted them using their TYPES and returns a new one sorted with all the
        ///     elements <br></br>
        ///     - Implementation : Check <br></br>
        /// </summary>
        /// <returns></returns>
        private static List<FileType> MergeSortFileTypeByType(List<FileType> ftList1, List<FileType> ftList2)
        {
            var ftReturned = new List<FileType>();

            var i = 0;
            var j = 0;
            var limit1 = ftList1.Count;
            var limit2 = ftList2.Count;

            while (i < limit1 && j < limit2)
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

            if (i != limit1)
                while (i < limit1)
                {
                    ftReturned.Add(ftList1[i]);
                    i += 1;
                }
            else
                while (j < limit2)
                {
                    ftReturned.Add(ftList2[j]);
                    j += 1;
                }

            return ftReturned;
        }

        /// <summary>
        ///     - Action : Create a new sorted list using merge algorithm <br></br>
        ///     This functions takes two FileType list sorted them using their MODIFIED DATES and returns a new one sorted with all
        ///     the elements <br></br>
        ///     Implementation : NOT Check
        /// </summary>
        private static List<FileType> MergeSortFileTypeByModifiedDate(List<FileType> ftList1, List<FileType> ftList2)
        {
            var ftReturned = new List<FileType>();

            var i = 0;
            var j = 0;
            var limit1 = ftList1.Count;
            var limit2 = ftList2.Count;

            while (i < limit1 && j < limit2)
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

            if (i != limit1)
                while (i < limit1)
                {
                    ftReturned.Add(ftList1[i]);
                    i += 1;
                }
            else
                while (j < limit2)
                {
                    ftReturned.Add(ftList2[j]);
                    j += 1;
                }

            return ftReturned;
        }

        // Sort functions

        /// <summary>
        ///     - Action : MergeSort algorithm to sort files (by names) <br></br>
        ///     - Implementation : Check (Working)
        /// </summary>
        /// <param name="ftList">the lit of pointer to sort</param>
        /// <returns>Returns the sorted list of filetype</returns>
        public static List<FileType> SortByName(List<FileType> ftList)
        {
            return DivideAndMergeAlgorithm(ftList, "name");
        }

        /// <summary>
        ///     - Action : MergeSort algorithm to sort files (by sizes) <br></br>
        ///     - Implementation : Check (Working) <br></br>
        /// </summary>
        /// <param name="ftList">the lit of pointer to sort</param>
        /// <returns>Returns the sorted list of filetype</returns>
        public static List<FileType> SortBySize(List<FileType> ftList)
        {
            return DivideAndMergeAlgorithm(ftList, "size");
        }

        /// <summary>
        ///     - Action : MergeSort algorithm to sort files (by types) <br></br>
        ///     - Implementation : Check (Working) <br></br>
        /// </summary>
        /// <param name="ftList">the lit of pointer to sort</param>
        /// <returns>Returns the sorted list of filetype</returns>
        public static List<FileType> SortByType(List<FileType> ftList)
        {
            return DivideAndMergeAlgorithm(ftList, "type");
        }

        /// <summary>
        ///     - Action : MergeSort algorithm to sort files (by modifiedDate) <br></br>
        ///     - Implementation : Check (Working) <br></br>
        /// </summary>
        /// <param name="ftList">the lit of pointer to sort</param>
        /// <returns>the sorted list of filetype</returns>
        public static List<FileType>
            SortByModifiedDate(List<FileType> ftList)
        {
            return DivideAndMergeAlgorithm(ftList, "date");
        }

        // Main functions

        /// <summary>
        ///     - Main algorithm : recursive method that divides and merge lists <br></br>
        ///     - Action : This functions is used for sort algorithm because it is the most efficient algorithm for string
        ///     treatment <br></br>
        ///     - Implementation : Check <br></br>
        /// </summary>
        /// <returns>Returns the sorted pointer list</returns>
        private static List<FileType> DivideAndMergeAlgorithm(List<FileType> ftList, string argument)
        {
            // If list is empty, returns it
            if (ftList.Count() <= 1) return ftList;

            // If not empty divide them and call the function again
            var ftList1 = new List<FileType>();
            var ftList2 = new List<FileType>();
            for (var i = 0; i < ftList.Count / 2; i++)
                ftList1.Add(ftList[i]);
            for (var i = ftList.Count / 2; i < ftList.Count(); i++)
                ftList2.Add(ftList[i]);
            return MergeSortFileType(DivideAndMergeAlgorithm(ftList1, argument),
                DivideAndMergeAlgorithm(ftList2, argument), argument);
        }

        /// <summary>
        ///     - Action : Sort fileType list using the merge sort algorithm <br></br>
        ///     The string argument gets the wanted value to be sort <br></br>
        ///     - Implementation : Not Check
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

        // NAIVE ALGORITHM 

        /// <summary>
        ///     - Action : Naive research of a fileType using its fullName<br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="fileTypes"></param>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static FileType SearchByFullName(List<FileType> fileTypes, string fullName)
        {
            foreach (var ft in fileTypes)
                if (ft.Name == fullName)
                    return ft;

            return SearchByIndeterminedName(fileTypes, fullName);
        }

        /// <summary>
        /// </summary>
        /// <param name="directoryType"></param>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static FileType SearchByFullName(DirectoryType directoryType, string fullName)
        {
            return SearchByFullName(directoryType.ChildrenFiles, fullName);
        }

        /// <summary>
        ///     Naive research of a fileType using an indeterminatedName which will get the most relevant file <br></br>
        ///     Implementation : Check
        /// </summary>
        /// <returns>return the fileType that has to be find</returns>
        public static FileType SearchByIndeterminedName(List<FileType> fileTypes, string indeterminedName)
        {
            if (fileTypes == null)
                return null;
            var bestFitft = fileTypes[0];
            var maxOcc = bestFitft.Name.Length;
            var list = SelectFileTypeByNameSize(fileTypes, indeterminedName.Length);
            foreach (var ft in list)
                if (ft.Name == indeterminedName)
                {
                    return ft;
                }
                else
                {
                    var currentOcc = 0;
                    while (currentOcc < indeterminedName.Length)
                        if (ft.Name[currentOcc] == indeterminedName[currentOcc])
                            currentOcc++;
                        else
                            break;

                    if (currentOcc == indeterminedName.Length)
                    {
                        return ft;
                    }

                    if (currentOcc > maxOcc)
                    {
                        bestFitft = ft;
                        maxOcc = currentOcc;
                    }
                }

            return bestFitft;
        }

        public static FileType SearchByIndeterminedName(DirectoryType directoryType, string indeterminated)
        {
            return SearchByIndeterminedName(directoryType.ChildrenFiles, indeterminated);
        }

        // RESEARCH USING FILES :
        // Method : Using files that stores all files, this algorithm will efficiently gets information about a file
        // Indexation : It uses lines number on each line of the file
        // Searching : Dichotomy research

        //
        public static List<string> SearchIndexationByFullName(string source, string name)
        {
            var res = new List<string>();
            var sr = StreamReader.Null;
            try
            {
                sr = new StreamReader(GenerateIndexation(source));
            }
            catch (Exception e)
            {
                if (e is UnauthorizedAccessException)
                    Console.WriteLine("hhelo");
                if (e is ArgumentException or ArgumentNullException)
                    throw new PathFormatException("", "SearchIndexationByFullName");
                if (e is FileNotFoundException or DirectoryNotFoundException)
                    throw new PathNotFoundException("", "SearchIndexationByFullName");
                throw new ManagerException("", "", "", "", "");
            }

            var line = sr.ReadLine();
            while (line != null)
            {
                var tmp = GetInformationByLineIndexation(line);
                if (tmp.Item2 == name)
                    res.Add(tmp.Item3);
                line = sr.ReadLine();
            }

            return res;
        }

        /// <summary>
        ///     - Action : Generate the indexation files
        ///     - Implementation : NOT CHECK
        /// </summary>
        /// <param name="baseRoot">The given working directory</param>
        /// <exception cref="InUseException"></exception>
        /// <exception cref="AccessException"></exception>
        /// <exception cref="PathFormatException"></exception>
        /// <exception cref="PathNotFoundException">The given baseRoot does not exist</exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// //TODO ADD OTHERS EXCEPTIONS
        private static string GenerateIndexation(string baseRoot)
        {
            if (!Directory.Exists(baseRoot))
                throw new PathNotFoundException($"{baseRoot} does not exist", "GenerateIndexation");

            var indexationName = $"indexation{GetPathToName(baseRoot)}.txt";
            var indexationPath = "";
            indexationPath = GetNameToPath(indexationName);
            if (File.Exists(indexationPath))
                ManagerWriter.ManagerWriter.Delete(indexationPath); // Delete all indexation
            ManagerWriter.ManagerWriter.Create(indexationName); // Create new one
            var sw = StreamWriter.Null;
            try
            {
                sw = new StreamWriter(indexationName); // Generate a stream
            }
            catch (Exception e)
            {
                if (e is UnauthorizedAccessException or SecurityException)
                    throw new AccessException($"{indexationName} could not be created", "GenerateIndexation");
                throw new ManagerException("", "", "", "", "");
            }

            var di = new DirectoryInfo(baseRoot);
            var line = 0;
            WriteRec(di, sw, ref line);
            sw.Close();
            return di.FullName;
        }

        /// <summary>
        ///     - Action : Using recursive method, the given function browses all subdirs and subfiles using a directory
        ///     the name, path and line are written in the stream sw
        ///     - Implementation : NOT CHECK
        /// </summary>
        /// <param name="di">The given current directory browsed</param>
        /// <param name="sw">The stream to modify</param>
        /// <param name="line">The current line in the file</param>
        private static void WriteRec(DirectoryInfo di, StreamWriter sw, ref int line)
        {
            // Browse each sub-files
            IEnumerable<FileInfo> subfis = new List<FileInfo>();
            try
            {
                subfis = di.EnumerateFiles();
            }
            catch (SecurityException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }

            foreach (var fi in subfis)
            {
                line++;
                try
                {
                    sw.WriteLine(line + ":" + fi.Name + "," + fi.FullName);
                }
                catch (SecurityException)
                {
                    sw.WriteLine(line + ":" + fi.Name + "," + "UNKNOWN!");
                }
            }

            // Browse each sub-directories
            IEnumerable<DirectoryInfo> subdirs = new List<DirectoryInfo>();
            try
            {
                subdirs = di.EnumerateDirectories();
            }
            catch (SecurityException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }

            foreach (var subDi in subdirs) WriteRec(subDi, sw, ref line);
        }

        private static Tuple<int, string, string> GetInformationByLineIndexation(string line)
        {
            var i = 0;
            // Getting the line's number
            var lineNumber = "";
            while (i < line.Length && line[i] != ':')
            {
                lineNumber += line[i];
                i++;
            }

            // Getting the name's number
            var name = "";
            while (i < line.Length && line[i] != ',') name += line[i];
            // Getting the path's number
            var path = "";
            while (i < line.Length) path += line[i];
            return new Tuple<int, string, string>(int.Parse(lineNumber), name, path);
        }

        #endregion

        #region RunApp

        // This region implements all algorithm to run an app using conf file
        public static List<string> RecommendedProgramsWindows(string ext)
        {
            var names = new List<string>();
            var baseKey = @"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\." + ext;
            string s;

            using (var rk = Registry.CurrentUser.OpenSubKey(baseKey + @"\OpenWithList"))
            {
                if (rk != null)
                {
                    var mruList = (string) rk.GetValue("MRUList");
                    if (mruList != null)
                        foreach (var c in mruList)
                        {
                            s = rk.GetValue(c.ToString()).ToString();
                            if (s.ToLower().Contains(".exe"))
                                names.Add(s);
                        }
                }
            }

            if (names.Count == 0)
                return new List<string>();


            //Search paths:
            var paths = new List<string>();
            baseKey = @"Software\Classes\Applications\{0}\shell\open\command";

            foreach (var name in names)
                using (var rk = Registry.LocalMachine.OpenSubKey(string.Format(baseKey, name)))
                {
                    if (rk != null)
                    {
                        //Console.WriteLine(name);
                        s = rk.GetValue("").ToString();
                        if (s.Contains("\""))
                            s = s.Trim('\"'); //remove quotes
                        paths.Add(s);
                    }
                }

            return paths;
        }

        public static List<string> RecommendedProgramsUnix(string ext)
        {
            return new List<string>(); // TODO Add recommendedPrograms for UNIX
        }

        public static List<string> RecommendedProgramsMac(string ext)
        {
            return new List<string>();
        }

        public static List<string> RecommendedPrograms(PlatformID os, string ext)
        {
            switch (os)
            {
                case PlatformID.Unix:
                    return RecommendedProgramsUnix(ext);
                case PlatformID.Win32NT:
                    return RecommendedProgramsWindows(ext);
                case PlatformID.MacOSX:
                    return RecommendedProgramsMac(ext);
            }

            return new List<string>();
        }

        public static void SelectAppProcess(string path)
        {
            var extension = GetFileExtension(path);
            var recommended = RecommendedPrograms(GetOS(), extension);
            if (recommended.Count != 0)
                LaunchAppProcess(recommended[0], extension);
            else
                LaunchAppProcess(ConfigLoader.ConfigLoader.GetOneSetting(extension), path);
        }

        public static void LaunchAppProcess(string app, string obj)
        {
            var process = new Process();
            var startInfo = process.StartInfo;
            startInfo.FileName = app;
            startInfo.Arguments = obj;
            process.Start();
        }

        #endregion
    }
}