using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Web3.Core.TokenHoldings.Models;
using Web3.Infra.Repositories;

namespace Web3.Api.TokenHoldings
{
    internal class FromConfigTokenInfosRepository : RepositoryBase<TokenInfo, string>
    {
        private readonly TokenInfo[] _tokenInfos;

        public FromConfigTokenInfosRepository(IConfiguration configuration)
        {
            _tokenInfos = configuration
                .GetSection("Etherscan:Tokens")
                .Get<TokenInfo[]>();
        }

        public override Task<TokenInfo> GetAsync(string key)
        {
            return Task.FromResult(_tokenInfos.First(ti => ti.Address == key));
        }

        protected override Task<IQueryable<TokenInfo>> GetAllInternalAsync()
        {
            return Task.FromResult(_tokenInfos.AsQueryable());
        }

        public override Task AddAsync(TokenInfo entity)
        {
            throw new NotImplementedException();
        }

        public override Task<TokenInfo> UpdateAsync(TokenInfo entity)
        {
            throw new NotImplementedException();
        }

        public override Task DeleteAsync(TokenInfo entity)
        {
            throw new NotImplementedException();
        }
    }
}