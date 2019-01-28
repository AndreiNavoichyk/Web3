using Autofac;
using Web3.Core.Utils;

namespace Web3.Api.Composition
{
    internal class UtilsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<AddressValidator>()
                .As<IAddressValidator>();

            base.Load(builder);
        }
    }
}