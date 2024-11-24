using FiscalApi.Abstractions;
using FiscalApi.Abstractions.Services;
using FiscalApi.Implementations.Services;
using FiscalApi.OptionsHttp;

namespace FiscalApi.Implementations
{
    public class FiscalApiClient : IFiscalApiClient
    {
        public IInvoiceService InvoiceService { get; }
        public IProductService ProductService { get; }

        public FiscalApiClient(FiscalApiSettings settings)
        {
            InvoiceService = new InvoiceService(FiscalApiClientFactory.Create(settings));
            ProductService = new ProductService(FiscalApiClientFactory.Create(settings));
            // Other services
        }
    }
}