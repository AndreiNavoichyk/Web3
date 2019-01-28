using System;
using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Web3.Infra.Http;

namespace Web3.Api.Composition
{
    internal class HttpModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<HttpClientFactorySettings>()
                .WithParameter(
                    new ResolvedParameter(
                        (pi, cc) => pi.ParameterType == typeof(TimeSpan),
                        (pi, cc) => TimeSpan.FromMilliseconds(cc.Resolve<IConfiguration>().GetSection("Http:TimeoutMs").Get<long>())));
            builder
                .RegisterType<HttpClientFactory>()
                .As<IHttpClientFactory>();

            base.Load(builder);
        }
    }
}
