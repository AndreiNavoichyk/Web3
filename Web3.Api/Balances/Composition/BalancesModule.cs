using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Web3.Core.Repositories;
using Web3.InfuraRepository;

namespace Web3.Api.Balances.Composition
{
    internal class BalancesModule : Module
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
                .RegisterType<BalancesRepository>()
                .As<IBalancesRepository>();

            base.Load(builder);
        }
    }
}