using System;
using FiscalApi.Abstractions;
using FiscalApi.Http;
using FiscalApi.Models.Common;

namespace FiscalApi.Services
{
    // Core Fiscalapi service
    public class FiscalApiClient : IFiscalApiClient
    {
        public IInvoiceService Invoices { get; }
        public IProductService Products { get; }


        private FiscalApiClient(FiscalApiOptions settings)
        {
            var httpClient = FiscalApiHttpClientFactory.Create(settings);
            var apiVersion = settings.ApiVersion;

            // Initialize services
            Invoices = new InvoiceService(httpClient, apiVersion);
            Products = new ProductService(httpClient, apiVersion);
        }

        public static IFiscalApiClient Create(FiscalApiOptions settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            ValidateSettings(settings);
            return new FiscalApiClient(settings);
        }


        private static void ValidateSettings(FiscalApiOptions settings)
        {
            if (string.IsNullOrEmpty(settings.ApiUrl))
                throw new ArgumentException("ApiUrl is required", nameof(settings.ApiUrl));
            if (string.IsNullOrEmpty(settings.ApiKey))
                throw new ArgumentException("ApiKey is required", nameof(settings.ApiKey));
            if (string.IsNullOrEmpty(settings.ApiVersion))
                throw new ArgumentException("ApiVersion is required", nameof(settings.ApiVersion));
            if (string.IsNullOrEmpty(settings.Tenant))
                throw new ArgumentException("Tenant is required", nameof(settings.Tenant));
        }
    }
}