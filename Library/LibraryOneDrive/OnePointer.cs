using System.Runtime.Serialization;

namespace Library.LibraryOneDrive
{
    public enum OneItemType
    {
        FOLDER,
        FILE
    }

    [DataContract]
    public class OnePointer : Pointer
    {
        
        public static OnePointer ROOT = new()
        {
            name = "",
            size = 0,
            folder = new OneFolder(),
            root = new object(),
            parentReference = new ParentReference
            {
                path = "/drive/root:"
            }
        };

        [DataMember(Name = "name")] public string name { get; set; }

        [DataMember(Name = "size")] public long size { get; set; }

        [DataMember(Name = "id")] public string id { get; set; }

        public string path => parentReference.path + "/" + name;

        [DataMember(Name = "folder")] public OneFolder folder { get; set; }

        [DataMember(Name = "file")] public OneFile file { get; set; }

        public OneItemType Type => folder == null ? OneItemType.FILE : OneItemType.FOLDER;
        
        public new bool IsDir => Type == OneItemType.FOLDER;

        [DataMember(Name="root")]public Object root { get; set; }

        public bool isRoot => root != null;
        
        [DataMember(Name = "parentReference")] public ParentReference parentReference { get; set; }

        /// <summary>
        /// Call when getting the arboresence of a OneFolder
        /// </summary>
        public void SetVariables()
        {
            _name = name;
            _size = size;
            string extension = System.IO.Path.GetExtension(name);
            _type = extension == "" ? "" : extension.Remove(0, 1);
            base.IsDir = this.IsDir;
            _parentPath = isRoot ? "/drive/root:" : parentReference.path+"/";
            _path = isRoot ? "/drive/root:" : _parentPath+name+"/";
        }
        
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