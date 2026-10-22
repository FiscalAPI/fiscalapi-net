using System.Threading.Tasks;
using Fiscalapi.Common;
using Fiscalapi.Models;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Tickets de Auto CFDI: ingesta de ventas pagadas del punto de venta, consulta con filtros, código para reimprimir y
    /// cancelación.
    /// </summary>
    public interface ITicketService
    {
        /// <summary>
        /// Ingiere un ticket y cobra 1 crédito de ticket. POST /api/v4/tickets
        /// Con <paramref name="idempotencyKey"/> (de 1 a 64 caracteres ASCII visibles, sin espacios) se envía el header
        /// Idempotency-Key: repetir la misma llave de idempotencia con el mismo cuerpo, dentro de 24 h, devuelve la respuesta
        /// original (201, con el header Idempotent-Replayed: true) sin registrar otro ticket ni cobrar otra vez; con otro
        /// cuerpo, 422; con la original aún en proceso, 409 con Retry-After. Sin ella, el header no se envía. Un folio y
        /// serie ya activos del emisor responden 409.
        /// </summary>
        Task<ApiResponse<TicketIngestionResult>> CreateAsync(Ticket ticket, string idempotencyKey = null);

        /// <summary>
        /// Lista los tickets del tenant con los filtros asignados (null o sin filtros: todos, primera página).
        /// GET /api/v4/tickets
        /// </summary>
        Task<ApiResponse<PagedList<Ticket>>> GetListAsync(TicketListFilters filters);

        /// <summary>
        /// Obtiene un ticket; con <paramref name="details"/> trae también sus conceptos e historia.
        /// GET /api/v4/tickets/{id}?details=true
        /// </summary>
        Task<ApiResponse<Ticket>> GetByIdAsync(string id, bool details = false);

        /// <summary>Obtiene el código del ticket y la URL de su QR, para reimprimirlo. GET /api/v4/tickets/{id}/code</summary>
        Task<ApiResponse<TicketCodeResult>> GetCodeAsync(string id);

        /// <summary>
        /// Cancela un ticket en estado pending (409 en cualquier otro estado); el crédito de ticket no se devuelve.
        /// DELETE /api/v4/tickets/{id}
        /// </summary>
        Task<ApiResponse<bool>> CancelAsync(string id);
    }
}
