namespace Web3.Api.Balances
{
    public interface IAddressValidator
    {
        bool Validate(string address);
    }
}