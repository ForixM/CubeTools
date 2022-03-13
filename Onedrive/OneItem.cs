using System.Runtime.Serialization;

namespace Onedrive
{
    public enum OneItemType
    {
        FOLDER,
        FILE
    }
    
    [DataContract]
    public class OneItem
    {
        [DataMember(Name="name")]
        public string name
        {
            get;
            set;
        }

        [DataMember(Name="size")]
        public int size
        {
            get;
            set;
        }

        [DataMember(Name="id")]
        public string id
        {
            get;
            set;
        }

        [DataMember(Name="folder")]
        public OneFolder folder
        {
            get;
            set;
        }

        [DataMember(Name="file")]
        public OneFile file
        {
            get;
            set;
        }

        public OneItemType Type
        {
            get => folder == null ? OneItemType.FILE : OneItemType.FOLDER;
        }

        [DataMember(Name="parentReference")]
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

        [DataContract]
        public class ParentReference
        {
            
            [DataMember(Name="driveId")]
            public string driveId
            {
                get;
                set;
            }

            [DataMember(Name="driveType")]
            public string driveType
            {
                get;
                set;
            }

            [DataMember(Name="id")]
            public string id
            {
                get;
                set;
            }

            [DataMember(Name="path")]
            public string path
            {
                get;
                set;
            }
        }
    }
}