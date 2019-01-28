using Autofac;
using Web3.Core.TokenHoldings.Models;
using Web3.Infra.Repositories;
using Web3.Infura.TokenHoldings;

namespace Web3.Api.TokenHoldings.Composition
{
    internal class TokenHoldingsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<FromConfigTokenInfosRepository>()
                .As<IRepository<TokenInfo, string>>()
                .As<IQueryableRepository<TokenInfo, string>>();
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