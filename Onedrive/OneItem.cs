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
    }
}