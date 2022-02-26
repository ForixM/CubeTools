using System;
using System.Collections.Generic;
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
                    throw new PathNotFoundException("The given file does not exist", "RemoveFileAttrbiute");
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
                    DirectoryInfo di = new DirectoryInfo(path); // Security Exception
                    if (set)
                        AddDirAttribute(di,
                            fa); // AccessException, DiskNotReadyException, PathNotFoundException, ManagerException
                    else
                        RemoveDirAttribute(di,
                            fa); // AccessException, DiskNotReadyException, PathNotFoundException, ManagerException
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
                    string ndest = ManagerReader.GenerateNameForModification(dest);
                    Directory.Move(source, ndest);
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
                    string ndest = ManagerReader.GenerateNameForModification(dest);
                    File.Move(source, ndest);
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
                throw new PathFormatException(source + " : format of path is incorrect", "Rename");
            // dest does not exist, simply
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
        /// <returns>The new path created</returns>=
        public static void Copy(string source) //TODO Implement Exception for Overload 2 of Copy function
        {
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
                }
            }
            throw new PathNotFoundException(source + " does not exist", "Copy");
        }

        /// <summary>
        /// Overload 2 : Copy the content in the source file into the dest file <br></br>
        /// - Action : Copy file source and create one (or replace it) to dest <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="source">the source file or folder</param>
        /// <param name="dest">the dest file or folder</param>
        /// <param name="replace">Replace a file or not : USE WITH PRECAUTION</param>
        /// <returns>Returns the new file created, empty for errors</returns>
        public static string Copy(string source, string dest, bool replace = false) //TODO Implement Exception for Copy Function
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
                {
                    string res = ManagerReader.GenerateNameForModification(dest);
                    File.Copy(source, res);
                    return res;
                }
                File.Copy(source, dest);
                return dest;
            }

            return "";
        }

        /// <summary>
        /// => UI Implementation
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
        public static bool Copy(FileType ft, string dest, bool replace) // TODO Double check UI Implementation, Exceptions
        {
            // 1) Pointer is a directory
            if (ft.IsDir && Directory.Exists(ft.Path))
            {
                // a) Destination exists
                if (Directory.Exists(dest))
                {
                    // replace
                    if (replace)
                    {
                        // TODO Verify code structure, maybe better using string or not
                        try
                        {
                            Directory.Delete(dest, true);
                        }
                        catch (IOException)
                        {
                            // 
                        }
                        catch (UnauthorizedAccessException)
                        {
                            throw new AccessException("");
                        }
                        return Copy(new DirectoryInfo(ft.Path), new DirectoryInfo(dest), replace);
                    }
                    else
                        return Copy(new DirectoryInfo(ft.Path),
                            new DirectoryInfo(ManagerReader.GenerateNameForModification(dest)), replace);
                }
                // b) Destination does not exist, no replace needed
                else
                {
                    Directory.CreateDirectory(dest);
                    return Copy(new DirectoryInfo(ft.Path), new DirectoryInfo(dest), replace);
                }
            }
            // 2) Pointer is a file
            return (Copy(ft.Path, dest, replace) != "");
        }

        /// <summary>
        /// Overload 4 : Copy contents recursively of directories using two DirectoryInfos <br></br>
        /// -Action : Copy files and directories and call the function again and again until there are not directory <br></br>
        /// - Implementation : Check (Protype : issued encountered)
        /// </summary>
        /// <param name="source">the DirectoryInfo of the source</param>
        /// <param name="target">the DirectoryInfo of the destination</param>
        /// <param name="replace">if the directory have to be replaced if it exists</param>
        /// <returns>the success of the action</returns>
        public static bool Copy(DirectoryInfo source, DirectoryInfo target, bool replace) // TODO Verification of this function
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

        public static bool Copy(ref DirectoryType dt, FileType copied, string dest, bool replace = false)
        {
            if (Directory.Exists(dt.Path) && (File.Exists(copied.Path) || Directory.Exists(copied.Path)))
            {
                Copy(copied, dest, replace);
            }
            // Corrupted Directory
            else if (!Directory.Exists(dt.Path))
            {
                throw new CorruptedDirectoryException(dt.Name + " has not been well loaded, Copy function aborted");
            }
            // Corrupted Pointer
            else
            {
                throw new CorruptedPointerException(copied.Name + " pointer is corrupted, Copy action aborted");
            }

            return false;
        }

        // CREATE FUNCTIONS

        /// <summary>
        /// => UI Implementation <br></br>
        /// Overload 1 : Create a FILE with no name given <br></br>
        /// - Action : Create a file without any information unless the extension <br></br>
        /// - Specification : this function will be useful for creating file using Create Button <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="extension">the extension given to the file. Default value is "" for directories</param>
        /// <returns>a new fileType linked to the file</returns>
        public static FileType Create(string extension = "") // TODO Verify Exceptions
        {
            string filename =
                ManagerReader.GenerateNameForModification($"New {extension.ToUpper()} File.{extension}");
            if (extension == "")
                filename = ManagerReader.GenerateNameForModification("New File");
            File.Create(filename).Close();
            return new FileType(filename);
        }

        /// <summary>
        /// => UI Implementation <br></br>
        /// Overload 2 : Create a FILE using a dest name and an extension if given <br></br>
        /// - Action : Create a file with the extension. If file already exists, nothing is done <br></br>
        /// - Specification : If you do not need specific file name, consider using <see cref="Create(string)"/> <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="dest">the given file name</param>
        /// <param name="extension">the extension given to the file. Default value is "" for directories</param>
        /// <returns>the associated FileType for UI</returns>
        public static FileType Create(string dest, string extension)
        {
            if (extension != "")
                dest = Path.GetFileNameWithoutExtension(dest) + "." + extension;
            if (!File.Exists(dest))
            {
                File.Create(dest).Close();
                return ManagerReader.ReadFileType(dest);
            }

            return new FileType(dest);
        }

        /// <summary>
        /// => UI Implementation <br></br>
        /// Overload 1 : Create a DIR with no name <br></br>
        /// - Action : Create a directory using no name : "New Folder" basic path value <br></br>
        /// - Specification : this function can be used to create empty directory with no given name <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <returns>the associated fileType for the directory</returns>
        public static FileType CreateDir()
        {
            string dest = ManagerReader.GenerateNameForModification("New Folder");
            Directory.CreateDirectory(dest);
            return ManagerReader.ReadFileType(dest);
        }

        /// <summary>
        /// => UI Implementation <br></br>
        /// Overload 2 : Create a DIR a name <br></br>
        /// - Action : Create a directory using a name, "New Folder" basic path value <br></br>
        /// - Specification : consider using <see cref="CreateDir()"></see> if you juste want to create a directory/><br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the dir path</param>
        /// <returns>the associated filetype linked to the directory</returns>
        public static FileType CreateDir(string path = "New Folder")
        {
            if (Directory.Exists(path))
            {
                string dest = ManagerReader.GenerateNameForModification(path);
                Directory.CreateDirectory(dest);
                return ManagerReader.ReadFileType(dest);
            }
            else
            {
                Directory.CreateDirectory(path);
                return ManagerReader.ReadFileType(path);
            }
        }

        // DELETED FUNCTIONS

        /// <summary>
        /// Overload 1 : Delete a file using its path if it exists <br></br>
        /// - Action : Delelte a file <br></br>
        /// - Specification : consider using <see cref="Delete(ref DirectoryType, FileType)"></see> for UI/> <br></br>
        /// - Implementation : NOT Check
        /// </summary>
        /// <param name="path">the path of the file</param>
        /// <returns>the success of the delete action</returns>
        public static bool Delete(string path)
        {
            if (File.Exists(path))
            {
                try
                { File.Delete(path); }
                catch (IOException)
                { return false; }
                catch (SecurityException)
                { return false;}
                return true;
            }

            return false;
        }

        /// <summary>
        /// Overload 2 : Delete a file using its associated FileType <br></br>
        /// - Action : Delete a file <br></br>
        /// - Specification : consider using <see cref="Delete(ref DirectoryType, FileType)"></see> for UI/> <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="ft">a fileType that is associated to a file</param>
        /// <returns>the success of the delete action</returns>
        public static bool Delete(FileType ft)
        {
            if (Delete(ft.Path))
            {
                ft.Dispose();
                return true;
            }

            return false;
        }

        /// <summary>
        /// => UI Implementation (better using <see cref="Delete(ref DirectoryType, List{FileType})"/>) <br></br>
        /// Overload 3 : Delete a FileType contained in a DirectoryType <br></br>
        /// - Action : delete a FileType and its associated file, remove the child <br></br>
        /// - WARNING : THIS FUNCTION IS NOT CORRECT, CONSIDER USING <see cref="Delete(ref DirectoryType, List{FileType})"/> <br></br>
        /// - Implementation : NOT Check
        /// </summary>
        /// <param name="dt">a DirectoryType associated to the current directory</param>
        /// <param name="ft">a FileType children of dt</param>
        /// <returns></returns>
        public static bool Delete(ref DirectoryType dt, FileType ft)
        {
            if (Directory.Exists(dt.Path) && File.Exists(ft.Path) && dt.HasChild(ft.Path) && Delete(ft))
            {
                dt.ChildrenFiles.Remove(ft);
                return true;
            }

            return false;
        }

        /// <summary>
        /// => UI Implementation <br></br>
        /// Overload 4 : <br></br>
        /// - Action : Delete FileType of a dierctoryType and their associated files if they exist <br></br>
        /// - Specification : Prefere using this function to remove a single file to simplify the usage of the delete function <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="dt">the current directory type</param>
        /// <param name="ftList">a list of FileType that has to be deleted</param>
        /// <returns>the success of the action</returns>
        public static bool Delete(ref DirectoryType dt, List<FileType> ftList)
        {
            bool result = true;
            foreach (FileType ft in ftList)
            {
                result &= Delete(ref dt, ft);
            }

            return result;
        }

        /// <summary>
        /// Overload 1 : Delete a directory using its path <br></br>
        /// - Action : Delete a directory using its path, recursive variable indicate if all subdirectories has to be deleted <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the directory path</param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static bool DeleteDir(string path, bool recursive = true)
        {
            if (Directory.Exists(path))
            {
                if (!recursive)
                {
                    try
                    {
                        Directory.Delete(path, false);
                        return true;
                    }
                    catch (IOException)
                    {
                        return false;
                    }
                }

                try
                {
                    Directory.Delete(path, true);
                }
                catch (IOException)
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Overload 2 : Delete a directory using its associated class <br></br>
        /// - Action : Delete a directory using its class, recursive variable indicate if all subdirectories has to be deleted <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="ft"></param>
        /// <param name="recursive"></param>
        /// <returns>the success</returns>
        public static bool DeleteDir(FileType ft, bool recursive = true)
        {
            if (ft.IsDir)
            {
                if (DeleteDir(ft.Path, recursive))
                {
                    ft.Dispose();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// => UI Implementation <br></br>
        /// Overload 3 : Delete directories <br></br>
        /// - Action : Delete a directories using their classes, recursive variable indicate if all subdirectories / files have to be deleted <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="ftList">the fileType list</param>
        /// <param name="recursive">if it has to suppress all sub-dierctories</param>
        /// <returns>the success</returns>
        public static bool DeleteDir(List<FileType> ftList, bool recursive = true)
        {
            bool result = true;
            foreach (FileType ft in ftList)
            {
                result &= DeleteDir(ft, recursive);
            }

            return result;
        }

        #endregion
    }
}