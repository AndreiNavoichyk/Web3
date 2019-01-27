using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Web3.Core.Addresses.Models;
using Web3.Core.Utils;
using Web3.Infra.Repositories;
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
                .As<IRepository<AddressInfo, string>>()
                .As<IQueryableRepository<AddressInfo, string>>();
            builder
                .RegisterType<AddressValidator>()
                .As<IAddressValidator>();

            base.Load(builder);
        }
    }
}