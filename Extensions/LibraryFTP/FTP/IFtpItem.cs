namespace LibraryFTP;

public abstract class IFtpItem
{
    protected string _parentPath;
    protected string _name;
    
    
    public string Name => _name;
    public string ParentPath => _parentPath;
    public string Path => _parentPath + _name;
}