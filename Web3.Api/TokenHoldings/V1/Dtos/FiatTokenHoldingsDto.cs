namespace Web3.Api.TokenHoldings.V1.Dtos
{
    internal class FiatTokenHoldingsDto
    {
        public string Address { get; set; }
        public TokenHoldingInfoDto Token { get; set; }
        public FiatDto Fiat { get; set; }
    }
}