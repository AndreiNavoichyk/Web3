using Web3.Core.Addresses.Models;
using Web3.Infra.Repositories;

namespace Web3.Core.Addresses
{
    public interface IAddressesRepository : IRepository<AddressInfo, string>
    {
    }
}