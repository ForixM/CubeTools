using System;
using System.Collections.Generic;
using CubeTools_UI.Views;
using CubeTools_UI.Views.Ftp;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.Pointers;
using ReactiveUI;

namespace CubeTools_UI.Models
{
    public class NavigationBarModel
    {
        
        #region Models variables
        
        // A pointer to the current loaded Directory
        private DirectoryType _directoryPointer;
        public DirectoryType DirectoryPointer
        {
            get => _directoryPointer;
            set => _directoryPointer = value;
        }
        
        // Queue Pointers : Pointers registered in a queue
        private List<string> _queuePointers;
        public List<string> QueuePointers
        {
            get => _queuePointers;
            set => _queuePointers = value;
        }
        
        // Index Queue : the current index of the queue
        private int _queueIndex;
        public int QueueIndex
        {
            get => _queueIndex;
            set => _queueIndex = value;
        }
        
        #endregion
        
        #region References
        
        public NavigationBar View;
        public LocalModel? ParentModel;
        
        #endregion

        // CTOR
        public NavigationBarModel(NavigationBar view)
        {
            View = view;
            
            _directoryPointer = new DirectoryType();
            _queuePointers = new List<string>();
            _queueIndex = -1;
        }
        
    }
}
