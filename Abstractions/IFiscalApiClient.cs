namespace FiscalApi.Abstractions
{
    public interface IFiscalApiClient
    {
        IInvoiceService Invoices { get; }
        IPersonService Persons { get; }
        IProductService Products { get; }
        IApiKeyService ApiKeys { get; }
        ICatalogService Catalogs { get; }
    }
}