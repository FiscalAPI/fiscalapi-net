using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;
using System;
using System.Threading.Tasks;

namespace Fiscalapi.Services
{
    public class ManifestService : IManifestService
    {
        private readonly IFiscalApiHttpClient _httpClient;
        private readonly string _apiVersion;

        public ManifestService(IFiscalApiHttpClient httpClient, string apiVersion)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiVersion = apiVersion ?? throw new ArgumentNullException(nameof(apiVersion));
        }

        public async Task<ApiResponse<FileResponse>> SignAsync(SignManifestRequest requestModel)
        {
            if (requestModel == null)
                throw new ArgumentNullException(nameof(requestModel));

            return await _httpClient.PostAsync<FileResponse>($"api/{_apiVersion}/manifests", requestModel);
        }
    }
}
