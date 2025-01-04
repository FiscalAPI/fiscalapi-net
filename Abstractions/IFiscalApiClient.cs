namespace FiscalApi.Abstractions
{
    public interface IFiscalApiClient
    {
        IInvoiceService Invoices { get; }

        IProductService Products { get; }
        //IUserService Users { get; }
        //ICustomerService Customers { get; }
    }
}