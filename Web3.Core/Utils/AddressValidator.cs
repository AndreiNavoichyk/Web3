using Nethereum.Util;

namespace Web3.Core.Utils
{
    public class AddressValidator : IAddressValidator
    {
        public bool Validate(string address)
        {
            return AddressUtil.Current.IsValidEthereumAddressHexFormat(address);
        }
    }
}