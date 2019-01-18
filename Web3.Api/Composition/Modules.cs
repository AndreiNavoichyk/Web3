using Autofac;
using Web3.Api.Addresses.Composition;

namespace Web3.Api.Composition
{
    internal class Modules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<AddressesModule>();
            base.Load(builder);
        }
    }
}
