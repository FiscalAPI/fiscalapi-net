using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using FiscalApi.Abstractions;
using FiscalApi.Abstractions.Services;
using FiscalApi.Implementations;
using FiscalApi.OptionsHttp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
/* Unmerged change from project 'FiscalApi.Sdk (net6.0)'
Before:
using FiscalApi.Sdk.Abstractions;
After:
using FiscalApi;
using FiscalApi.Sdk;
using FiscalApi.Sdk;
using FiscalApi.Sdk.Abstractions;
*/

namespace FiscalApi
{
    public static class ApiExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            // Get the assembly where the extension is being called
            var assembly = Assembly.GetExecutingAssembly();

            // Register singleton services
            var singletonInterfaces = assembly.GetExportedTypes()
                .Where(t => t.IsInterface && typeof(IFiscalApiSingletonService).IsAssignableFrom(t));

            foreach (var interfaceType in singletonInterfaces)
            {
                var implementationType = assembly.GetTypes()
                    .FirstOrDefault(t => interfaceType.IsAssignableFrom(t) && t.IsClass);

                if (implementationType != null)
                {
                    services.AddSingleton(interfaceType, implementationType);
                }
            }


            // Register scoped services
            var scopedInterfaces = assembly.GetExportedTypes()
                .Where(t => t.IsInterface && typeof(IFiscalApiScopedService).IsAssignableFrom(t));

            foreach (var interfaceType in scopedInterfaces)
            {
                var implementationType = assembly.GetTypes()
                    .FirstOrDefault(t => interfaceType.IsAssignableFrom(t) && t.IsClass);

                if (implementationType != null)
                {
                    services.AddScoped(interfaceType, implementationType);
                }
            }


            // Register transient services
            var transientInterfaces = assembly.GetExportedTypes()
                .Where(t => t.IsInterface && typeof(IFiscalApiTransientService).IsAssignableFrom(t));

            foreach (var interfaceType in transientInterfaces)
            {
                var implementationType = assembly.GetTypes()
                    .FirstOrDefault(t => interfaceType.IsAssignableFrom(t) && t.IsClass);

                if (implementationType != null)
                {
                    services.AddTransient(interfaceType, implementationType);
                }
            }

            return services;
        }

        public static IServiceCollection AddFiscalApiClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Registrar FiscalApiClient con HttpClient configurado
            services.AddHttpClient<IFiscalApiClient, FiscalApiClient>((sp, client) =>
            {
                var settings = configuration.GetSection(nameof(FiscalApiSettings)).Get<FiscalApiSettings>();

                client.BaseAddress = new Uri(settings.ApiUrl);
                client.DefaultRequestHeaders.Add("X-API-KEY", settings.ApiKey);
                client.DefaultRequestHeaders.Add("X-TENANT-KEY", settings.Tenant);
                client.DefaultRequestHeaders.Add("X-API-VERSION", settings.ApiVersion);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            });


            services.AddCoreServices();

            return services;
        }


        /// <summary>
        /// Encode string to base64
        /// </summary>
        /// <param name="plainText">string to encode</param>
        /// <returns>base64 encoded string</returns>
        public static string EncodeToBase64(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Decode string to plainText
        /// </summary>
        /// <param name="base64EncodedData">base64 encoded data to decode</param>
        /// <returns>plainText</returns>
        public static string DecodeFromBase64(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}