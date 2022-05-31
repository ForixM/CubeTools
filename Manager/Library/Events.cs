using System.Globalization;
using Library.ManagerExceptions;
namespace Library
{
    public partial class Pointer
    {
        // This region contains every functions that give information of a pointer (DATE)

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Get the date of creation with a given path <br></br>
        ///     - Implementation : Check <br></br>
        /// </summary>
        /// <returns>the creation date</returns>
        /// <exception cref="AccessException">The pointer cannot be read</exception>
        /// <exception cref="PathNotFoundException">The pointer does not exist</exception>
        /// <exception cref="CorruptedPointerException">The given pointer is corrupted</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public string GetPointerCreationDate()
        {
            if (!Exist())
            {
                if (!_isDir) throw new CorruptedPointerException("Pointer of the file " + _path + " is corrupted", "GetFileCreationDate"); // TODO Edit exception for file
                throw new CorruptedDirectoryException("Pointer of the folder " + _path + " is corrupted", "GetPointerCreationDate");
            }

            try
            {
                return _isDir 
                    ? Directory.GetCreationTime(_path).ToString(CultureInfo.CurrentCulture)
                    : File.GetCreationTime(_path).ToString(CultureInfo.CurrentCulture);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    UnauthorizedAccessException => new AccessException(_path + " cannot be accessed",
                        "GetPointerCreationDate"),
                    PathTooLongException or ArgumentException or ArgumentNullException => new PathFormatException(_path + " has an invalid format", "GetPointerCreationDate"),
                    _ => new ManagerException("Reader error", "High", "Unable to read",
                        _path + " could not be read", "GetPointerCreationDate")
                };
            }
        }

        /// <summary>
        ///     - Type : High Level Method
        ///     - Action : Get the date of last edition with a given path <br></br>
        ///     - Implementation : Check <br></br>
        /// </summary>
        /// <returns>the last edition date</returns>
        /// <exception cref="AccessException">The pointer cannot be read</exception>
        /// <exception cref="PathNotFoundException">The pointer does not exist</exception>
        /// <exception cref="CorruptedPointerException">The given pointer is corrupted</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public string GetPointerLastEdition()
        {
            if (!Exist())
            {
                if (!_isDir) throw new CorruptedPointerException("Pointer of the file " + _path + " is corrupted", "GetPointerLastEdition"); // TODO Edit exception for file
                throw new CorruptedDirectoryException("Pointer of the folder " + _path + " is corrupted", "GetPointerLastEdition");
            }

            try
            {
                return _isDir 
                    ? Directory.GetLastWriteTime(_path).ToString(CultureInfo.CurrentCulture)
                    : File.GetLastWriteTime(_path).ToString(CultureInfo.CurrentCulture);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    UnauthorizedAccessException => 
                        new AccessException(_path + " cannot be accessed", "GetPointerLastEdition"),
                    PathTooLongException or ArgumentException or ArgumentNullException => 
                        new PathFormatException(_path + " has an invalid format", "GetPointerLastEdition"),
                    _ => 
                        new ManagerException("Reader error", "High", "Unable to read", _path + " could not be read", "GetPointerLastEdition")
                };
            }
        }

        /// <summary>
        ///     - Type : High Level Method
        ///     - Action : Get the date of access with a given path<br></br>
        ///     - Implementation : Check <br></br>
        /// </summary>
        /// <returns>the last edition date</returns>
        /// <exception cref="AccessException">The pointer cannot be read</exception>
        /// <exception cref="PathNotFoundException">The pointer does not exist</exception>
        /// <exception cref="CorruptedPointerException">The given pointer is corrupted</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public string GetPointerAccessDate()
        {
            if (!Exist())
            {
                if (!_isDir) throw new CorruptedPointerException("Pointer of the file " + _path + " is corrupted", "GetPointerAccessDate"); // TODO Edit exception for file
                throw new CorruptedDirectoryException("Pointer of the folder " + _path + " is corrupted", "GetPointerAccessDate");
            }

            try
            {
                return _isDir 
                    ? Directory.GetLastAccessTime(_path).ToString(CultureInfo.CurrentCulture)
                    : File.GetLastAccessTime(_path).ToString(CultureInfo.CurrentCulture);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    UnauthorizedAccessException => 
                        new AccessException(_path + " cannot be accessed", "GetPointerAccessDate"),
                    PathTooLongException or ArgumentException or ArgumentNullException => 
                        new PathFormatException(_path + " has an invalid format", "GetPointerAccessDate"),
                    _ => 
                        new ManagerException("Reader error", "High", "Unable to read", _path + " could not be read", "GetPointerAccessDate")
                };
            }
        }
    }
}