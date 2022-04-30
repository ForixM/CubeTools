using System;
using System.Collections.Generic;
using CubeTools_UI.Views.Ftp;
using LibraryFTP;

namespace CubeTools_UI.Models.Ftp
{
    public class RemoteFTPModel
    {
        public MainWindowFTPModel ParentModel;
        private RemoteFTP _view;
        //
        public FtpFolder RemoteDirectory;
        public FtpArboresence Children;
        //
        public List<RemotePointer> Selected;
        public List<RemotePointer> Copied;
        public List<RemotePointer> Cut;

        public RemoteFTPModel(MainWindowFTPModel main, RemoteFTP view, string path)
        {
            Selected = new List<RemotePointer>();
            Copied = new List<RemotePointer>();
            Cut = new List<RemotePointer>(); 
            ParentModel = main;
            _view = view;
            try
            {
                RemoteDirectory = new FtpFolder(path);
                Children = main.Client.ListDirectory(RemoteDirectory);
            }
            catch (Exception e)
            {
                //if (e is ManagerException @managerException)
                //new Views.ErrorPopUp.ErrorPopUp()
            }
        }
    }
}