using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Web3.Core.TokenHoldings;
using Web3.Core.TokenHoldings.Models;

namespace Web3.Api.TokenHoldings
{
    internal class FromConfigTokensProvider : ITokensProvider
    {
        private readonly TokenInfo[] _tokenInfos;
        
        public FromConfigTokensProvider(IConfiguration configuration)
        {
            _tokenInfos = configuration
                .GetSection("Etherscan:Tokens")
                .Get<TokenInfo[]>();
        }
        
        public Task<IEnumerable<TokenInfo>> GetAsync(int number)
        {
            return Task.FromResult(_tokenInfos.Take(Math.Min(_tokenInfos.Length, number)));
        }
    }
}