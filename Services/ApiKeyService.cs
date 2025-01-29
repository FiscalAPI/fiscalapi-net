using Fiscalapi.Abstractions;
using Fiscalapi.Http;
using Fiscalapi.Models;

namespace Fiscalapi.Services
{
    public class ApiKeyService : BaseFiscalApiService<ApiKey>, IApiKeyService
    {
        public ApiKeyService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "apikeys", apiVersion)
        {
        }
    }
}