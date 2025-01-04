using FiscalApi.Models.Common;
using FiscalApi.Models.Invoices;
using System.Threading.Tasks;

namespace FiscalApi.Abstractions
{
    public interface IInvoiceService : IFiscalApiService<Invoice>
    {
        Task<ApiResponse<object>> CancelAsync(string id);
    }
}