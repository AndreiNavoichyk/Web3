using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Web3.Core.Models;
using Web3.Core.Repositories;
using Web3.Core.Repositories.Exceptions;

namespace Web3.InfuraRepository
{
    public class BalancesRepository : IBalancesRepository
    {
        private readonly Nethereum.Web3.Web3 _web;

        public BalancesRepository(Settings settings)
        {
            _web = new Nethereum.Web3.Web3(settings.Url);
        }

        public async Task<Balance> GetAsync(string key)
        {
            try
            {
                return await GetInternalAsync(key);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Can not get balance from Infura", ex);
            }
        }

        public Task<IEnumerable<Balance>> GetAllAsync(Expression<Func<Balance, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Balance entity)
        {
            throw new NotImplementedException();
        }

        public Task<Balance> UpdateAsync(Balance entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Balance entity)
        {
            throw new NotImplementedException();
        }

        private async Task<Balance> GetInternalAsync(string address)
        {
            var weiBalance = await _web.Eth.GetBalance.SendRequestAsync(address);
            var ethBalance = Nethereum.Web3.Web3.Convert.FromWei(weiBalance);
            var balance = new Balance
            {
                Address = address,
                Value = ethBalance
            };
            return balance;
        }
    }
}