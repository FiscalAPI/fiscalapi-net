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
        // GET /api/v4/download-rules/test
        Task<ApiResponse<DownloadRequest>> CreateTestRuleAsync();
    }
}