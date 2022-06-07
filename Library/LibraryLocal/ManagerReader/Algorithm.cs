using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Library.ManagerExceptions;

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
                return path;

            var res = path;
            var dir = GetParent(path);
            // If it is a File, deal with extension
            if (File.Exists(path))
            {
                var name = GetPathToNameNoExtension(path);
                var extension = GetFileExtension(path);
                while (File.Exists(res) || Directory.Exists(res))
                    res = $"{dir}/{name} - Copy.{extension}";
                return res;
            }
            else
            {
                var name = GetPathToName(path);
                while (File.Exists(res) || Directory.Exists(res))
                    res += "- Copy";

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
            if (name.Length > 165 || path.Length > 255) return false;
            return Path.GetInvalidFileNameChars().All(c => !name.Contains(c));
        }

        public static bool IsPathCorrect(LocalPointer ft)
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
        ///     - Action : Select FileTypes in a FilePointer list using a minimum size for their names <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="fileTypes">List of fileType supposed to not be corrupted</param>
        /// <param name="minimumNameSize">the size of name required</param>
        /// <returns>the selected pointers with conditions</returns>
        private static List<LocalPointer> SelectFileTypeByNameSize(List<LocalPointer> fileTypes, int minimumNameSize)
        {
            var res = new List<LocalPointer>();
            foreach (var ft in fileTypes)
                if (ft.Name.Count() >= minimumNameSize)
                    res.Add(ft);

            return res;
        }
        
        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T">The given type</typeparam>
        /// <returns></returns>
        public static ObservableCollection<T> ListToObservable<T>(List<T> list)
        {
            var res = new ObservableCollection<T>();
            foreach (var e in list) res.Add(e);
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
            return size switch
            {
                0 => "",
                < 1024 => $"{size} B",
                < 1048576 => $"{size / 1024} KB",
                < 1073741824 => $"{size / 1048576} MB",
                _ => $"{size / 1073741824} GB"
            };
        }

        /// <summary>
        ///     - Type : High Level <br></br>
        ///     -> <see cref="ByteToPowByte(long)" />
        /// </summary>
        /// <returns>Returns string for display</returns>
        /// <exception cref="CorruptedPointerException">the given pointer is corrupteds</exception>
        public static string ByteToPowByte(LocalPointer ft)
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
                if (e is SecurityException or DirectoryNotFoundException)
                    throw new AccessException($"{path} could not be accessed", "FastReaderSize");
                if (e is ArgumentException or PathTooLongException or ArgumentNullException)
                    throw new PathFormatException($"{path} is incorrect", "FastReaderFiles");
                throw new ManagerException("Unable to get the amount of files of the given path", Level.High, "Access unable", $"unable to get size of {path}", "FastReaderFiles");
            }
        }

        #endregion

        #region Sort

        // SORT BY STRINGS

        /// <summary>
        ///     Create a new sorted list using merge algorithm
        ///     This functions takes two FilePointer list sorted them using their NAMES and returns a new one sorted with all the
        ///     elements
        ///     Implementation : Check
        /// </summary>
        private static List<LocalPointer> MergeSortFileTypeByName(List<LocalPointer> ftList1, List<LocalPointer> ftList2)
        {
            var ftReturned = new List<LocalPointer>();

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
        ///     This functions takes two FilePointer list sorted them using their SIZES and returns a new one sorted with all the
        ///     elements <br></br>
        ///     - Implementation : Check <br></br>
        /// </summary>
        /// <returns></returns>
        private static List<LocalPointer> MergeSortFileTypeBySize(List<LocalPointer> ftList1, List<LocalPointer> ftList2)
        {
            var ftReturned = new List<LocalPointer>();

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
        ///     This functions takes two FilePointer list sorted them using their TYPES and returns a new one sorted with all the
        ///     elements <br></br>
        ///     - Implementation : Check <br></br>
        /// </summary>
        /// <returns></returns>
        private static List<LocalPointer> MergeSortFileTypeByType(List<LocalPointer> ftList1, List<LocalPointer> ftList2)
        {
            var ftReturned = new List<LocalPointer>();

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
        ///     This functions takes two FilePointer list sorted them using their MODIFIED DATES and returns a new one sorted with all
        ///     the elements <br></br>
        ///     Implementation : NOT Check
        /// </summary>
        private static List<LocalPointer> MergeSortFileTypeByModifiedDate(List<LocalPointer> ftList1, List<LocalPointer> ftList2)
        {
            var ftReturned = new List<LocalPointer>();

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
        public static List<LocalPointer> SortByName(List<LocalPointer> ftList)
        {
            var dirList = new List<LocalPointer>();
            foreach (var ft in ftList.Where(ft => ft.IsDir))
            {
                dirList.Add(ft);
            }

            var fileList = new List<LocalPointer>();
            foreach (var ft in ftList.Where(ft => !ft.IsDir))
            {
                fileList.Add(ft);
            }
            fileList = DivideAndMergeAlgorithm(fileList, "name");
            fileList.Reverse();
            dirList = DivideAndMergeAlgorithm(dirList, "name");
            dirList.Reverse();
            dirList = dirList.Concat(fileList).ToList();
            return dirList;
        }

        /// <summary>
        ///     - Action : MergeSort algorithm to sort files (by sizes) <br></br>
        ///     - Implementation : Check (Working) <br></br>
        /// </summary>
        /// <param name="ftList">the lit of pointer to sort</param>
        /// <returns>Returns the sorted list of filetype</returns>
        public static List<LocalPointer> SortBySize(List<LocalPointer> ftList)
        {
            var dirList = new List<LocalPointer>();
            foreach (var ft in ftList.Where(ft => ft.IsDir))
            {
                dirList.Add(ft);
            }

            var fileList = new List<LocalPointer>();
            foreach (var ft in ftList.Where(ft => !ft.IsDir))
            {
                fileList.Add(ft);
            }
            fileList = DivideAndMergeAlgorithm(fileList, "size");
            dirList = DivideAndMergeAlgorithm(dirList, "size");
            dirList = dirList.Concat(fileList).ToList();
            return dirList;
        }

        /// <summary>
        ///     - Action : MergeSort algorithm to sort files (by types) <br></br>
        ///     - Implementation : Check (Working) <br></br>
        /// </summary>
        /// <param name="ftList">the lit of pointer to sort</param>
        /// <returns>Returns the sorted list of filetype</returns>
        public static List<LocalPointer> SortByType(List<LocalPointer> ftList)
        {
            var res =  DivideAndMergeAlgorithm(ftList, "type");
            res.Reverse();
            return res;
        }

        /// <summary>
        ///     - Action : MergeSort algorithm to sort files (by modifiedDate) <br></br>
        ///     - Implementation : Check (Working) <br></br>
        /// </summary>
        /// <param name="ftList">the lit of pointer to sort</param>
        /// <returns>the sorted list of filetype</returns>
        public static List<LocalPointer> SortByModifiedDate(List<LocalPointer> ftList)
        {
            var dirList = new List<LocalPointer>();
            foreach (var ft in ftList.Where(ft => ft.IsDir))
            {
                dirList.Add(ft);
            }

            var fileList = new List<LocalPointer>();
            foreach (var ft in ftList.Where(ft => !ft.IsDir))
            {
                fileList.Add(ft);
            }
            fileList = DivideAndMergeAlgorithm(fileList, "date");
            dirList = DivideAndMergeAlgorithm(dirList, "date");
            dirList = dirList.Concat(fileList).ToList();
            return dirList;
        }

        // Main functions

        /// <summary>
        ///     - Main algorithm : recursive method that divides and merge lists <br></br>
        ///     - Action : This functions is used for sort algorithm because it is the most efficient algorithm for string
        ///     treatment <br></br>
        ///     - Implementation : Check <br></br>
        /// </summary>
        /// <returns>Returns the sorted pointer list</returns>
        private static List<LocalPointer> DivideAndMergeAlgorithm(List<LocalPointer> ftList, string argument)
        {
            // If list is empty, returns it
            if (ftList.Count() <= 1) return ftList;

            // If not empty divide them and call the function again
            var ftList1 = new List<LocalPointer>();
            var ftList2 = new List<LocalPointer>();
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
        private static List<LocalPointer> MergeSortFileType(List<LocalPointer> ftList1, List<LocalPointer> ftList2, string argument)
        {
            return argument switch
            {
                "size" => MergeSortFileTypeBySize(ftList1, ftList2),
                "type" => MergeSortFileTypeByType(ftList1, ftList2),
                "name" => MergeSortFileTypeByName(ftList1, ftList2),
                "date" => MergeSortFileTypeByModifiedDate(ftList1, ftList2),
                _ => new List<LocalPointer>()
            };
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
        public static LocalPointer SearchByFullName(List<LocalPointer> fileTypes, string fullName)
        {
            foreach (var ft in fileTypes.Where(ft => ft.Name == fullName))
                return ft;

            return SearchByIndeterminedName(fileTypes, fullName);
        }

        /// <summary>
        ///     Naive research of a fileType using an indeterminatedName which will get the most relevant file <br></br>
        ///     Implementation : Check
        /// </summary>
        /// <returns>return the fileType that has to be find</returns>
        public static LocalPointer SearchByIndeterminedName(List<LocalPointer> pointers, string indeterminedName)
        {
            var bestFitft = pointers[0];
            var maxOcc = bestFitft.Name.Length;
            var list = SelectFileTypeByNameSize(pointers, indeterminedName.Length);
            foreach (var pointer in list)
                if (pointer.Name == indeterminedName)
                    return pointer;
                else
                {
                    var currentOcc = 0;
                    while (currentOcc < indeterminedName.Length)
                        if (pointer.Name[currentOcc] == indeterminedName[currentOcc])
                            currentOcc++;
                        else
                            break;

                    if (currentOcc == indeterminedName.Length)
                    {
                        return pointer;
                    }

                    if (currentOcc > maxOcc)
                    {
                        bestFitft = pointer;
                        maxOcc = currentOcc;
                    }
                }

            return bestFitft;
        }

        // FAST READER

        public static IEnumerable<LocalPointer> FastSearchByName(string path, string regex="", int max = 20)
        {
            if (Directory.Exists(path))
            {
                List<string> paths = new List<string>();
                try
                {
                    if (!regex.Contains('*'))
                        regex += '*';
                    foreach (var _path in Directory.EnumerateFiles(path, regex, SearchOption.AllDirectories))
                        paths.Add(_path);
                }
                catch (Exception e)
                {
                    switch (e)
                    {
                        case ArgumentException or ArgumentNullException or PathTooLongException:
                            throw new PathFormatException($"{path} has an invalid character or is too long","FastSearchByName");
                        case DirectoryNotFoundException or IOException:
                            throw new PathNotFoundException($"{path} not found in your system","FastSearchByName");
                    }
                }
                foreach (var file in paths)
                {
                    LocalPointer ft = null;
                    try
                    {
                        if (File.Exists(ft!.Path)) ft = new FilePointer.FileLocalPointer(file);
                        else ft = new DirectoryPointer.DirectoryLocalPointer(file);
                    }
                    catch (Exception)
                    { 
                        // ignored
                    }

                    if (ft is not null)
                    {
                        yield return ft;
                        max--;
                    }
                    if (max <= 0) break;
                }
            }
            else throw new PathNotFoundException(path + " could not be identified as a directory", "FastSearchByName");
        }
        

        #endregion

        #region RunApp
        
        
        [DllImport("shell32.dll")]
        static extern int FindExecutable(string lpFile, string lpDirectory, [Out] StringBuilder lpResult);

        public static List<string> RecommendedProgramsUnix(string ext)
        {
            return new List<string>(); // TODO Add recommendedPrograms for UNIX
        }

        public static List<string> RecommendedProgramsMac(string ext)
        {
            return new List<string>();
        }

        public static List<string> RecommendedPrograms(PlatformID os, string ext, string path)
        {
            switch (os)
            {
                case PlatformID.Unix:
                    return RecommendedProgramsUnix(ext);
                case PlatformID.Win32NT:
                    StringBuilder res = new StringBuilder();
                    FindExecutable(path, GetParent(path), res);
                    return new List<string>(){(res.ToString())};
                case PlatformID.MacOSX:
                    return RecommendedProgramsMac(ext);
            }

            return new List<string>();
        }

        /// <summary>
        /// Launch an app process depending on the extension of the path
        /// </summary>
        /// <param name="path">The given file to open</param>
        public static void AutoLaunchAppProcess(string path)
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            };
            p.Start();
            // System.Diagnostics.Process.Start("C:\\Users\\forix\\Documents\\anime a test.txt");
            // System.Diagnostics.Process.Start(path);
            // var extension = GetFileExtension(path);
            // var recommended = RecommendedPrograms(GetOs(), extension, path);
            // if (recommended.Count != 0)
            //     LaunchAppProcess(recommended[0], path);
        }
        
        /// <summary>
        /// Launch an app process with the application
        /// </summary>
        /// <param name="app">The application to run</param>
        /// <param name="obj">The file to open</param>
        public static void LaunchAppProcess(string app, string obj)
        {
            var process = new Process();
            var startInfo = process.StartInfo;
            startInfo.FileName = app;
            startInfo.Arguments = "\"" + obj.Replace('/','\\') + "\"";
            try
            {
                process.Start();
            }
            catch
            {
                throw new AccessException("Cannot open this type of file !", "LaunchAppProcess");
            }
        }

        #endregion
    }
}