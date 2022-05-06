using System.Collections.Generic;
using LibraryFTP;

namespace CubeTools_UI.Models;

public class FTPNavigationBarModel
{
    private int index;
    private List<FtpFolder> _queue;
    
    public int Index { get; set; }
    public List<FtpFolder> Queue { get; set; }

    public FTPNavigationBarModel()
    {
        index = -1;
        _queue = new List<FtpFolder>();
    }
}