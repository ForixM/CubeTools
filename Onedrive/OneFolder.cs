namespace Onedrive
{
    public class OneFolder
    {
        public int childCOunt
        {
            get;
            set;
        }

        public FolderView folderView
        {
            get;
            set;
        }

        public string folderType
        {
            get;
            set;
        }

        public override string ToString()
        {
            return "Folder";
        }

        public class FolderView
        {
            public string viewType
            {
                get;
                set;
            }

            public string sortBy
            {
                get;
                set;
            }

            public string sortOrder
            {
                get;
                set;
            }
        } 
    }
}