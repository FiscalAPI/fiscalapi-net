using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fiscalapi.Services
{
    public class SatValidationService : ISatValidationService
    {
        private readonly IFiscalApiHttpClient _httpClient;
        private readonly string _apiVersion;

        public SatValidationService(IFiscalApiHttpClient httpClient, string apiVersion)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiVersion = apiVersion ?? throw new ArgumentNullException(nameof(apiVersion));
        }

        private string BaseEndpoint => $"api/{_apiVersion}/sat-validations";

        public Task<ApiResponse<List<SatValidationType>>> GetTypesAsync()
        {
            return _httpClient.GetAsync<List<SatValidationType>>(BaseEndpoint);
        }

        public Task<ApiResponse<SatValidationType>> GetTypeByIdAsync(string id)
        {
            ValidateId(id);
            return _httpClient.GetAsync<SatValidationType>($"{BaseEndpoint}/{id}");
        }

        public Task<ApiResponse<List<SatValidationResult>>> GetStatusesAsync(string id)
        {
            ValidateId(id);
            return _httpClient.GetAsync<List<SatValidationResult>>($"{BaseEndpoint}/{id}/statuses");
        }

        public Task<ApiResponse<List<SatValidationResult>>> ValidateAsync(SatValidationRequest requestModel)
        {
            if (requestModel == null)
                throw new ArgumentNullException(nameof(requestModel), "No se acepta una solicitud de validación nula");

            return _httpClient.PostAsync<List<SatValidationResult>>(BaseEndpoint, requestModel);
        }

        private static void ValidateId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Se requiere el id del tipo de validación (por ejemplo sat.cfdi.status)", nameof(id));
        }
    }
}
