using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Web3.Core.Addresses;
using Web3.Core.Addresses.Models;
using Web3.Infra.Repositories.Exceptions;

namespace Web3.InfuraRepository.Addresses
{
    public class AddressesRepository : IAddressesRepository
    {
        private readonly Nethereum.Web3.Web3 _web;

        public AddressesRepository(Settings settings)
        {
            _web = new Nethereum.Web3.Web3(settings.Url);
        }

        public async Task<AddressInfo> GetAsync(string key)
        {
            try
            {
                return await GetInternalAsync(key);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Can not get info from Infura", ex);
            }
        }

        public Task<IEnumerable<AddressInfo>> GetAllAsync(Expression<Func<AddressInfo, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(AddressInfo entity)
        {
            throw new NotImplementedException();
        }

        public Task<AddressInfo> UpdateAsync(AddressInfo entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(AddressInfo entity)
        {
            throw new NotImplementedException();
        }

        private async Task<AddressInfo> GetInternalAsync(string address)
        {
            var weiBalance = await _web.Eth.GetBalance.SendRequestAsync(address);
            var ethBalance = Nethereum.Web3.Web3.Convert.FromWei(weiBalance);
            var balance = new AddressInfo
            {
                Id = address,
                Value = ethBalance
            };
            return balance;
        }
    }
}