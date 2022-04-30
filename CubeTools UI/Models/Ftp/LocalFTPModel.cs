using System;
using System.Collections.Generic;
using CubeTools_UI.Views.Ftp;
using Library.Pointers;

namespace CubeTools_UI.Models.Ftp
{
    public class LocalFTPModel
    {
        public MainWindowFTPModel ParentModel;
        private LocalFTP _view;
        
        public DirectoryType LocalDirectory;
        
        public List<LocalPointer> Selected;
        public List<LocalPointer> Copied;
        public List<LocalPointer> Cut;

        public LocalFTPModel(MainWindowFTPModel main, LocalFTP view, string path)
        {
            //
            Selected = new List<LocalPointer>();
            Copied = new List<LocalPointer>();
            Cut = new List<LocalPointer>();
            //
            ParentModel = main;
            _view = view;
            try
            {
                LocalDirectory = new DirectoryType(path);
            }
            catch (Exception e)
            {
                //if (e is ManagerException @managerException)
                //new Views.ErrorPopUp.ErrorPopUp()
            }
        }
    }
}