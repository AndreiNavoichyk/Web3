using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Web3.Core.Services.Exchange;
using Web3.Core.Services.Exchange.CryptoCompare;

namespace Web3.Api.Composition
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ExchangeService>()
                .As<IExchangeService>();
            builder
                .RegisterType<CryptoCompareSettings>()
                .WithParameter(
                    new ResolvedParameter(
                        (pi, cc) => pi.ParameterType == typeof(string),
                        (pi, cc) => cc.Resolve<IConfiguration>()["CryptoCompare:UrlFormat"]));
            builder
                .RegisterType<CryptoCompareExchanger>()
                .As<IExchanger>();

            base.Load(builder);
        }
    }
}
