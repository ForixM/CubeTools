using System.Security;
using Library.ManagerExceptions;
namespace Library
{
    public abstract partial class Pointer
    {
        // This region contains every functions that give information of a pointer (ATTRIBUTES)

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Verify whether pointer is hidden  <br></br>
        ///     - Implementation : CHECK  <br></br>
        /// </summary>
        /// <returns>Whether it is hidden</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given pointer cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given pointer does not exist</exception>
        /// <exception cref="CorruptedPointerException">The pointer is corrupted</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public bool IsHidden() => HasAttribute(FileAttributes.Hidden);
        
        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Verify whether a pointer is compressed <br></br>
        ///     - Implementation : CHECK  <br></br>
        /// </summary>
        /// <returns>Whether it is compressed or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="CorruptedPointerException">The pointer is corrupted</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public bool IsCompressed() => HasAttribute(FileAttributes.Compressed);

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Verify whether a file or a directory is archived  <br></br>
        ///     - Implementation : CHECK  <br></br>
        /// </summary>
        /// <returns>Whether it is archived or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="CorruptedPointerException">The pointer is corrupted</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public bool IsArchived() => HasAttribute(FileAttributes.Archive);

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Verify whether a file or directory is part of a the system  <br></br>
        ///     - Implementation : Check  <br></br>
        /// </summary>
        /// <returns>If the file/folder is part of system or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="CorruptedPointerException">The pointer is corrupted</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public bool IsASystemFile() => HasAttribute(FileAttributes.System);

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Verify whether a file or directory is in readOnly  <br></br>
        ///     - Implementation : CHECK  <br></br>
        /// </summary>
        /// <returns>If the file is in readOnly</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="CorruptedPointerException">The pointer is corrupted</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public bool IsReadOnly() => HasAttribute(FileAttributes.ReadOnly);

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
        public abstract bool HasAttribute(FileAttributes fa);
    }
}