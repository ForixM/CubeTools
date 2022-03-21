namespace LibraryOneDrive
{
    public class Token
    {
        public string token_type { get; set; }

        public int expires_in { get; set; }

        public string scope { get; set; }

        public string access_token { get; set; }

        public string refresh_token { get; set; }

        public string authentication_token { get; set; }

        public string user_id { get; set; }
    }
}