namespace Web3.InfuraRepository
{
    public class Settings
    {
        public Settings(string url)
        {
            Url = url;
        }

        public string Url { get; }
    }
}