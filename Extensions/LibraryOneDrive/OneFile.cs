namespace LibraryOneDrive
{
    public class OneFile
    {
        public Hashes hashes { get; set; }

        public string mimeType { get; set; }

        public override string ToString()
        {
            return "File";
        }

        public class Hashes
        {
            public string quickXorHash { get; set; }

            public string sha1Hash { get; set; }

            public string sha256Hash { get; set; }
        }
    }
}