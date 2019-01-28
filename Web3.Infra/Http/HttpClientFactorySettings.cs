using System;

namespace Web3.Infra.Http
{
    public class HttpClientFactorySettings
    {
        public HttpClientFactorySettings(TimeSpan timeout)
        {
            Timeout = timeout;
        }

        public TimeSpan Timeout { get; set; }
    }
}