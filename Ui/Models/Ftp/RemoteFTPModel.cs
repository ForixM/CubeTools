using System;
using System.Collections.Generic;
using Library.ManagerExceptions;
using LibraryFTP;
using Ui.Views.ErrorPopUp;
using Ui.Views.Ftp;

namespace Ui.Models.Ftp
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
                RemoteDirectory = FtpFolder.ROOT;
                Children = main.Client.ListDirectory(RemoteDirectory);
            }
            catch (Exception e)
            {
                new NormalErrorPopUp(new ManagerException("Invalid Credentials", "Low-Critical", "CubeTools crashed", "Username or Password are incorrect")).Show();
            }
        }
    }
}