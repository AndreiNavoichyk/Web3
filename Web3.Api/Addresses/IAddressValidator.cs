namespace Web3.Api.Addresses
{
    public interface IAddressValidator
    {
        bool Validate(string address);
    }
}