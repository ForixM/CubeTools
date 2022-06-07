using System.Security;
using Library.ManagerExceptions;
using Library.ManagerReader;

namespace Library.ManagerWriter
{
    public partial class ManagerWriter
    {
        // COPY FUNCTIONS

        /// <summary>
        ///     - Low Level : Copy the file with no dest name <br></br>
        ///     - Action : Copy a file using format <see cref="ManagerReader.GenerateNameForModification(string)" /><br></br>
        ///     - Specification : If you want to specify the name of the copy, consider using
        ///     <see cref="Copy(string, string, bool)" /><br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="source">the source file name or dir full path</param>
        /// <returns>The new path created</returns>
        /// <exception cref="PathFormatException">the format of the given path is not correct</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="AccessException">the given file could not be copied</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static LocalPointer Copy(string source)
        {
            // Source or dest have an incorrect format
            if (!ManagerReader.ManagerReader.IsPathCorrect(source))
                throw new PathFormatException(source + " : format of path is incorrect", "Copy");
            // Source does not exist
            if (!File.Exists(source) && !Directory.Exists(source))
                throw new PathNotFoundException("Impossible to rename data", "Copy");
            // Source exists
            if (File.Exists(source))
            {
                var res = ManagerReader.ManagerReader.GenerateNameForModification(source);
                try
                {
                    File.Copy(source, res);
                    return new FilePointer.FileLocalPointer(res);
                }
                catch (Exception e)
                {
                    throw e switch
                    {
                        UnauthorizedAccessException => new AccessException(source + " could not be accessed", "Copy"),
                        IOException => new ReplaceException(res + " already exist, cannot replace it", "Copy"),
                        _ => new ManagerException("ManagerException", Level.High, "Copy impossible",
                            "Cannot copy file " + source, "Copy")
                    };
                }
            }
            else
            {
                string newDest = ManagerReader.ManagerReader.GenerateNameForModification(source);
                CopyDirectory(source, newDest, true);
                return new DirectoryPointer.DirectoryLocalPointer(newDest);
            }
        }

        /// <summary>
        ///     - Low Level : Copy the content in the source file or folder into the dest file or folder <br></br>
        ///     - Action : Copy file or folder source and create one (or replace it) to dest <br></br>
        ///     - Implementation : Check
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
        public static LocalPointer Copy(string source, string dest, bool replace = false)
        {
            // Source or dest have an incorrect format
            if (!ManagerReader.ManagerReader.IsPathCorrect(source) || !ManagerReader.ManagerReader.IsPathCorrect(dest))
                throw new PathFormatException(source + " : format of path is incorrect", "Copy");
            // Source does not exist
            if (!File.Exists(source) && !Directory.Exists(source))
                throw new PathNotFoundException(source + " not found", "Copy");
            
            // Dest already exists : destroy it
            if (replace)
            {
                if (File.Exists(dest))
                    try
                    {
                        DeleteFile(dest);
                    }
                    catch (Exception e)
                    {
                        throw e switch
                        {
                            UnauthorizedAccessException => new AccessException(dest + " could not be replaced", "Copy"),
                            IOException => new InUseException(dest + " could not be replaced", "Copy"),
                            _ => new ManagerException("An error occured", Level.High, "Impossible to replace",
                                "Copy was impossible, replace could not be done", "Copy")
                        };
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
                        throw new ManagerException("An error occured", Level.Normal, "Impossible to replace",
                            "Copy was impossible, replace could not be done", "Copy");
                    }
            }

            // Then finally Copy
            try
            {
                if (File.Exists(source))
                {
                    File.Copy(source, dest);
                    return new FilePointer.FileLocalPointer(dest);
                }
                else
                {
                    CopyDirectory(source, dest, true);
                    return new DirectoryPointer.DirectoryLocalPointer(dest);
                }
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case ManagerException:
                        throw;
                    case UnauthorizedAccessException or SecurityException:
                        throw new AccessException(source + " could not be accessed", "Copy");
                    case IOException:
                        throw new SystemErrorException("system blocked copy of " + source + " to " + dest, "Copy");
                    default:
                        throw new ManagerException("Writer error", Level.Normal, "Unable to copy a file", $"{source} could not be copied", "Copy");
                }
            }
        }

        /// <summary>
        ///     => CubeTools UI Implementation <br></br>
        ///     - High Level : Copy recursively the content of the source file/dir into the dest file/dir for each dir <br></br>
        ///     - Action :  <br></br>
        ///     ->   if it is a file, basic call to Copy function  <br></br>
        ///     ->   if it is a directory each files of each sub-directories and their files will be copied. Directory will be
        ///     created <br></br>
        ///     - Implementation : Check <br></br>
        /// </summary>
        /// <param name="localPointer">the pointer variable</param>
        /// <param name="dest">the dest path</param>
        /// <param name="replace">files and dirs have to be replaced</param>
        /// <exception cref="PathFormatException">The format of the path is incorrect</exception>
        /// <exception cref="PathNotFoundException">The given path could not be found</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="InUseException">The given path is already used in another process</exception>
        /// <exception cref="ManagerException">An error occured while copying</exception>
        /// <exception cref="CorruptedPointerException">The given pointer is corrupted</exception>
        public static void Copy(LocalPointer localPointer, string dest, bool replace)
        {
            if (localPointer.Exist()) throw new CorruptedPointerException("pointer given : " + localPointer.Path + " is corrupted", "Copy");
            Copy(localPointer.Path, dest, replace);
        }
        

        // COPY FOR DIRECTORIES

        /// <summary>
        ///     - Low Level : Copy Sub-directories recursively and their sub-files <br></br>
        ///     - Action : Create a directory with the given dest, copy then sub-files and then sub-directories <br></br>
        ///     if recursive is enabled, this function also copy all sub-files of sub-directories <br></br>
        ///     - Implementation : NOT CHECK
        /// </summary>
        /// <param name="source">the source folder</param>
        /// <param name="dest">the dest folder</param>
        /// <param name="recursive">whether it should copied all subdirectories</param>
        /// <exception cref="PathNotFoundException">One file or folder does not exists</exception>
        /// <exception cref="AccessException">One file or folder could not be accessed</exception>
        /// <exception cref="PathFormatException">The format is invalid for one of the files or folder copied</exception>
        /// <exception cref="SystemErrorException">System raise an error</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static void CopyDirectory(string source, string dest, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(source);

            // Check if the source directory exists
            if (!dir.Exists) throw new PathNotFoundException($"Source directory not found: {dir.FullName}", "CopyDirectory");

            // Create directories before we start copying
            try
            {
                // Get Subdirectories
                var dirs = dir.GetDirectories();
                // Create the destination directory
                if (!Directory.Exists(dest))
                    Directory.CreateDirectory(dest);
                // Get the files in the source directory and copy to the destination directory
                foreach (var file in dir.GetFiles())
                {
                    var targetFilePath = Path.Combine(dest, file.Name);
                    file.CopyTo(targetFilePath);
                }

                // If recursive and copying subdirectories, recursively call this method
                if (recursive)
                    foreach (var subDir in dirs)
                    {
                        var newDestinationDir = Path.Combine(dest, subDir.Name);
                        CopyDirectory(subDir.FullName, newDestinationDir, true);
                    }
            }
            catch (Exception e)
            {
                throw e switch
                {
                    SecurityException or UnauthorizedAccessException => new AccessException(
                        dir.FullName + " could not be accessed", "CopyDirectory"),
                    IOException => new SystemErrorException("Copy could not be done", "CopyDirectory"),
                    PathTooLongException or NotSupportedException => new PathFormatException(
                        "A file / folder contained a path too large", "CopyDirectory"),
                    _ => new ManagerException("An error occured", Level.Normal, "Copy directories",
                        "Copy sub-dirs and sub-files could not be done", "CopyDirectory")
                };
            }
        }
    }
}