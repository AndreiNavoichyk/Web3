using System.Threading.Tasks;

namespace Web3.Core.Services.Exchange
{
    public interface IExchanger
    {
        Task<decimal> GetRateAsync(string from, string to);
    }
}