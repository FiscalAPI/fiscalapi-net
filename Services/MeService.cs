using System;
using System.Threading.Tasks;
using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;

namespace Fiscalapi.Services
{
    public class MeService : IMeService
    {
        private readonly IFiscalApiHttpClient _httpClient;
        private readonly string _apiVersion;

        public MeService(IFiscalApiHttpClient httpClient, string apiVersion)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiVersion = apiVersion ?? throw new ArgumentNullException(nameof(apiVersion));
        }

        public Task<ApiResponse<MeInfo>> GetAsync() => _httpClient.GetAsync<MeInfo>($"api/{_apiVersion}/me");
    }
}
