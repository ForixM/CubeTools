using System.Collections.Generic;
using Library;
using Library.DirectoryPointer;
using Library.DirectoryPointer.DirectoryPointerLoaded;
using Ui.Views;

namespace Ui.Models
{
    public class NavigationBarModel
    {
        
        #region Models variables
        
        // A pointer to the current loaded Directory
        private DirectoryPointerLoaded _folderPointer;
        public DirectoryPointerLoaded FolderPointer
        {
            get => _folderPointer;
            set => _folderPointer = value;
        }
        
        // Queue Pointers : Pointers registered in a queue
        private List<DirectoryPointer> _queuePointers;
        public List<DirectoryPointer> QueuePointers
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
        
        public void Add(DirectoryPointer folder)
        {
            if (_queuePointers.Count - 1 == _queueIndex || _queueIndex < 0)
            {
                _queuePointers.Add(folder);
            }
            else if (_queuePointers.Count > _queueIndex + 1 && folder != _queuePointers[_queueIndex + 1])
            {
                _queuePointers.RemoveRange(_queueIndex + 1, _queuePointers.Count - _queueIndex - 1);
                _queuePointers.Add(folder);
            }

            _queueIndex++;
        }

        // CTOR
        public NavigationBarModel(NavigationBar view)
        {
            View = view;
            
            _folderPointer = new DirectoryPointerLoaded();
            _queuePointers = new List<DirectoryPointer>();
            _queueIndex = -1;
        }
        
    }
}
