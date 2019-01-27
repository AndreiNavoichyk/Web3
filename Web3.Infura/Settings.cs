namespace Web3.Infura
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