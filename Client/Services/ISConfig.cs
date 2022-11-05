namespace Client.Services
{
    public class ISConfig
    {
        public string Url { get; set; }
        public string ClientName { get; set; }
        public string ClientPassword { get; set; }
        public bool UsingHttps { get; set; }
    }
}
