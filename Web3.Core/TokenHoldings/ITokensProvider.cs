using System.Collections.Generic;
using System.Threading.Tasks;
using Web3.Core.TokenHoldings.Models;

namespace Web3.Core.TokenHoldings
{
    public interface ITokensProvider
    {
        Task<IEnumerable<TokenInfo>> GetAsync(int number);
    }
}