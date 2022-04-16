using System;
using System.Collections.Generic;
using Library.Pointers;
using ReactiveUI;

namespace CubeTools_UI.Models
{
    public class NavigationBarModel
    {
        private MainWindowModel _model;
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
            set => value = _queueIndex;
        }
        
        // CTOR
        public NavigationBarModel(MainWindowModel model)
        {
            _model = model;
            _directoryPointer = new DirectoryType();
            _queuePointers = new List<string>();
            _queueIndex = -1;
        }
    }
}
