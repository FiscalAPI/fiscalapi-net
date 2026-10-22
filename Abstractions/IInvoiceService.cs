using System.Threading.Tasks;
using Fiscalapi.Common;
using Fiscalapi.Models;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Interface for the Invoice service
    /// </summary>
    public interface IInvoiceService : IFiscalApiService<Invoice>
    {
        /// <summary>
        /// Crea una factura. POST /api/v4/invoices
        /// Con <paramref name="idempotencyKey"/> (de 1 a 64 caracteres ASCII visibles, sin espacios) se envía el header
        /// Idempotency-Key: repetir la misma llave de idempotencia con la misma petición, dentro de 24 h, devuelve la factura original con el
        /// header Idempotent-Replayed: true y sin timbrar otra vez; con otra petición, 422; con la original aún en proceso, 409.
        /// Sin ella, el header no se envía.
        /// </summary>
        Task<ApiResponse<Invoice>> CreateAsync(Invoice requestModel, string idempotencyKey = null);

        // Cancel any type of invoice
        Task<ApiResponse<CancelInvoiceResponse>> CancelAsync(CancelInvoiceRequest requestModel);

        // Get invoice PDF file
        Task<ApiResponse<FileResponse>> GetPdfAsync(CreatePdfRequest requestModel);

        // Get invoice XML file
        Task<ApiResponse<FileResponse>> GetXmlAsync(string id);

        // Send invoice to the email
        Task<ApiResponse<bool>> SendAsync(SendInvoiceRequest requestModel);


        // Get invoice status
        Task<ApiResponse<InvoiceStatusResponse>> GetStatusAsync(InvoiceStatusRequest requestModel);
    }
}