using System.Security;
using Library.ManagerExceptions;

namespace Library.ManagerWriter
{
    public static partial class ManagerWriter
    {
        // CREATE FUNCTIONS

        /// <summary>
        ///     - Type : Low Level : Create a FILE using a dest name and an extension if given <br></br>
        ///     - Action : Create a file with the extension. If file already exists, creat one by generating a new name <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="dest">the given FILE NAME</param>
        /// <param name="extension">the extension given to the file. Default value is "" for directories</param>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="PathFormatException">The given path has an incorrect format</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static FilePointer.FilePointer Create(string dest = "", string extension = "")
        {
            // Source or dest have an incorrect format
            if (!ManagerReader.ManagerReader.IsPathCorrect(dest))
                throw new PathFormatException(dest + " : format of path is incorrect", "Create");
            // Getting Path
            if (extension == "")
                dest = (dest == "") ? ManagerReader.ManagerReader.GenerateNameForModification("New File") : dest;
            if (File.Exists(dest))
                dest = ManagerReader.ManagerReader.GenerateNameForModification(dest);
            try
            {
                File.Create(dest).Close();
            }
            catch (Exception e)
            {
                throw e switch
                {
                    UnauthorizedAccessException => new AccessException(dest + " could not be read", "Create"),
                    NotSupportedException => new PathFormatException(dest + " : format is incorrect", "Create"),
                    _ => new ManagerException("Writer Error", "Medium", "Creation impossible", "Access has been denied",
                        "Create")
                };
            }

            return new FilePointer.FilePointer(dest);
        }

        /// <summary>
        ///     - Type : High Level <br></br>
        ///     - Action : Create a directory using a name, "Folder" basic path value <br></br>
        ///     - Specification : If it already exists, simply return the reference or a folder with
        ///         its dest modified depending on the boolean generation <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="dest">the dir path</param>
        /// <param name="generate">Whether the function has to create a real folder</param>
        /// <returns>the associated filetype linked to the directory</returns>
        public static DirectoryPointer.DirectoryPointer CreateDir(string dest = "Folder", bool generate = false)
        {
            // Source or dest have an incorrect format
            if (!ManagerReader.ManagerReader.IsPathCorrect(dest)) throw new PathFormatException(dest + " : format of path is incorrect", "CreateDir");
            // Verify the condition with the generation boolean
            switch (generate)
            {
                // If the function has to generate but it already exists, then generate a new dest name
                case true when Directory.Exists(dest):
                    dest = ManagerReader.ManagerReader.GenerateNameForModification(dest);
                    break;
                // Else no generation and dest already exists, the function can simply return the reference to the dest
                case false when Directory.Exists(dest): return new DirectoryPointer.DirectoryPointer(dest);
            }
            // Now, try to generate a new directory
            try
            {
                Directory.CreateDirectory(dest);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    ArgumentException or ArgumentNullException => new PathFormatException($"","CreateDir"),
                    IOException => new SystemErrorException($"System blocked the creation of {dest}", "CreateDir"),
                    UnauthorizedAccessException or SecurityException => new AccessException($"{dest} could not be created", "CreateDir"),
                    _ => new ManagerException("An Error occured", "Medium", "Unable to create directory",
                        $"Creation of the directory {dest} could not be done", "CreateDir")
                };
            }
            return new DirectoryPointer.DirectoryPointer(dest);
        }
    }
}