using Microsoft.Extensions.Configuration;

namespace Web3.Api.TokenHoldings
{
    public class TokenHoldingsSettings
    {
        public TokenHoldingsSettings(IConfiguration configuration)
        {
            TopTokensNumber = configuration.GetSection("TokenHoldings:TopTokensNumber").Get<int>();
        }
        
        public int TopTokensNumber { get; }
    }
}