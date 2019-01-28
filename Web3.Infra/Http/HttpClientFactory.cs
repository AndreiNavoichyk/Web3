using System.Net.Http;

namespace Web3.Infra.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClientFactorySettings _settings;

        public HttpClientFactory(HttpClientFactorySettings settings)
        {
            _settings = settings;
        }

        public HttpClient Create()
        {
            return new HttpClient {Timeout = _settings.Timeout};
        }
    }
}