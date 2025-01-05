namespace FiscalApi.Abstractions
{
    public interface IFiscalApiClient
    {
        IInvoiceService Invoices { get; }

        IPersonService Persons { get; }
        IProductService Products { get; }
        //IUserService Users { get; }
        //ICustomerService Customers { get; }
    }
}