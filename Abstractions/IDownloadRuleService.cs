using System.Threading.Tasks;
using Fiscalapi.Common;
using Fiscalapi.Models;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Interface for the Download Rule Service.
    /// </summary>
    public interface IDownloadRuleService : IFiscalApiService<DownloadRule>
    {
        // GET /api/v4/products/{id}/taxes
        Task<ApiResponse<DownloadRequest>> CreateTestRuleAsync();
    }
}