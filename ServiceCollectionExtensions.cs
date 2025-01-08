using System;
using System.Text;
using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FiscalApi
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFiscalApi(
            this IServiceCollection services,
            Action<FiscalApiOptions> configureSettings)
        {
            services.Configure(configureSettings);

            services.AddSingleton<IFiscalApiClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<FiscalApiOptions>>().Value;
                return FiscalApiClient.Create(settings);
            });

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