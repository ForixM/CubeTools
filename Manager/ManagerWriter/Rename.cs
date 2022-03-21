using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using Manager.ManagerExceptions;
using Manager.Pointers;

namespace Manager.ManagerWriter
{
    public static partial class ManagerWriter
    {
        // RENAME FUNCTIONS

        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action : Rename a file or dir with a dest. Generate a copy by default =>/><br></br>
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
        public static async void Rename(string source, string dest)
        {
            // Source does not exist
            if (!File.Exists(source) && !Directory.Exists(source))
                throw new PathNotFoundException("Impossible to rename data", "Rename");
            // Source and dest are the same
            if (source == dest)
                return;
            // Source or dest have an incorrect format
            if (!ManagerReader.ManagerReader.IsPathCorrect(source) || !ManagerReader.ManagerReader.IsPathCorrect(dest))
                throw new PathFormatException(source + " : format of path is incorrect", "Rename");
            // The given source exists
            if (Directory.Exists(source))
            {
                // dest already exists => replace exceptions launched
                if (Directory.Exists(dest))
                    throw new ReplaceException(dest + " already exist, cannot merge directories", "Rename"); // TODO Add Asking for changes by user
                try
                {
                    Directory.Move(source, dest);
                    return;
                }
                catch (Exception e)
                {
                    if (e is IOException)
                        throw new InUseException(source + " is already used by an external program or is contained in another volume", "Rename");
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(source + " could not be renamed", "Rename");
                    throw new ManagerException("", "", "", "", ""); // TODO Edit exception
                }
            }
            // The given source is a file : basic algorithm => just rename the file
            if (File.Exists(source))
            {
                // file dest already exists
                if (File.Exists(dest))
                    throw new ReplaceException(dest + " already exist, cannot merge overwrite files", "Rename"); // TODO Ask for the user : REPLACE
                try
                {
                    File.Move(source, dest);
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(source + " could not be renamed", "Rename");
                    throw new ManagerException("", "", "", "", ""); // TODO EDIT EXCEPTION
                }
            }
        }

        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action : Rename a file or dir with a dest. Generate a copy by default => /><br></br>
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
            if (Directory.Exists(source) && !Directory.Exists(dest))
                Rename(source, dest);
            if (!Directory.Exists(source) && !File.Exists(source))
                throw new PathNotFoundException($"{source} could not be found, rename aborted", "RenameMerge");
            
            // Source and dest are the same
            if (source == dest)
                return;
            //int percentage = 0;
            //int amount = ManagerReader.ManagerReader.FastReaderFiles(source);
            List<Tuple<bool, string>> status = new List<Tuple<bool, string>>();
            try
            {
                DirectoryInfo di = new DirectoryInfo(source);
                _RenameMerge(status, di, dest);
                /* // TODO SYSTEM OF MESSAGES BOX COULD BE ADDED
                foreach (var item in status)
                {
                    if (!item.Item1)
                        
                }*/
            }
            catch (SecurityException e)
            {
                throw new AccessException("", ""); // TODO EDIT EXCEPTION
            }
        }

        /// <summary>
        /// - Action : Main algorithm, it uses recursion
        ///    * Generate subdirs and browse and rename all subfiles.
        ///    * Generate a copy by default
        /// - Specification : The used of a Tuple status keeps a stack trace of errors.
        /// - Implementation : NOT CHECK
        /// </summary>
        /// <param name="status"></param>
        /// <param name="workingDir"></param>
        /// <param name="modifiedDirPath"></param>
        private static void _RenameMerge(List<Tuple<bool, string>> status, DirectoryInfo workingDir, string modifiedDirPath)
        {
            // CREATE THE WORKING DIRECTORY IF NOT CREATED
            if (!Directory.Exists(modifiedDirPath))
                CreateDir(modifiedDirPath);
            
            // BROWSE SUB FILES OF WORKING DIRS
            foreach (var fi in workingDir.GetFiles())
            {
                // GENERATE THE NEW PATH
                string fiPath = modifiedDirPath + '/' + fi.Name;
                // If the path already exists, generate a copy
                if (File.Exists(fiPath))
                {
                    Copy(fi.FullName.Replace('\\','/'), ManagerReader.ManagerReader.GenerateNameForModification(fiPath).Replace('\\','/'));
                    Delete(fi.FullName);
                    status.Add(Tuple.Create(false, fi.FullName));
                }
                // Otherwise, simply move the file to the new directory
                else
                {
                    SimpleRenameFile(fi.FullName.Replace('\\','/'), fiPath);
                    status.Add(Tuple.Create(true, fiPath));
                }
            }
            // BROWSE SUB DIRECTORIES OF WORKING DIRECTORIES
            bool CanBeDeleted = true; // Useful to check if all sub-dirs of the working directories can be treated
            foreach (var di in workingDir.EnumerateDirectories())
            {
                // di => each DI of the working directory | di.Name => path of the data to rename
                // diPath => the 
                string diPath = modifiedDirPath + '/' + di.Name;
                Console.WriteLine(diPath);
                if (!Directory.Exists(diPath))
                    CreateDir(diPath);
                try
                {
                    _RenameMerge(status,new DirectoryInfo(workingDir.FullName + '/' + di.Name), diPath);
                    status.Add(Tuple.Create<bool, string>(true, diPath));
                }
                catch (Exception)
                {
                    CanBeDeleted = false;
                    continue; // SKIP error
                }
            }

            if (CanBeDeleted)
            {
                DeleteDir(workingDir.FullName);
                status.Add(Tuple.Create<bool, string>(true, modifiedDirPath));
            }
            else
            {
                status.Add(Tuple.Create<bool, string>(false, workingDir.FullName));
            }
        }

        /// <summary>
        /// // TODO Add descriptions
        /// </summary>
        /// <param name="source">The source file</param>
        /// <param name="dest">The new path to give</param>
        /// <exception cref="AccessException">The given </exception>
        /// <exception cref="ManagerException"></exception>
        /// <exception cref="PathNotFoundException"></exception>
        private static void SimpleRenameFile(string source, string dest)
        {
            if (!File.Exists(source))
                throw new PathNotFoundException($"{source} does not exist", "SimpleRenameFile");
            try
            {
                File.Move(source, dest);
            }
            catch (Exception e)
            {
                if (e is UnauthorizedAccessException)
                    throw new AccessException("", "SimpleRenameFile"); // TODO EDIT EXCEPTION
                throw new ManagerException("", "", "", "",""); // TODO EDIT EXCEPTION
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
        public static void Rename(string source, string dest, bool merge = false)
        {
            if (merge)
                RenameMerge(source, dest);
            else
                Rename(source,dest);
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
    }
}