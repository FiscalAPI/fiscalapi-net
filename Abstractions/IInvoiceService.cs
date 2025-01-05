using System.Threading.Tasks;
using FiscalApi.Common;
using FiscalApi.Models;

namespace FiscalApi.Abstractions
{
    public interface IInvoiceService : IFiscalApiService<Invoice>
    {

        // Cancel any type of invoice
        Task<ApiResponse<CancelInvoiceResponse>> CancelAsync(CancelInvoiceRequest requestModel);

        // Get invoice PDF file
        Task<ApiResponse<FileResponse>> GetPdfAsync(CreatePdfRequest requestModel);

        // Get invoice XML file
        Task<ApiResponse<FileResponse>> GetXmlAsync(string id);

        // Send invoice to the email
        Task<ApiResponse<bool>> SendAsync(SendInvoiceRequest requestModel);
    }
}