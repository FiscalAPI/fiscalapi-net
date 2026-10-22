using Fiscalapi.Common;
using Fiscalapi.Models;
using System.Threading.Tasks;

namespace Fiscalapi.Abstractions
{
    public interface IStampService : IFiscalApiService<StampTransaction>
    {
        Task<ApiResponse<bool>> TransferStamps(StampTransactionParams requestModel);

        /// <summary>
        /// Transfiere timbres, créditos de validación o créditos de ticket. POST /api/v4/stamps
        /// Con <paramref name="idempotencyKey"/> (de 1 a 64 caracteres ASCII visibles, sin espacios) se envía el header
        /// Idempotency-Key: repetir la misma key con la misma petición, dentro de 24 h, responde la respuesta original con el
        /// header Idempotent-Replayed: true y sin transferir otra vez; con otra petición, 422; con la original aún en proceso,
        /// 409. Sin ella, el header no se envía.
        /// </summary>
        Task<ApiResponse<bool>> TransferStamps(StampTransactionParams requestModel, string idempotencyKey);
        Task<ApiResponse<bool>> WithdrawStamps(StampTransactionParams requestModel);
    }
}
