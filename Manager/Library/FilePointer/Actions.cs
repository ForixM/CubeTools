using Library.ManagerExceptions;
using Microsoft.VisualBasic.FileIO;

namespace Library.FilePointer
{
    public partial class FilePointer
    {
        // This region contains elementary action
        // Purpose : Destroy FilePointer using functions from the static library

        /// <summary>
        ///     - High Level => Delete a file using its associated FilePointer <br></br>
        ///     - Action : Delete a file <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <exception cref="InUseException">The given path is already used</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="ManagerException">An Error occurred</exception>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        public void Delete()
        {
            if (!Exist()) throw new PathNotFoundException(Path + " does not exist", "Delete");
            try
            {
                FileSystem.DeleteFile(Path, UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.ThrowException);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    IOException => new InUseException(Path + " is used by another process", "Delete"),
                    UnauthorizedAccessException => new AccessException(Path + " access is denied", "Delete"),
                    _ => new ManagerException("Delete is impossible", "Medium", "Writer Error",
                        Path + " could not be deleted", "Delete")
                };
            }
        }

    }
}