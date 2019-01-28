using System.Threading.Tasks;

namespace Web3.Core.Services.Exchange
{
    public class ExchangeService : IExchangeService
    {
        private readonly IExchanger _exchanger;

        public ExchangeService(IExchanger exchanger)
        {
            _exchanger = exchanger;
        }

        public async Task<ExchangeResult> ExchangeAsync(ExchangeValue from, string toSymbol)
        {
            var rate = await _exchanger.GetRateAsync(from.Symbol, toSymbol);

            return new ExchangeResult
            {
                From = from,
                To = new ExchangeValue
                {
                    Symbol = toSymbol,
                    Value = rate * from.Value
                }
            };
        }
    }
}