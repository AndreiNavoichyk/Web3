using Autofac;
using Web3.Core.Repositories;
using Web3.Core.TokenHoldings;
using Web3.Core.TokenHoldings.Models;
using Web3.InfuraRepository;

namespace Web3.Api.TokenHoldings.Composition
{
    internal class TokenHoldingsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<FromConfigTokensProvider>()
                .As<ITokensProvider>();
            builder
                .RegisterType<TokenHoldingsRepository>()
                .As<IRepository<TokenHoldingInfo, (string address, string tokenAddress)>>()
                .As<IQueryableRepository<TokenHoldingInfo, (string address, string tokenAddress)>>();
            builder
                .RegisterType<TokenHoldingsSettings>();
            
            base.Load(builder);
        }
    }
}