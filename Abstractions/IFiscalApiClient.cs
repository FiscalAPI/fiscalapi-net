using FiscalApi.Abstractions.Services;

namespace FiscalApi.Abstractions
{
    public interface IFiscalApiClient : IFiscalApiScopedService
    {
        IInvoiceService InvoiceService { get; }
        IProductService ProductService { get; }
    }
}