using System.Net.Http;

namespace Web3.Infra.Http
{
    public interface IHttpClientFactory
    {
        HttpClient Create();
    }
}
