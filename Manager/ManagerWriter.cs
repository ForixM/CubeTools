using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Manager
{
    public static class ManagerWriter
    {
        #region Properties
        // This region contains every function that set information of the file given with the path

        // SetProperties functions : Basicaly set the property for the file given in argument

        // Implementation : Check
        public static void SetFileHidden(string path) { File.SetAttributes(path, FileAttributes.Hidden); }
        // Implementation : Not check
        public static void SetFileCompressed(string path) { File.SetAttributes(path, FileAttributes.Compressed); }
        // Implementation : Check
        public static void SetFileSystem(string path) { File.SetAttributes(path, FileAttributes.System); }
        // Implementation : Check
        public static void SetFileArchived(string path) { File.SetAttributes(path, FileAttributes.Archive); }

        #endregion

        #region Action

        // RENAME FUNCTIONS

        /// <summary>
        /// Overload 1 : Rename with no path dest <br></br>
        /// - Action : Rename a file without specified name and does not overwrite the file if there is one which has the same path <br></br>
        /// - Specification : This function is not really usefull, consider using <see cref="Rename(string, string)"/><br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="dest">the destination file name or dir name</param>
        /// <returns>The success of the rename function</returns>
        public static bool Rename(string dest)
        {
            if (File.Exists(dest))
            {
                File.Move(dest, ManagerReader.GenerateFileNameForModification(dest));
                return true;
            } 
            else if (Directory.Exists(dest))
            {
                Directory.Move(dest, ManagerReader.GenerateDirectoryNameForModification(dest));
                return true;
            }
            else
            {
                return false;
            }
                
        }

        /// <summary>
        /// Overload 2 : Rename a file / dir using a path dest : no extension conversion<br></br>
        /// - Action : Rename a file or dir with a dest. Generate a copy by default => <see cref="ManagerReader.GenerateFileNameForModification(string)"/><br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="source">the source file name or dir name</param>
        /// <param name="dest">the destination file name or dir name</param>
        /// <returns>The success of the rename function</returns>
        public static bool Rename(string source, string dest)
        {
            if (!File.Exists(source) && !Directory.Exists(source)) // The source file exists
                return false;

            else if (File.Exists(dest) || Directory.Exists(dest)) // The dest file exists
            {
                if (ManagerReader.IsDirectory(source))
                    Directory.Move(source, ManagerReader.GenerateDirectoryNameForModification(dest));
                else
                    File.Move(source, ManagerReader.GenerateFileNameForModification(dest));
                return true;
            }
            else
            {
                if (ManagerReader.IsDirectory(source))
                    Directory.Move(source, dest);
                else
                    File.Move(source, ManagerReader.GetFileNameWithExtension(dest, Path.GetExtension(source)));
            }
            return true;
        }

        /// <summary>
        /// => For UI Implementation <br></br>
        /// Overload 3 : Try to rename a FileType using a newPath <br></br>
        /// - Action : Rename a FileType class path and its associated file using a string path <br></br>
        /// - Specification : Can be used for the UI implementation thanks to the list of <see cref="DirectoryType"/> class <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="ft">the source file linked to FileType Class</param>
        /// <param name="dest">the destination file name or dir name</param>
        /// <param name="overwrite">overwrite the file / dir : USE WITH PRECUATION</param>
        /// <returns>The success of the rename action</returns>
        public static bool Rename(FileType ft, string dest, bool overwrite = false)
        {
            if (!File.Exists(ft.Path) && !ft.IsDir)
                return false;
            else if (overwrite)
            {
                if (ft.IsDir)
                {
                    if (File.Exists(dest))
                        return false;
                    else
                    {
                        if (Directory.Exists(dest))
                            Directory.Delete(dest, true);
                        Directory.Move(ft.Path, dest);
                    }
                }
                else
                    File.Move(ft.Path, dest, true);
                ft.Path = dest;
                ManagerReader.ReadFileType(ref ft);
            }
            else
            {
                if (ft.IsDir)
                    Directory.Move(ft.Path, ManagerReader.GenerateDirectoryNameForModification(dest));
                else
                    File.Move(ft.Path, ManagerReader.GenerateFileNameForModification(dest));
                ft.Path = dest;
                ManagerReader.ReadFileType(ref ft);
                return true;
            }
            return true;
        }

        /// <summary>
        /// Overload 4 : Rename with no path dest <br></br>
        /// - Action : Rename a file using two path (source and dest). Overwrite functionnality is enabled using the last parameter<br></br>
        /// - Specification : If you do not use the overwrite functionnality, consider using <see cref="Rename(string, string)"/><br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="source">the source file name or dir name</param>
        /// <param name="dest">the destination file name or dir name</param>
        /// <param name="overwrite">true : you want to overwrite | false : you want a single copy</param>
        /// <returns>The success of the rename function</returns>
        public static bool Rename(string source, string dest, bool overwrite = true)
        {
            if (!File.Exists(source) || Directory.Exists(source))
                return false;
            else if (!overwrite)
                Rename(source, dest);
            else
            {
                if (File.Exists(dest))
                    File.Delete(dest);
                else if (Directory.Exists(dest))
                    Directory.Delete(dest, true);
                Rename(source, ManagerReader.GetFileNameWithExtension(dest));
            }
            return true;
        }
        // COPY FUNCTIONS

        /// <summary>
        /// Overload 1 : Copy the file with no dest name <br></br>
        /// - Action : Copy a file using format <see cref="ManagerReader.GenerateFileNameForModification(string)"/><br></br>
        /// - Specification : If you want to specify the name of the copy, consider using <see cref="Copy(string, string, bool)"/><br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="source">the source file name or dir full path</param>
        /// <returns>The success of the rename function</returns>
        public static bool Copy(string source)
        {
            if (File.Exists(source))
            {
                File.Copy(source, ManagerReader.GenerateFileNameForModification(source));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Overload 2 : Copy the content in the source file into the dest file <br></br>
        /// - Action : Copy file source and create one (or replace it) to dest <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest">the dest file or folder</param>
        /// <param name="replace">Replace a file or not : USE WITH PRECAUTION</param>
        /// <returns></returns>
        public static bool Copy(string source, string dest, bool replace = false)
        {
            if (File.Exists(source))
            {
                if (replace)
                {
                    if (File.Exists(dest))
                        File.Delete(dest);
                    else if (Directory.Exists(dest))
                        Directory.Delete(dest, true);
                }
                    
                if (File.Exists(dest))
                    File.Copy(source, ManagerReader.GenerateFileNameForModification(dest));
                else
                    File.Copy(source, dest);
                return true;
            }
            return false;
        }

        /// <summary>
        /// => FOR UI Implementation
        /// Overload 3 : Copy recursively the content of the source file/dir into the dest file/dir for each dir <br></br>
        /// - Action :  <br></br>
        /// ->   if it is a file, basic call to Copy function  <br></br>
        /// ->   if it is a directory each files of each sub-directories and their files will be copied. Directory will be created <br></br>
        /// - Implementation : Check (dependance on Copy using DirectoryInfo method)
        /// </summary>
        /// <param name="ft">the filetype variable</param>
        /// <param name="dest">the dest path</param>
        /// <param name="replace">files and dirs have to be replaced</param>
        /// <returns>the success of the action</returns>
        public static bool Copy(FileType ft, string dest, bool replace)
        {
            if (ft.IsDir)
            {
                if (Directory.Exists(dest))
                {
                    if (replace)
                    {
                        Directory.Delete(dest, true);
                        return Copy(new DirectoryInfo(ft.Path), new DirectoryInfo(dest), replace);
                    }
                    else
                        return Copy(new DirectoryInfo(ft.Path), new DirectoryInfo(ManagerReader.GenerateDirectoryNameForModification(dest)), replace);
                }
                else
                {
                    Directory.CreateDirectory(dest);
                    return Copy(new DirectoryInfo(ft.Path), new DirectoryInfo(dest), replace);
                }
            }

            return Copy(ft.Path, dest, replace);
        }

        /// <summary>
        /// Overload 4 : Copy contents recursively of directories using two DirectoryInfos
        /// -Action : Copy files and directories and call the function again and again until there are not directory
        /// Implementation : Check (Protype : issued encountered)
        /// </summary>
        public static bool Copy(DirectoryInfo source, DirectoryInfo target, bool replace)
        {
            if (!Directory.Exists(target.FullName))
                return false;

            // Copy Files of the current directory
            foreach (FileInfo fi in source.GetFiles())
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), replace);

            // Copy each subdirectory using recursion
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                Copy(diSourceSubDir, nextTargetSubDir, replace);
            }
            return true;
        }

        /// <summary>
        /// Overload 5 : Copy a directory and its children files <br></br>
        /// - Action : If the dest directory already exists, then rename the copy <br></br>
        /// - Specification : this function can only be useful when you want to make a self copy of the current dir <br></br>
        /// - Implementation : NOT Check - NOT Useful
        /// </summary>
        public static void Copy(ref DirectoryType dt, string dest, bool replace)
        {
            if (Directory.Exists(dt.Path))
            {
                /*
                if (Directory.Exists(dest))
                    dt.Path = ManagerReader.GenerateDirectoryNameForModification(dest);
                else
                    dt.Path = dest;

                foreach (FileType fileType in dt.ChildrenFiles)
                {
                    Copy(fileType, $"{dest}/{fileType.Name}", replace);
                }
                */
            }
        }

        // CREATE/DELETE FUNCTIONS

        /// <summary>
        /// Overload 1 : Create a file and add it an extension or not <br></br>
        /// - Action : Create a file with the extension. If file already exists, nothing is done <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the given file name</param>
        /// <param name="extension">the extension given to the file. Default value is "" for directories</param>
        public static void Create(string path, string extension = "")
        {
            if (extension != "")
                path = Path.GetFileNameWithoutExtension(path) + "." + extension;
            if (!File.Exists(path) && !Directory.Exists(path))
                File.Create(path);
        }

        /// <summary>
        /// Overload 1 : Create a file and add it an extension or not <br></br>
        /// - Action : Create a file with the extension. If file already exists, nothing is done <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the dir path</param>
        public static void CreateDir(string path)
        {
            if (Directory.Exists(path))
                Directory.CreateDirectory(ManagerReader.GenerateDirectoryNameForModification(path));
            else
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Delete a file
        /// Implementation : Check
        /// </summary>
        public static void Delete(string path) { File.Delete(path); }
        public static void Delete(FileType ft) { Delete(ft.Name); }

        /// <summary>
        /// Delete a dir
        /// Implementation : Check
        /// </summary>
        public static void DeleteDir(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        /// <summary>
        /// // Not Implemented
        /// </summary>
        public static void DeleteDir(DirectoryType dt)
        {
        }

        #endregion

    }
}
