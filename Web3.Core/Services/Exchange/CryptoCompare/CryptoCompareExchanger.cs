using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Web3.Infra.Exceptions;
using Web3.Infra.Http;

namespace Web3.Core.Services.Exchange.CryptoCompare
{
    public class CryptoCompareExchanger : IExchanger
    {
        private readonly CryptoCompareSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public CryptoCompareExchanger(
            CryptoCompareSettings settings,
            IHttpClientFactory httpClientFactory)
        {
            _settings = settings;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<decimal> GetRateAsync(string fromSymbol, string toSymbol)
        {
            try
            {
                return await GetRateInternalAsync(fromSymbol, toSymbol);
            }
            catch (Exception ex)
            {
                throw new AppException($"Can not get rate from:{fromSymbol} to:{toSymbol}", ex);
            }
        }

        private async Task<decimal> GetRateInternalAsync(string fromSymbol, string toSymbol)
        {
            using (var httpClient = _httpClientFactory.Create())
            {
                var responseMessage =
                    await httpClient.GetAsync(new Uri(string.Format(_settings.UrlFormat, fromSymbol, toSymbol)));

                responseMessage.EnsureSuccessStatusCode();

                var rate = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(
                    await responseMessage.Content.ReadAsStringAsync());

                return rate[toSymbol];
            }
        }
    }
}