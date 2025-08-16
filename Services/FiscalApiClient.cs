using System;
using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;

namespace Fiscalapi.Services
{
    /// <summary>
    /// Client for the Fiscal API
    /// </summary>
    public class FiscalApiClient : IFiscalApiClient
    {
        public IInvoiceService Invoices { get; }
        public IProductService Products { get; }
        public IPersonService Persons { get; }
        public IApiKeyService ApiKeys { get; }
        public ICatalogService Catalogs { get; }
        public ITaxFileService TaxFiles { get; }
        public IDownloadCatalogService DownloadCatalogs { get; }
        public IDownloadRuleService DownloadRules { get; }
        public IDownloadRequestService DownloadRequests { get; }


        private FiscalApiClient(FiscalapiSettings settings)
        {
            var httpClient = FiscalApiHttpClientFactory.Create(settings);
            var apiVersion = settings.ApiVersion;


            // Initialize services
            Invoices = new InvoiceService(httpClient, apiVersion);
            Products = new ProductService(httpClient, apiVersion);
            Persons = new PersonService(httpClient, apiVersion);
            ApiKeys = new ApiKeyService(httpClient, apiVersion);
            Catalogs = new CatalogService(httpClient, apiVersion);
            TaxFiles = new TaxFileService(httpClient, apiVersion);
            DownloadCatalogs = new DownloadCatalogService(httpClient, apiVersion);
            DownloadRules = new DownloadRuleService(httpClient, apiVersion);
            DownloadRequests = new DownloadRequestService(httpClient, apiVersion);
        }

        public static IFiscalApiClient Create(FiscalapiSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            ValidateSettings(settings);
            return new FiscalApiClient(settings);
        }


        private static void ValidateSettings(FiscalapiSettings settings)
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