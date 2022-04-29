using System;
using System.Collections.Generic;
using CubeTools_UI.Views.Ftp;
using Library.Pointers;

namespace CubeTools_UI.Models.Ftp
{
    public class LocalFTPModel
    {
        public MainWindowFTPModel ParentModel;
        public DirectoryType Local;
        public List<LocalPointer> SelectedLocal;
        public List<LocalPointer> CopiedLocal;
        public List<LocalPointer> CutLocal;

        public LocalFTPModel(MainWindowFTPModel main, string path)
        {
            SelectedLocal = new List<LocalPointer>();
            CopiedLocal = new List<LocalPointer>();
            CutLocal = new List<LocalPointer>();
            ParentModel = main;
            try
            {
                Local = new DirectoryType(path);
            }
            catch (Exception e)
            {
                //if (e is ManagerException @managerException)
                //new Views.ErrorPopUp.ErrorPopUp()
            }
        }
    }
}