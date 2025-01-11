namespace Fiscalapi.Abstractions
{
    public interface IFiscalApiClient
    {
        IInvoiceService Invoices { get; }
        IPersonService Persons { get; }
        IProductService Products { get; }
        IApiKeyService ApiKeys { get; }

        ITaxFileService TaxFiles { get; }
        ICatalogService Catalogs { get; }
    }
}