using System.Threading.Tasks;
using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;

namespace Fiscalapi.Services
{
    public class DownloadRuleService : BaseFiscalApiService<DownloadRule>, IDownloadRuleService
    {
        public DownloadRuleService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "download-rules", apiVersion)
        {
        }


        public Task<ApiResponse<DownloadRequest>> CreateTestRuleAsync()
        {
            // GET /api/v4/products/{id}/taxes
            var path = $"test";
            var endpoint = BuildEndpoint(path);
            return HttpClient.PostAsync<DownloadRequest>(endpoint, null);
        }
    }
}