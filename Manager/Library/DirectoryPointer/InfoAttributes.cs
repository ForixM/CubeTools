using System.Security;
using Library.ManagerExceptions;
namespace Library.DirectoryPointer
{
    public partial class DirectoryPointer : Pointer
    {
        // This region contains every functions that give information of a pointer (ATTRIBUTES)

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Verify whether the given pointer has the fa attribute <br></br>
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
            try
            {
                return (DirectoryInfo!.Attributes & fa) != 0;
            }
            catch (Exception e)
            {
                throw e switch
                {
                    FileNotFoundException or DirectoryNotFoundException =>
                        new PathNotFoundException("the given path " + _path + " could not be found in the system",
                            "HasAttribute"),
                    PathTooLongException => new PathFormatException("the given path " + _path + " is invalid",
                        "HasAttribute"),
                    SecurityException or ArgumentException or UnauthorizedAccessException or NullReferenceException =>
                        new AccessException("the directory to access # " + _path + " # cannot be read", "HasAttribute"),
                    IOException => new DiskNotReadyException("the given path " + _path + " could not be read",
                        "HasAttribute"),
                    _ => new ManagerException("Reader error", Level.High, "Unable to read",
                        "the given path " + _path + " could not be read", "HasAttribute")
                };
            }
        }
    }
}