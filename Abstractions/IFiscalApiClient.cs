namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Interface for the Fiscal API client
    /// </summary>
    public interface IFiscalApiClient
    {
        IInvoiceService Invoices { get; }
        IPersonService Persons { get; }
        IProductService Products { get; }
        IApiKeyService ApiKeys { get; }

        ITaxFileService TaxFiles { get; }
        ICatalogService Catalogs { get; }
        IDownloadCatalogService DownloadCatalogs { get; }
        IDownloadRuleService DownloadRules { get; }
        IDownloadRequestService DownloadRequests { get; }
    }
}