using System.Collections.Generic;

namespace LibraryFTP;

public class FtpArboresence
{
    private List<FtpFile> _files;

    public List<FtpFile> Files => _files;

    public FtpArboresence()
    {
        _files = new List<FtpFile>();
    }

    public override string ToString()
    {
        string str = "";
        foreach (FtpFile ftpFile in _files)
        {
            str += ftpFile.ToString() + "\n";
        }

        return str;
    }
}