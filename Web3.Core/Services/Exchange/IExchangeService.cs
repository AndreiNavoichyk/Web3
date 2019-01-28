using System.Threading.Tasks;

namespace Web3.Core.Services.Exchange
{
    public interface IExchangeService
    {
        Task<ExchangeResult> ExchangeAsync(ExchangeValue from, string toSymbol);
    }
}
