namespace Web3.Core.Services.Exchange.CryptoCompare
{
    public class CryptoCompareSettings
    {
        public CryptoCompareSettings(string urlFormat)
        {
            UrlFormat = urlFormat;
        }

        public string UrlFormat { get; set; }
    }
}