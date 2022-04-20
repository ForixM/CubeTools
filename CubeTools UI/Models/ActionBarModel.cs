using System;
using System.Collections.Generic;
using Library.Pointers;

namespace CubeTools_UI.Models
{
    
    public class ActionBarModel
    {
        // Main Model
        private MainWindowModel _model;
        // VARIABLES
        private List<FileType> _selected;
        private List<FileType> _copied;
        private List<FileType> _cut;
        public List<FileType> SelectedXaml
        {
            get => _selected;
            set => _selected = value;
        }
        public List<FileType> CopiedXaml
        {
            get => _copied;
            set => _copied = value;
        }

        public List<FileType> CutXaml
        {
            get => _cut;
            set => _cut = value;
        }

        // CTOR
        public ActionBarModel(MainWindowModel model)
        {
            _model = model;
            _selected = new List<FileType>();
            _copied = new List<FileType>();
            _cut = new List<FileType>();
        }
    }
}
