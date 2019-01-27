using Autofac;
using Web3.Api.Addresses.Composition;
using Web3.Api.TokenHoldings.Composition;

namespace Web3.Api.Composition
{
    internal class Modules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<AddressesModule>();
            builder.RegisterModule<TokenHoldingsModule>();
            base.Load(builder);
        }
    }
}
