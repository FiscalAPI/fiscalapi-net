using System.Collections.Generic;
using System.Threading.Tasks;
using FiscalApi.Models;

namespace FiscalApi.Abstractions.Services
{
    //public interface IInvoiceService : IFiscalApiScopedService
    //{
    //    Task CreateAsync(Invoice invoice);
    //    Task<ApiResponse<Invoice>> GetAsync(string id);

    //    Task CancelAsync(string id);
    //}
    public interface IInvoiceService
    {
        Task<ApiResponse<List<Invoice>>> GetListAsync(bool details = false);
        Task<ApiResponse<Invoice>> GetByIdAsync(string id, bool details = false);
        Task<ApiResponse<Invoice>> CreateAsync(Invoice payload);
        Task<ApiResponse<Invoice>> UpdateAsync(string id, Invoice payload);
        Task<ApiResponse<object>> CancelAsync(string id);
    }

    



}