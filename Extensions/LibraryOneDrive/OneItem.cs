using System.Runtime.Serialization;

namespace LibraryOneDrive
{
    public enum OneItemType
    {
        FOLDER,
        FILE
    }

    [DataContract]
    public class OneItem
    {
        [DataMember(Name = "name")] public string name { get; set; }

        [DataMember(Name = "size")] public long size { get; set; }

        [DataMember(Name = "id")] public string id { get; set; }

        public string path => parentReference.path + "/" + name;

        [DataMember(Name = "folder")] public OneFolder folder { get; set; }

        [DataMember(Name = "file")] public OneFile file { get; set; }

        public OneItemType Type => folder == null ? OneItemType.FILE : OneItemType.FOLDER;

        [DataMember(Name = "parentReference")] public ParentReference parentReference { get; set; }

        public override string ToString()
        {
            var disp = "name=" + name + "\nsize=" + size + "\n";
            if (folder != null)
                disp += folder.ToString();
            else if (file != null) disp += file.ToString();
            return disp;
        }

        [DataContract]
        public class ParentReference
        {
            [DataMember(Name = "driveId")] public string driveId { get; set; }

            [DataMember(Name = "driveType")] public string driveType { get; set; }

            [DataMember(Name = "id")] public string id { get; set; }

            [DataMember(Name = "path")] public string path { get; set; }
        }
    }
}