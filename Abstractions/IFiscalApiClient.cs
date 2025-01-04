using System;
using System.Collections.Generic;
using System.Text;

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