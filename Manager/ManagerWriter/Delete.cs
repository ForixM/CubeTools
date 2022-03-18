using System;
using System.Collections.Generic;
using System.IO;
using Manager.ManagerExceptions;
using Manager.Pointers;
using Microsoft.VisualBasic.FileIO;

namespace Manager.ManagerWriter
{
    public static partial class ManagerWriter
    {
         

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
                FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin, UICancelOption.DoNothing);
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
                if (recursive)
                    FileSystem.DeleteDirectory(path, DeleteDirectoryOption.DeleteAllContents);
                else
                    FileSystem.DeleteDirectory(path, DeleteDirectoryOption.ThrowIfDirectoryNonEmpty);
            }
            catch (Exception e)
            {
                if (e is IOException)
                    throw new SystemErrorException("directory not empty or system blocked " + path, "DeleteDir");
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
    }
}