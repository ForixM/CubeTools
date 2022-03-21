using System;
using System.IO;
using Library.ManagerExceptions;

namespace Library.ManagerWriter
{
    public static partial class ManagerWriter
    {
        // CREATE FUNCTIONS

        /// <summary>
        ///     - Type : Low Level : Create a FILE using a dest name and an extension if given <br></br>
        ///     - Action : Create a file with the extension. If file already exists, nothing is done <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="dest">the given file name</param>
        /// <param name="extension">the extension given to the file. Default value is "" for directories</param>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="PathFormatException">The given path has an incorrect format</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static void Create(string dest = "", string extension = "")
        {
            // Source or dest have an incorrect format
            if (!ManagerReader.ManagerReader.IsPathCorrect(dest))
                throw new PathFormatException(dest + " : format of path is incorrect", "Create");
            var path = "";
            if (extension == "")
                path = dest == "" ? ManagerReader.ManagerReader.GenerateNameForModification("New File") : dest;
            if (!File.Exists(path))
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
                    throw new ManagerException("Writer Error", "Medium", "Creation impossible",
                        "Access has been denied", "Create");
                }
        }

        /// <summary>
        ///     => CubeTools UI Implementation <br></br>
        ///     Overload 2 : Create a DIR a name <br></br>
        ///     - Action : Create a directory using a name, "Folder" basic path value <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="dest">the dir path</param>
        /// <returns>the associated filetype linked to the directory</returns>
        public static void CreateDir(string dest = "Folder")
        {
            // Source or dest have an incorrect format
            if (!ManagerReader.ManagerReader.IsPathCorrect(dest))
                throw new PathFormatException(dest + " : format of path is incorrect", "CreateDir");
            if (!Directory.Exists(dest))
                try
                {
                    Directory.CreateDirectory(dest);
                }
                catch (Exception e)
                {
                    if (e is IOException)
                        throw new SystemErrorException("System blocked", "CreateDir");
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(dest + " could not be created", "CreateDir");
                    throw new ManagerException("An Error occured", "Medium", "Impossible to create directory",
                        "Creation of the directory could not be done", "CreateDir");
                }
        }
    }
}