using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Web3.Core.Addresses;
using Web3.InfuraRepository;
using Web3.InfuraRepository.Addresses;

namespace Web3.Api.Addresses.Composition
{
    internal class AddressesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<Settings>()
                .WithParameter(
                    new ResolvedParameter(
                        (pi, cc) => pi.ParameterType == typeof(string),
                        (pi, cc) => cc.Resolve<IConfiguration>()["Infura:Url"]))
                .AsSelf();
            builder
                .RegisterType<AddressesRepository>()
                .As<IAddressesRepository>();
            builder
                .RegisterType<AddressValidator>()
                .As<IAddressValidator>();

            base.Load(builder);
        }
    }
}