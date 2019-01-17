using Nethereum.Util;

namespace Web3.Api.Balances
{
    internal class AddressValidator : IAddressValidator
    {
        public bool Validate(string address)
        {
            return AddressUtil.Current.IsValidEthereumAddressHexFormat(address);
        }
    }
}