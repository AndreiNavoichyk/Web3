using Autofac;
using Web3.Api.Balances.Composition;

namespace Web3.Api.Composition
{
    internal class Modules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<BalancesModule>();
            base.Load(builder);
        }
    }
}
