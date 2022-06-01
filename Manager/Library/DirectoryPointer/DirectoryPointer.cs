using System.Security;
using Library.ManagerExceptions;

namespace Library.DirectoryPointer
{
    /// <summary>
    ///     This class represents a pointer to a loaded directory
    ///     It will managed our file tree and will use actions to modify files efficiently
    /// </summary>
    public partial class DirectoryPointer : Pointer
    {
        #region Variables

        protected DirectoryInfo? _directoryInfo;
        public DirectoryInfo? DirectoryInfo { get => _directoryInfo; set => _directoryInfo = value; }

        #endregion

        #region Init

        // This region will generate correctly the DirectoryPointer with FileTypes
        
        
        /// <summary>
        ///     - Action : Default constructor of DirectoryPointer class
        ///     - Implementation : Check
        /// </summary>
        public DirectoryPointer() : base()
        {
            Type = "";
            _directoryInfo = null;
        }

        /// <summary>
        ///     - Action : Load a pointer to a directory given in the parameter with its path
        ///     - Implementation : Check
        /// </summary>
        /// <param name="path">the path of the directory</param>
        /// <exception cref="AccessException">the given directory cannot be accessed</exception>
        /// <exception cref="SystemErrorException">The system blocked the constructor</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public DirectoryPointer(string path) : base(path)
        {
            if (!Directory.Exists(path) || string.IsNullOrEmpty(path))
                throw new PathNotFoundException(path + " could not be identified", "Directory Constructor");

            Type = "";
            try
            {
                Directory.SetCurrentDirectory(path);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    IOException => new SystemErrorException(path + " has been blocked by system", "Directory Constructor"),
                    SecurityException => new AccessException(path + " could be not accessed", "Directory Constructor"),
                    _ => new ManagerException("ManagerException", "High", "GenerateDirectory",
                        "Generate directory was impossible", "Directory Constructor")
                };
            }
            try
            {
                _directoryInfo = new DirectoryInfo(path);
            }
            catch (Exception e)
            {
                if (e is ManagerException) Console.WriteLine("# ManagerException occured");
            }
        }

        #endregion
        
        #region Loader

        public DirectoryPointerLoaded.DirectoryPointerLoaded LoadDirectoryPointer()
        {
            var dpl = new DirectoryPointerLoaded.DirectoryPointerLoaded(Path);
            Dispose();
            return dpl;
        }

        #endregion
        
    }
}