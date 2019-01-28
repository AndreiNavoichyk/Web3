using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Web3.Core.TokenHoldings.Models;
using Web3.Infra.Repositories;
using Web3.Infra.Repositories.Exceptions;
using Web3.SmartContracts;

namespace Web3.Infura.TokenHoldings
{
    public class TokenHoldingsRepository : RepositoryBase<TokenHoldingInfo, (string address, string tokenAddress)>
    {
        private readonly Nethereum.Web3.Web3 _web3;
        
        public TokenHoldingsRepository(Settings settings)
        {
            _web3 = new Nethereum.Web3.Web3(settings.Url);
        }
        
        public override async Task<TokenHoldingInfo> GetAsync((string address, string tokenAddress) key)
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

        protected override Task<IQueryable<TokenHoldingInfo>> GetAllInternalAsync()
        {
            throw new NotImplementedException();
        }

        public override Task AddAsync(TokenHoldingInfo entity)
        {
            throw new NotImplementedException();
        }

        public override Task<TokenHoldingInfo> UpdateAsync(TokenHoldingInfo entity)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteAsync(TokenHoldingInfo entity)
        {
            throw new NotImplementedException();
        }
        
        private async Task<TokenHoldingInfo> GetInternalAsync((string address, string tokenAddress) key)
        {
            var balanceOfFunction = new BalanceOfFunction
            {
                Owner = key.address
            };
            var balanceHandler = _web3.Eth.GetContractQueryHandler<BalanceOfFunction>();
            var weiBalance = await balanceHandler.QueryAsync<BigInteger>(key.tokenAddress, balanceOfFunction);

            return new TokenHoldingInfo
            {
                Address = key.tokenAddress,
                Value = Nethereum.Web3.Web3.Convert.FromWei(weiBalance)
            };
        }
    }
}