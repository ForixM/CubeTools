namespace Library
{
    public partial class FilePointer : Pointer
    {

        #region Variables

        private FileInfo? _fileInfo;
        public FileInfo? FileInfo => _fileInfo;

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

        // Initializers
        // TODO Edit Load Size algorithm
        public void LoadSize()
        {
        }

        #endregion
    }
}