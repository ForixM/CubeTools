using System.Security;
using Library.ManagerExceptions;
using Microsoft.VisualBasic.FileIO;

namespace Library.ManagerWriter
{
    public static partial class ManagerWriter
    {
        // RENAME FUNCTIONS

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Rename any type of data (file or folder) <br></br>
        ///     - Specification : dest should not exist to avoid merging directories <br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <param name="source">the source path</param>
        /// <param name="dest">the destination path</param>
        /// <param name="overwrite">whether the source can be overwrite</param>
        /// <returns>The success of the rename function</returns>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        /// <exception cref="InUseException">the source is being used by an external program</exception>
        /// <exception cref="AccessException">the source cannot be accessed</exception>
        /// <exception cref="PathFormatException">the source format is incorrect</exception>
        /// <exception cref="ReplaceException">dest already exist, cannot overwrite</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static void Rename(string source, string dest, bool overwrite = false)
        {
            // Source does not exist
            if (!File.Exists(source) && !Directory.Exists(source)) throw new PathNotFoundException("Impossible to rename data", "Rename");
            
            // Source and dest are the same
            if (source == dest) return;
            
            // Source or dest have an incorrect format
            if (!ManagerReader.ManagerReader.IsPathCorrect(source) || !ManagerReader.ManagerReader.IsPathCorrect(dest)) throw new PathFormatException(source + " : format of path is incorrect", "Rename");
            
            // Rename whether it is a file or a folder
            if (Directory.Exists(source)) RenameDirectory(source, dest, overwrite);
            else RenameFile(source, dest, overwrite);
        }

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Rename dir with a dest.<br></br>
        ///     - Specification : dest should not exist to avoid merging directories
        ///     - Implementation : CHECK
        /// </summary>
        /// <param name="source">the source path</param>
        /// <param name="dest">the destination path</param>
        /// <param name="overwrite">whether the folder has to overwrite its content</param>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        /// <exception cref="InUseException">the source is being used by an external program</exception>
        /// <exception cref="AccessException">the source cannot be accessed</exception>
        /// <exception cref="PathFormatException">the source format is incorrect</exception>
        /// <exception cref="ReplaceException">dest already exist, cannot overwrite</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static void RenameDirectory(string source, string dest, bool overwrite = false)
        {
            // Source does not exist
            if (!Directory.Exists(source) && !File.Exists(source)) throw new PathNotFoundException($"{source} could not be found, rename aborted", "RenameMerge");
            // Source and dest are the same
            if (source == dest) return;
            
            // Trying to get the directory info attached to the source path and Create a new One
            DirectoryInfo? sourceDirectoryInfo;
            DirectoryInfo? destDirectoryInfo;
            try
            {
                sourceDirectoryInfo = new DirectoryInfo(source);
                if (overwrite)
                {
                    DeleteDir(dest);
                    destDirectoryInfo = CreateDir(dest).DirectoryInfo;
                }
                else destDirectoryInfo = Directory.Exists(dest) ? new DirectoryInfo(dest) : CreateDir(dest).DirectoryInfo;
            }
            catch (Exception e)
            {
                if (e is ManagerException) throw;
                throw e switch
                {
                    SecurityException => new AccessException($"Access to {source} is denied", "RenameDirectory"),
                    ArgumentNullException or PathTooLongException or ArgumentException => new PathFormatException($"{source} has an invalid format", "RenameDirectory"),
                    _ => new ManagerException("Unable to rename a folder", Level.High, "Writer error", $"Unable to rename {source} to {dest}", "RenameDirectory")
                };
            }
            
            // DirectoryInfo is supposed to be not null (and so loaded)
            if (destDirectoryInfo is null || sourceDirectoryInfo is null)
                throw new AccessException($"Access to {source} or {dest} is impossible", "RenameDirectory");
            
            // Rename files
            foreach (var fi in sourceDirectoryInfo.EnumerateFiles())
            {
                var newPath = destDirectoryInfo.FullName.Replace("\\", "/") + "/" + fi.Name;
                try
                {
                    RenameFile(fi.FullName.Replace("\\","/"), newPath, overwrite);
                }
                catch (Exception)
                {
                    
                }
            }
            // Rename folders
            foreach (var di in sourceDirectoryInfo.EnumerateDirectories())
            {
                var newPath = destDirectoryInfo.FullName.Replace("\\", "/") + "/" + di.Name;
                try
                {
                    RenameDirectory(di.FullName.Replace("\\","/"), newPath, overwrite);
                }
                catch (Exception)
                {
                    
                }
            }
        }

        /// <summary>
        ///     - Type : High Level <br></br>
        ///     - Action : Rename a dir thanks to its pointer.<br></br>
        ///     - Specification : dest should not exist to avoid merging directories
        ///     - Implementation : CHECK
        /// </summary>
        /// <param name="localPointer">the pointer associated to the source path</param>
        /// <param name="dest">the destination path</param>
        /// <param name="overwrite">whether the folder has to overwrite its content</param>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        /// <exception cref="InUseException">the source is being used by an external program</exception>
        /// <exception cref="AccessException">the source cannot be accessed</exception>
        /// <exception cref="PathFormatException">the source format is incorrect</exception>
        /// <exception cref="ReplaceException">dest already exist, cannot overwrite</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static void RenameDirectory(DirectoryPointer.DirectoryLocalPointer localPointer, string dest, bool overwrite = false) =>
            RenameDirectory(localPointer.Path, dest, overwrite);

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : A Simple rename of a file given by its path source <br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <param name="source">The source file</param>
        /// <param name="dest">The new path</param>
        /// <param name="overwrite">Whether the file has to be overwrite or not</param>
        /// <exception cref="AccessException">Unable to access source because of an access denied</exception>
        /// <exception cref="PathFormatException">The given source or the given dest has an invalid format</exception>
        /// <exception cref="PathNotFoundException">The source could not be found in the client's system</exception>
        /// <exception cref="ManagerException">An error occured while trying to rename a file</exception>
        public static void RenameFile(string source, string dest, bool overwrite = false)
        {
            if (!File.Exists(source)) throw new PathNotFoundException($"{source} does not exist", "RenameFile");
            if (File.Exists(dest) && !overwrite) throw new ReplaceException($"{dest} already exists, rename aborted", "RenameFile");
            try
            {
                if (overwrite && File.Exists(dest)) DeleteFile(dest);
                FileSystem.MoveFile(source, dest, UIOption.AllDialogs, UICancelOption.ThrowException);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    ArgumentException or ArgumentNullException or NotSupportedException or PathTooLongException => new PathFormatException($"{source} has an invalid format", "RenameFile"),
                    FileNotFoundException => new PathNotFoundException($"{source} could not be found in the client's system", "RenameFile"),
                    IOException => new DiskNotReadyException($"The disk is not ready to rename {dest}", "RenameFile"),
                    SecurityException or UnauthorizedAccessException => new AccessException($"Access to {source} is denied", "RenameFile"),
                    _ => new ManagerException("Unable to rename a file", Level.High, "Writer error", $"Unable to rename {source} to {dest}", "RenameFile")
                };
            }
        }

        /// <summary>
        ///     - Type : High Level <br></br>
        ///     - Action : A Simple rename of a file given by its pointer <br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <param name="localPointer">The pointer associated to the source</param>
        /// <param name="dest">The new path</param>
        /// <param name="overwrite">Whether the file has to be overwrite or not</param>
        /// <exception cref="AccessException">Unable to access source because of an access denied</exception>
        /// <exception cref="PathFormatException">The given source or the given dest has an invalid format</exception>
        /// <exception cref="PathNotFoundException">The source could not be found in the client's system</exception>
        /// <exception cref="ManagerException">An error occured while trying to rename a file</exception>
        public static void RenameFile(FilePointer.FileLocalPointer localPointer, string dest, bool overwrite = false) => RenameFile(localPointer.Path, dest, overwrite);
        
        /// <summary>
        ///     => CubeTools UI Implementation <br></br>
        ///     - Type : High Level <br></br>
        ///     - Action : Rename a file using its associated Pointer <br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <param name="localPointer">the source file linked to FilePointer Class</param>
        /// <param name="dest">the destination file name or dir path</param>
        /// <param name="merge">if true, merge directories and generate copy for files</param>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        /// <exception cref="InUseException">the source is being used by an external program</exception>
        /// <exception cref="AccessException">the source cannot be accessed</exception>
        /// <exception cref="PathFormatException">the source format is incorrect</exception>
        /// <exception cref="ReplaceException">dest already exist, cannot overwrite</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static void Rename(LocalPointer localPointer, string dest, bool merge = false) => Rename(localPointer.Path, dest, merge);
    }
}