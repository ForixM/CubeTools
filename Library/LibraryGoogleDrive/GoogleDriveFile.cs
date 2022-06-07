﻿using System.Collections.Generic;
using System.IO;

namespace Library.LibraryGoogleDrive
{
    public class GoogleDriveFile : Pointer
    {
        protected string _id;
        protected string _descrpition;
        
        public string Id { get => _id; set => _id = value; }
        public string Description { get => _descrpition; set => _descrpition = value; }

        public GoogleDriveFile(string fileId)
        {
            _id = fileId;
            _type = FileReader.GetFileType(fileId);
            _name = FileReader.GetFileName(fileId);
            _size = FileReader.GetFileSize(fileId);
            _descrpition = FileReader.GetFileDescription(fileId);
            _path = FileReader.GetPathFromFile(fileId);
            IsDir = FileReader.IsDir(fileId);
        }
    }
}