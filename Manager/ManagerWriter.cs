using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security;
using Manager.ManagerExceptions;

namespace Manager
{
    public static class ManagerWriter
    {
        #region Properties

        // This region contains every function that set information of the file given with the path
        
        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action : Remove an attribute <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="fi">FileInfo instance of the given file</param>
        /// <param name="attributesToRemove">the attribute to remove</param>
        /// <returns>the new file attributes</returns>
        /// <exception cref="AccessException">the file cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        private static void RemoveFileAttribute(FileInfo fi, FileAttributes attributesToRemove)
        {
            try
            {
                fi.Attributes &= ~attributesToRemove;
            }
            catch (Exception e)
            {
                if (e is SecurityException or UnauthorizedAccessException)
                    throw new AccessException(  "File could not be accessed", "RemoveFileAttribute");
                if (e is IOException)
                    throw new DiskNotReadyException("Disk is refreshing", "RemoveFileAttribute");
                if (e is FileNotFoundException or DirectoryNotFoundException)
                    throw new PathNotFoundException("The given file does not exist", "RemoveFileAttribute");
                throw new ManagerException("Reader error", "Medium", "Impossible to read file",
                    "The file cannot be read", "RemoveFileAttribute");
            }
        }

        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action : Add an attribute to a file <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the path that has to be modified</param>
        /// <param name="attributes">attributes to add </param>
        /// <exception cref="AccessException">the given path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        private static void AddFileAttribute(string path, FileAttributes attributes)
        {
            try
            {
                File.SetAttributes(path, attributes);
            }
            catch (Exception e)
            {
                if (e is SecurityException or UnauthorizedAccessException)
                    throw new AccessException(  "File could not be accessed", "AddFileAttribute");
                if (e is IOException)
                    throw new DiskNotReadyException("Disk is refreshing", "AddFileAttribute");
                if (e is FileNotFoundException or DirectoryNotFoundException)
                    throw new PathNotFoundException("The given file does not exist", "AddFileAttribute");
                throw new ManagerException("Writer error", "Medium", "Impossible to modify file attributes",
                    "The file cannot be modified", "AddFileAttribute");
            }
        }

        /// <summary>
        /// - Action : Remove a directory attributes  <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="di">the directory that has to be modified</param>
        /// <param name="attributesToRemove">the attribute to remove</param>
        /// <exception cref="AccessException">the given path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        private static void RemoveDirAttribute(DirectoryInfo di, FileAttributes attributesToRemove)
        {
            try
            {
                di.Attributes &= ~attributesToRemove;
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException or DirectoryNotFoundException)
                    throw new PathNotFoundException("the given directory does not exist","RemoveDirAttribute");
                if (e is UnauthorizedAccessException or SecurityException)
                    throw new AccessException("The given directory cannot be accessed", "RemoveDirAttribute");
                if (e is IOException)
                    throw new DiskNotReadyException("the disk is not ready to modify data", "RemoveDirAttribute");
                throw new ManagerException("Writer error", "Medium", "Impossible to modify directory attributes",
                    "The path cannot be modified", "RemoveDirAttribute");
            }
        }

        /// <summary>
        /// - Action : Add an attribute to a directory given with a directoryInfo
        /// - Implementation : NOT Check
        /// </summary>
        /// <param name="di">the directory that has to be modified</param>
        /// <param name="attribute">the attribute that has to be added</param>
        /// <exception cref="AccessException">the given path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        private static void AddDirAttribute(DirectoryInfo di, FileAttributes attribute)
        {
            try
            {
                di.Attributes |= attribute;
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException or DirectoryNotFoundException)
                    throw new PathNotFoundException("the given directory does not exist","AddDirAttribute");
                if (e is UnauthorizedAccessException or SecurityException)
                    throw new AccessException("The given directory cannot be accessed", "AddDirAttribute");
                if (e is IOException)
                    throw new DiskNotReadyException("the disk is not ready to modify data", "AddDirAttribute");
                throw new ManagerException("Writer error", "Medium", "Impossible to modify directory attributes",
                    "The path cannot be modified", "AddDirAttribute");
            }
        }

        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action : Set an attribute given in parameter <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the path of the file</param>
        /// <param name="set">whether it has to be set or not</param>
        /// <param name="fa">the attribute that has to be set or not</param>
        /// <exception cref="AccessException">the given path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static void SetAttributes(string path, bool set, FileAttributes fa)
        {
            if (File.Exists(path))
            {
                try
                {
                    if (set)
                        AddFileAttribute(path, fa);
                    else
                        RemoveFileAttribute(new FileInfo(path), fa);
                }
                catch (Exception e)
                {
                    if (e is SecurityException or UnauthorizedAccessException)
                        throw new AccessException("the file given " + path + " access is denied", "SetAttributes");
                    throw new ManagerException();
                }
            }

            if (Directory.Exists(path))
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    if (set)
                        AddDirAttribute(di, fa);
                    else
                        RemoveDirAttribute(di, fa);
                    return;
                }
                catch (SecurityException)
                {
                    throw new AccessException("the directory given " + path + " access is denied", "SetAttributes");
                }
            }

            throw new PathNotFoundException("the given path " + path + " does not exist", "SetAttributes");
        }

        /// <summary>
        /// - Type : High Level <br></br>
        /// -> <see cref="SetAttributes(string,bool,System.IO.FileAttributes)"/>
        /// </summary>
        /// <param name="ft">a fileType associated to a file</param>
        /// <param name="set">whether it has to be set or not</param>
        /// <param name="fa">the attribute that has to be set or not</param>
        /// <exception cref="AccessException">the given path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static void SetAttributes(FileType ft, bool set, FileAttributes fa)
        {
            SetAttributes(ft.Path, set, fa);
        }

        #endregion

        #region Action

        // RENAME FUNCTIONS

        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action : Rename a file or dir with a dest. Generate a copy by default => <see cref="ManagerReader.GenerateNameForModification(string)"/><br></br>
        /// - Specification : dest should not exist to avoid merging directories
        /// - Implementation : Check
        /// </summary>
        /// <param name="source">the source path</param>
        /// <param name="dest">the destination path</param>
        /// <returns>The success of the rename function</returns>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        /// <exception cref="InUseException">the source is being used by an external program</exception>
        /// <exception cref="AccessException">the source cannot be accessed</exception>
        /// <exception cref="PathFormatException">the source format is incorrect</exception>
        /// <exception cref="ReplaceException">dest already exist, cannot overwrite</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static void Rename(string source, string dest) // TODO Create async function
        {
            // Source does not exist
            if (!File.Exists(source) && !Directory.Exists(source))
                throw new PathNotFoundException("Impossible to rename data", "Rename");
            // Source and dest are the same
            if (source == dest)
                return;
            // Source or dest have an incorrect format
            if (!ManagerReader.IsPathCorrect(source) || !ManagerReader.IsPathCorrect(dest))
                throw new PathFormatException(source + " : format of path is incorrect", "Rename");
            
            if (Directory.Exists(source))
            {
                // dest already exists
                if (Directory.Exists(dest))
                    throw new ReplaceException(dest + " already exist, cannot merge directories", "Rename");
                try
                {
                    Directory.Move(source, dest);
                    return;
                }
                catch (IOException) // Already used in a program or in another volume
                {
                    throw new InUseException(source + " is already used by an external program or is contained in another volume", "Rename");
                }
                catch (UnauthorizedAccessException) // Access denied
                {
                    throw new AccessException(source + " could not be renamed", "Rename");
                }
            }

            if (File.Exists(source))
            {
                // file dest already exists
                if (File.Exists(dest))
                    throw new ReplaceException(dest + " already exist, cannot merge overwrite files", "Rename");
                try
                {
                    if (File.Exists(dest))
                        dest = ManagerReader.GenerateNameForModification(dest);
                    File.Move(source, dest);
                }
                catch (UnauthorizedAccessException) // Access denied
                {
                    throw new AccessException(source + " could not be renamed", "Rename");
                }
            }
        }

        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action : Rename a file or dir with a dest. Generate a copy by default => <see cref="ManagerReader.GenerateNameForModification(string)"/><br></br>
        /// - Specification : dest should not exist to avoid merging directories
        /// - Implementation : Check
        /// </summary>
        /// <param name="source">the source path</param>
        /// <param name="dest">the destination path</param>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        /// <exception cref="InUseException">the source is being used by an external program</exception>
        /// <exception cref="AccessException">the source cannot be accessed</exception>
        /// <exception cref="PathFormatException">the source format is incorrect</exception>
        /// <exception cref="ReplaceException">dest already exist, cannot overwrite</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static void RenameMerge(string source, string dest) // TODO Recursive method
        {
            // Source does not exist
            if (!File.Exists(source) && !Directory.Exists(source))
                throw new PathNotFoundException("Impossible to rename data", "Rename");
            // Source and dest are the same
            if (source == dest)
                return;
            // Source or dest have an incorrect format
            if (!ManagerReader.IsPathCorrect(source) || !ManagerReader.IsPathCorrect(dest))
                throw new PathFormatException("format of path is incorrect", "Rename");
            // dest does not exist, simply rename
            if (!Directory.Exists(dest) && !File.Exists(dest))
            {
                Rename(source, dest);
                return;
            }

            if (File.Exists(source)) // source is a file and dest already exist
            {
                Rename(source, ManagerReader.GenerateNameForModification(source)); // Rename with a modified name if it exists
            }
            else // source is a directory and dest already exist
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(source); // SecurityException
                    foreach (var subFi in di.EnumerateFiles("*",SearchOption.AllDirectories).ToArray())
                    {
                        string parent = di.FullName;
                        string subPath = parent + '/' + subFi.Name;
                        Rename(subPath, ManagerReader.GenerateNameForModification(subPath));
                    }
                    foreach (var subDi in di.EnumerateDirectories("*",SearchOption.AllDirectories).ToArray())
                    {
                        string parent = di.FullName;
                        string subPath = parent + '/' + subDi.Name; // TODO ERRRRROR
                        RenameMerge(subDi.FullName, subPath);
                    }
                }
                catch (SecurityException)
                {
                    throw new AccessException(source + " could not be read", "Rename");
                }
            }
            
        }

        /// <summary>
        /// => UI Implementation <br></br>
        /// - Type : High Level : Try to rename a FileType using a newPath <br></br>
        /// - Action : Rename a FileType class path and its associated file using a string path <br></br>
        /// - Specification : Can be used for the UI implementation thanks to the list of <see cref="DirectoryType"/> class <br></br>
        /// - Implementation : NOT Check
        /// </summary>
        /// <param name="ft">the source file linked to FileType Class</param>
        /// <param name="dest">the destination file name or dir path</param>
        /// <param name="merge">if true, merge directories and generate copy for files</param>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        /// <exception cref="InUseException">the source is being used by an external program</exception>
        /// <exception cref="AccessException">the source cannot be accessed</exception>
        /// <exception cref="PathFormatException">the source format is incorrect</exception>
        /// <exception cref="ReplaceException">dest already exist, cannot overwrite</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static void Rename(FileType ft, string dest, bool merge = false)
        {
            if (merge)
                RenameMerge(ft.Path, dest);
            else
                Rename(ft.Path,dest);
        }

        // COPY FUNCTIONS

        /// <summary>
        /// - Low Level : Copy the file with no dest name <br></br>
        /// - Action : Copy a file using format <see cref="ManagerReader.GenerateNameForModification(string)"/><br></br>
        /// - Specification : If you want to specify the name of the copy, consider using <see cref="Copy(string, string, bool)"/><br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="source">the source file name or dir full path</param>
        /// <returns>The new path created</returns>
        /// <exception cref="PathFormatException">the format of the given path is not correct</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="AccessException">the given file could not be copied</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static void Copy(string source)
        {
            // Source or dest have an incorrect format
            if (!ManagerReader.IsPathCorrect(source))
                throw new PathFormatException(source + " : format of path is incorrect", "Copy");
            // Source does not exist
            if (!File.Exists(source) && !Directory.Exists(source))
                throw new PathNotFoundException("Impossible to rename data", "Copy");
            // Source exists
            if (File.Exists(source))
            {
                string res = ManagerReader.GenerateNameForModification(source);
                try
                {
                    File.Copy(source, res);
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(source + " could not be accessed", "Copy");
                    if (e is IOException)
                        throw new ReplaceException(res + " already exist, cannot replace it", "Copy");
                    throw new ManagerException("ManagerException", "High", "Copy impossible",
                        "Cannot copy file " + source, "Copy");
                }
            }
            else
            {
                CopyDirectory(source, ManagerReader.GenerateNameForModification(source), true);
            }
        }

        /// <summary>
        /// - Low Level : Copy the content in the source file or folder into the dest file or folder <br></br>
        /// - Action : Copy file or folder source and create one (or replace it) to dest <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="source">the source file or folder</param>
        /// <param name="dest">the dest file or folder</param>
        /// <param name="replace">Replace a file or not : USE WITH PRECAUTION</param>
        /// <returns>Returns the new file created, empty for errors</returns>
        /// <exception cref="PathFormatException">The format of the path is incorrect</exception>
        /// <exception cref="PathNotFoundException">The given path could not be found</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="InUseException">The given path is already used in another process</exception>
        /// <exception cref="ManagerException">An error occured while copying</exception>
        public static void Copy(string source, string dest, bool replace = false)
        {
            // Source or dest have an incorrect format
            if (!ManagerReader.IsPathCorrect(source) ||!ManagerReader.IsPathCorrect(dest))
                throw new PathFormatException(source + " : format of path is incorrect", "Copy");
            // Source does not exist
            if (!File.Exists(source) && !Directory.Exists(source))
                throw new PathNotFoundException(source + " not found", "Copy");
            // Dest already exists : destroy it
            if (replace)
            {
                if (File.Exists(dest))
                {
                    try
                    {
                        Delete(dest);
                    }
                    catch (Exception e)
                    {
                        if (e is UnauthorizedAccessException)
                            throw new AccessException(dest + " could not be replaced", "Copy");
                        if (e is IOException)
                            throw new InUseException(dest + " could not be replaced", "Copy");
                        throw new ManagerException("An error occured", "High", "Impossible to replace", "Copy was impossible, replace could not be done", "Copy");
                    }
                }
                else if (Directory.Exists(dest))
                    try
                    {
                        DeleteDir(dest);
                    }
                    catch (Exception e)
                    {
                        if (e is UnauthorizedAccessException)
                            throw new AccessException(dest + " could not be replaced", "Copy");
                        throw new ManagerException("An error occured", "Medium", "Impossible to replace",
                            "Copy was impossible, replace could not be done", "Copy");
                    }
            }
            // Then finally Copy
            try
            {
                if (File.Exists(source))
                {
                    Create(dest);
                    File.Copy(source, dest);
                }
                else
                {
                    CreateDir(dest);
                    CopyDirectory(source, dest, true);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                throw new AccessException(source + " could not be accessed", "Copy");
            }
            catch (IOException e) 
            {
                throw new SystemErrorException("system blocked copy of " + source + " to " + dest, "Copy");
            }
        }

        /// <summary>
        /// => UI Implementation <br></br>
        /// - High Level : Copy recursively the content of the source file/dir into the dest file/dir for each dir <br></br>
        /// - Action :  <br></br>
        /// ->   if it is a file, basic call to Copy function  <br></br>
        /// ->   if it is a directory each files of each sub-directories and their files will be copied. Directory will be created <br></br>
        /// - Implementation : Check <br></br>
        /// </summary>
        /// <param name="ft">the pointer variable</param>
        /// <param name="dest">the dest path</param>
        /// <param name="replace">files and dirs have to be replaced</param>
        /// <exception cref="PathFormatException">The format of the path is incorrect</exception>
        /// <exception cref="PathNotFoundException">The given path could not be found</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="InUseException">The given path is already used in another process</exception>
        /// <exception cref="ManagerException">An error occured while copying</exception>
        /// <exception cref="CorruptedPointerException">The given pointer is corrupted</exception>
        public static void Copy(FileType ft, string dest, bool replace)
        {
            if (!File.Exists(ft.Path))
                throw new CorruptedPointerException("pointer given : " + ft.Path + " is corrupted", "Copy");
            Copy(ft.Path, dest, replace);
        }

        /// <summary>
        /// => UI Implementation <br></br>
        /// - High Level : <see cref="Copy(string,string,bool)"/> <br></br>
        /// - Implementation : Check <br></br>
        /// </summary>
        /// <param name="dt">the current directory passed by reference</param>
        /// <param name="copied">the copied pointer of file / folder</param>
        /// <param name="dest">the path of the destination file / folder</param>
        /// <param name="replace">whether the file / folder has to replace if needed</param>
        /// <exception cref="PathFormatException">The format of the path is incorrect</exception>
        /// <exception cref="PathNotFoundException">The given path could not be found</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="InUseException">The given path is already used in another process</exception>
        /// <exception cref="ManagerException">An error occured while copying</exception>
        /// <exception cref="CorruptedPointerException">The given pointer is corrupted</exception>
        /// <exception cref="CorruptedDirectoryException">The given directory is corrupted</exception>
        public static void Copy(ref DirectoryType dt, FileType copied, string dest, bool replace = false)
        {
            if (Directory.Exists(dt.Path) && (File.Exists(copied.Path) || Directory.Exists(copied.Path)))
                Copy(copied, dest, replace);
            // Corrupted Directory
            else if (!Directory.Exists(dt.Path))
                throw new CorruptedDirectoryException(dt.Name + " has not been well loaded, Copy function aborted");
            // Corrupted Pointer
            else
                throw new CorruptedPointerException(copied.Name + " pointer is corrupted, Copy action aborted");
        }
        
        // COPY FOR DIRECTORIES
        
        /// <summary>
        /// - Low Level : Copy Sub-directories recursively and their sub-files <br></br>
        /// - Action : Create a directory with the given dest, copy then sub-files and then sub-directories <br></br>
        /// if recursive is enabled, this function also copy all sub-files of sub-directories <br></br>
        /// - Implementation : NOT CHECK
        /// </summary>
        /// <param name="source">the source folder</param>
        /// <param name="dest">the dest folder</param>
        /// <param name="recursive">whether it should copied all subdirectories</param>
        /// <exception cref="PathNotFoundException">One file or folder does not exists</exception>
        /// <exception cref="AccessException">One file or folder could not be accessed</exception>
        /// <exception cref="PathFormatException">The format is invalid for one of the files or folder copied</exception>
        /// <exception cref="SystemErrorException">System raise an error</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        static void CopyDirectory(string source, string dest, bool recursive) // TODO Make async for performance
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(source);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new PathNotFoundException($"Source directory not found: {dir.FullName}", "CopyDirectory");

            // Cache directories before we start copying
            try
            {
                // Get Subdirectories
                DirectoryInfo[] dirs = dir.GetDirectories();
                // Create the destination directory
                Directory.CreateDirectory(dest);
                // Get the files in the source directory and copy to the destination directory
                foreach (FileInfo file in dir.GetFiles())
                {
                    string targetFilePath = Path.Combine(dest, file.Name);
                    file.CopyTo(targetFilePath);
                }

                // If recursive and copying subdirectories, recursively call this method
                if (recursive)
                {
                    foreach (DirectoryInfo subDir in dirs)
                    {
                        string newDestinationDir = Path.Combine(dest, subDir.Name);
                        CopyDirectory(subDir.FullName, newDestinationDir, true);
                    }
                }
            }
            catch (Exception e)
            {
                if (e is SecurityException or UnauthorizedAccessException)
                    throw new AccessException(dir.FullName + " could not be accessed", "CopyDirectory");
                if (e is IOException)
                    throw new SystemErrorException("Copy could not be done", "CopyDirectory");
                if (e is PathTooLongException or NotSupportedException)
                    throw new PathFormatException("A file / folder contained a path too large", "CopyDirectory");
                throw new ManagerException("An error occured","Medium","Copy directories","Copy sub-dirs and sub-files could not be done","CopyDirectory");
            }
        }

        // CREATE FUNCTIONS

        /// <summary>
        /// - Type : Low Level : Create a FILE using a dest name and an extension if given <br></br>
        /// - Action : Create a file with the extension. If file already exists, nothing is done <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="dest">the given file name</param>
        /// <param name="extension">the extension given to the file. Default value is "" for directories</param>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="PathFormatException">The given path has an incorrect format</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static void Create(string dest="", string extension = "")
        {
            // Source or dest have an incorrect format
            if (!ManagerReader.IsPathCorrect(dest))
                throw new PathFormatException(dest + " : format of path is incorrect", "Create");
            string path = "";
            if (extension == "")
            {
                path = dest == "" ? ManagerReader.GenerateNameForModification("New File") : dest;
            }
            if (!File.Exists(path))
            {
                try
                {
                    File.Create(dest).Close();
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(path + " could not be read", "Create");
                    if (e is NotSupportedException)
                        throw new PathFormatException(path + " : format is incorrect", "Create");
                    throw new ManagerException("Writer Error", "Medium", "Creation impossible", "Access has been denied", "Create");
                }
            }
        }

        /// <summary>
        /// => UI Implementation <br></br>
        /// Overload 2 : Create a DIR a name <br></br>
        /// - Action : Create a directory using a name, "Folder" basic path value <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="dest">the dir path</param>
        /// <returns>the associated filetype linked to the directory</returns>
        public static void CreateDir(string dest = "Folder")
        {
            // Source or dest have an incorrect format
            if (!ManagerReader.IsPathCorrect(dest))
                throw new PathFormatException(dest + " : format of path is incorrect", "CreateDir");
            if (!Directory.Exists(dest))
            {
                try
                {
                    Directory.CreateDirectory(dest);
                }
                catch (Exception e)
                {
                    if (e is IOException)
                        throw new SystemErrorException("System blocked","CreateDir");
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(dest + " could not be created", "CreateDir");
                    throw new ManagerException("An Error occured", "Medium", "Impossible to create directory", "Creation of the directory could not be done", "CreateDir");
                }
            }
        }

        // DELETED FUNCTIONS

        /// <summary>
        /// - Low Level => Delete a file using its path if it exists <br></br>
        /// - Action : Delete a file <br></br>
        /// - Specification : consider using <see cref="Delete(ref DirectoryType, FileType)"></see> for UI/> <br></br>
        /// - Implementation : NOT Check
        /// </summary>
        /// <param name="path">the path of the file</param>
        /// <exception cref="InUseException">The given path is already used</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="ManagerException">An Error occurred</exception>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        public static void Delete(string path)
        {
            if (!File.Exists(path))
                throw new PathNotFoundException(path + " does not exist","Delete");
            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                if (e is IOException)
                    throw new InUseException(path + " is used by another process", "Delete");
                if (e is UnauthorizedAccessException)
                    throw new AccessException(path + " access is denied","Delete");
                throw new ManagerException("Delete is impossible", "Medium", "Writer Error", path + " could not be deleted", "Delete");
            }
            
        }

        /// <summary>
        /// - High Level => Delete a file using its associated FileType <br></br>
        /// - Action : Delete a file <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="ft">a fileType that is associated to a file</param>
        /// <exception cref="InUseException">The given path is already used</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="ManagerException">An Error occurred</exception>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        public static void Delete(FileType ft)
        {
            if (!File.Exists(ft.Path))
                throw new CorruptedPointerException(ft.Path + " does not exist anymore", "Delete");
            Delete(ft.Path);
            ft.Dispose();
        }

        /// <summary>
        /// - Low Level : Delete a directory using its path <br></br>
        /// - Action : Delete a directory using its path, recursive variable indicate if all subdirectories has to be deleted <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the directory path</param>
        /// <param name="recursive">whether all content inside the directory should be deleted</param>
        /// <exception cref="SystemErrorException">the system blocked app</exception>
        /// <exception cref="AccessException">Access has been denied</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        public static void DeleteDir(string path, bool recursive = true)
        {
            if (!Directory.Exists(path))
                throw new PathNotFoundException(path + " does not exist", "DeleteDir");
            try
            {
                Directory.Delete(path, recursive);
            }
            catch (Exception e)
            {
                if (e is IOException)
                    throw new SystemErrorException("system blocked " + path, "DeleteDir");
                if (e is UnauthorizedAccessException)
                    throw new AccessException(path + " access denied", "DeleteDir");
                throw new ManagerException("Impossible to Delete", "Medium", "Writer Error", path + " could not be deleted", "DeleteDir");
            }
        }

        /// <summary>
        /// - High Level : Delete a directory using its associated class <br></br>
        /// - Action : Delete a directory using its class, recursive variable indicate if all subdirectories has to be deleted <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="ft">the directory pointer</param>
        /// <param name="recursive">whether all content inside the directory should be deleted</param>
        /// <exception cref="SystemErrorException">the system blocked app</exception>
        /// <exception cref="AccessException">Access has been denied</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        public static void DeleteDir(FileType ft, bool recursive = true)
        {
            DeleteDir(ft.Path, recursive);
            ft.Dispose();
        }

        /// <summary>
        /// - High Level : Delete directories <br></br>
        /// - Action : Delete a directories using their classes, recursive variable indicate if all subdirectories / files have to be deleted <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="ftList">the fileType list</param>
        /// <param name="recursive">if it has to suppress all sub-directories</param>
        /// <exception cref="SystemErrorException">the system blocked app</exception>
        /// <exception cref="AccessException">Access has been denied</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        /// <exception cref="PathNotFoundException">One of the given pointers does not exist</exception>
        public static void DeleteDir(List<FileType> ftList, bool recursive = true)
        {
            foreach (FileType ft in ftList)
            {
                DeleteDir(ft, recursive);
            }
        }

        #endregion
    }
}