namespace LibraryFTP;

public class FtpFile
{
    private string _name;
    private int _size;

    public string Name => _name;
    public int Size => _size;
    
    public FtpFile(string name, int size)
    {
        this._name = name;
        this._size = size;
    }

    public override string ToString()
    {
        return $"{{name={_name}, size={_size}}}";
    }
}