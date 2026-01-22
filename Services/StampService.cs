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
            return await HttpClient.PostAsync<bool>(BuildEndpoint(), requestModel);
        }

        public async Task<ApiResponse<bool>> WithdrawStamps(StampTransactionParams requestModel)
        {
            return await HttpClient.PostAsync<bool>(BuildEndpoint(), requestModel);
        }
    }
}
