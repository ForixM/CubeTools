namespace Library.FilePointer
{
    /// <summary>
    /// ##################### FILE POINTER CLASS ######################### <br></br>
    /// ALL FILES ARE STORED THANKS TO INSTANCE OF THIS CLASS <br></br>
    /// CUBETOOLS SOFTWARE DEPENDS ON THIS CLASS WHEN WORKING WITH FILES <br></br>
    /// Inheritance : Pointer - x  <br></br>
    /// ##################################################################
    /// </summary>
    public partial class FilePointer : Pointer
    {
        #region Variables
        
        private FileInfo? _fileInfo;
        public FileInfo? FileInfo { get => _fileInfo; set => _fileInfo = value; }

        #endregion
        
        #region Init

        // Constructors
        public FilePointer() : base()
        {
            _fileInfo = null;
        }

        public FilePointer(string path) : base(path)
        {
            _fileInfo = new FileInfo(path);
        }

        #endregion
    }
}