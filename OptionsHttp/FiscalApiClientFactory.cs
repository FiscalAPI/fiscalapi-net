using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FiscalApi.OptionsHttp
{
    public static class FiscalApiClientFactory
    {
        private static readonly ConcurrentDictionary<string, Lazy<HttpClient>> Clients =
            new ConcurrentDictionary<string, Lazy<HttpClient>>();

        public static HttpClient Create(FiscalApiSettings settings)
        {
            var clientKey = settings.ApiKey;

            var lazyClient = Clients.GetOrAdd(clientKey, new Lazy<HttpClient>(() =>
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(settings.ApiUrl),
                    DefaultRequestHeaders =
                    {
                        { "X-API-KEY", settings.ApiKey },
                        { "X-TENANT-KEY", settings.Tenant },
                        { "X-API-VERSION", settings.ApiVersion }
                    }
                };

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client;
            }));

            return lazyClient.Value;
        }
    }
}