using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;
using System.Threading.Tasks;

namespace Fiscalapi.Services
{
    public class StampService : BaseFiscalApiService<StampTransaction>, IStampService
    {
        public StampService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "stamps", apiVersion)
        {
        }

        public async Task<ApiResponse<bool>> TransferStamps(StampTransactionParams requestModel)
        {
            ValidateRequest(requestModel);
            return await HttpClient.PostAsync<bool>(BuildEndpoint(), requestModel);
        }

        public async Task<ApiResponse<bool>> WithdrawStamps(StampTransactionParams requestModel)
        {
            ValidateRequest(requestModel);
            return await HttpClient.PostAsync<bool>(BuildEndpoint(), requestModel);
        }

        private void ValidateRequest(StampTransactionParams requestModel)
        {
            if (requestModel == null)
                throw new ArgumentNullException(nameof(requestModel), "No se acepta modelo de transaccion nulo");

            if (string.IsNullOrWhiteSpace(requestModel.FromPersonId))
                throw new ArgumentException("Se requiere el ID de la persona de origen para la transferencia de timbres", nameof(requestModel.FromPersonId));

            if (string.IsNullOrWhiteSpace(requestModel.ToPersonId))
                throw new ArgumentException("Se requiere el ID de la persona de destino para la transferencia de timbres", nameof(requestModel.ToPersonId));

            if (requestModel.Amount <= 0)
                throw new ArgumentException("La cantidad debe ser mayor que cero", nameof(requestModel.Amount));
        }
    }
}
