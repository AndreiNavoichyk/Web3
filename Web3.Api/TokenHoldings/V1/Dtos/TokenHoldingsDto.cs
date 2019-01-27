using System.Collections.Generic;

namespace Web3.Api.TokenHoldings.V1.Dtos
{
    internal class TokenHoldingsDto
    {
        public string Address { get; set; }
        public List<TokenHoldingInfoDto> Tokens { get; set; }
    }
}