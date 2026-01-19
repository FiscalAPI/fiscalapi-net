using Fiscalapi.Common;
using Fiscalapi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fiscalapi.Abstractions
{
    public interface IStampService : IFiscalApiService<StampTransaction>
    {
        Task<ApiResponse<bool>> TransferStamps(StampTransactionParams requestModel);
        Task<ApiResponse<bool>> WithdrawStamps(StampTransactionParams requestModel);
    }
}
