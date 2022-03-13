namespace Onedrive
{
    public class OneItem
    {
        public string name
        {
            get;
            set;
        }

        public int size
        {
            get;
            set;
        }

        public OneFolder folder
        {
            get;
            set;
        }

        public OneFile file
        {
            get;
            set;
        }

        public ParentReference parentReference
        {
            get;
            set;
        }

        public override string ToString()
        {
            string disp = "name="+name+"\nsize="+size+"\n";
            if (folder != null)
            {
                disp += folder.ToString();
            }
            else if (file != null)
            {
                disp += file.ToString();
            }
            return disp;
        }

        public class ParentReference
        {
            public string driveId
            {
                get;
                set;
            }

            public string driveType
            {
                get;
                set;
            }

            public string id
            {
                get;
                set;
            }

            public string path
            {
                get;
                set;
            }
        }
    }
}