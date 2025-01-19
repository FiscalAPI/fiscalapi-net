using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.Http.Headers;
using Fiscalapi.Common;

namespace Fiscalapi.Http
{
    public static class FiscalApiHttpClientFactory
    {
        private static readonly ConcurrentDictionary<string, Lazy<HttpClient>> Clients =
            new ConcurrentDictionary<string, Lazy<HttpClient>>();

        public static IFiscalApiHttpClient Create(FiscalapiSettings options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            var clientKey = $"{options.ApiKey}:{options.Tenant}:{options.ApiUrl}";

            var httpClient = Clients.GetOrAdd(clientKey, _ => new Lazy<HttpClient>(() =>
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(options.ApiUrl),
                    Timeout = TimeSpan.FromSeconds(30)
                };

                client.DefaultRequestHeaders.Add("X-API-KEY", options.ApiKey);
                client.DefaultRequestHeaders.Add("X-TENANT-KEY", options.Tenant);
                client.DefaultRequestHeaders.Add("X-API-VERSION", options.ApiVersion);
                client.DefaultRequestHeaders.Add("X-TIMEZONE", options.TimeZone);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return client;
            })).Value;

            return new FiscalApiHttpClient(httpClient);
        }
    }
}