using System;
using System.Collections.Generic;
using CubeTools_UI.Views.Ftp;
using LibraryFTP;

namespace CubeTools_UI.Models.Ftp
{
    public class RemoteFTPModel
    {
        public MainWindowFTPModel ParentModel;
        public FtpArboresence Remote;
        public List<RemotePointer> SelectedRemote;
        public List<RemotePointer> CopiedRemote;
        public List<RemotePointer> CutRemote;

        public RemoteFTPModel(MainWindowFTPModel main, string path)
        {
            SelectedRemote = new List<RemotePointer>();
            CopiedRemote = new List<RemotePointer>();
            CutRemote = new List<RemotePointer>();
            ParentModel = main;
            try
            {
                Remote = main.Client.ListDirectory(new FtpFolder(path));
            }
            catch (Exception e)
            {
                //if (e is ManagerException @managerException)
                //new Views.ErrorPopUp.ErrorPopUp()
            }
        }
    }
}