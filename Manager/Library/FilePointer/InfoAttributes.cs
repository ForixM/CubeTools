using System.Security;
using Library.ManagerExceptions;
namespace Library.FilePointer
{
    public partial class FilePointer
    {
        // This region contains every functions that give information of a pointer (ATTRIBUTES)

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Verify whether the given file pointer has the fa attribute <br></br>
        ///     - Implementation : CHECK  <br></br>
        /// </summary>
        /// <param name="fa">the attribute to test</param>
        /// <returns></returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="CorruptedPointerException">The given pointer is invalid</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public override bool HasAttribute(FileAttributes fa)
        {
            if (!Exist())
            {
                if (IsDir) throw new CorruptedDirectoryException();
                throw new CorruptedPointerException();
            }

            try
            {
                return (FileInfo!.Attributes & fa) != 0;
            }
            catch (Exception e)
            {
                throw e switch
                {
                    FileNotFoundException or DirectoryNotFoundException => 
                        new PathNotFoundException("the given path " + _path + " could not be found in the system", "HasAttribute"),
                    PathTooLongException => new PathFormatException("the given path " + _path + " is invalid", "HasAttribute"),
                    IOException => new InUseException("the file " + _path + " is used by an external program",
                        "HasAttribute"),
                    UnauthorizedAccessException or SecurityException => new AccessException("the file " + _path + " access is denied",
                        "HasAttribute"),
                    _ => new ManagerException("Reader error", "High", "Impossible to read",
                        "the given path " + _path + " could not be read", "HasAttribute")
                };
            }
        }
    }
}