using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;
using System;
using System.Threading.Tasks;

namespace Fiscalapi.Services
{
    public class EmployerService : IEmployerService
    {
        private IFiscalApiHttpClient HttpClient { get; }
        private readonly string _baseEndpoint;

        public EmployerService(IFiscalApiHttpClient fiscalApiHttpClient, string apiVersion)
        {
            HttpClient = fiscalApiHttpClient;
            _baseEndpoint = $"api/{apiVersion}/people";
        }

        private string BuildEndpoint(string personId)
            => $"{_baseEndpoint}/{personId}/employer";

        // GET /api/{version}/people/{personId}/employer
        public async Task<ApiResponse<EmployerData>> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Person ID is required to build endpoint", nameof(id));
            return await HttpClient.GetAsync<EmployerData>(BuildEndpoint(id));
        }

        // POST /api/{version}/people/{personId}/employer
        public async Task<ApiResponse<EmployerData>> CreateAsync(EmployerData requestModel)
        {
            ValidateRequestModel(requestModel);
            return await HttpClient.PostAsync<EmployerData>(BuildEndpoint(requestModel.PersonId), requestModel);
        }

        // PUT /api/{version}/people/{personId}/employer
        public async Task<ApiResponse<EmployerData>> UpdateAsync(EmployerData requestModel)
        {
            ValidateRequestModel(requestModel);
            return await HttpClient.PutAsync<EmployerData>(BuildEndpoint(requestModel.PersonId), requestModel);
        }

        // DELETE /api/{version}/people/{personId}/employer
        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID de la persona requerido", nameof(id));
            return await HttpClient.DeleteAsync(BuildEndpoint(id));
        }

        private void ValidateRequestModel(EmployerData requestModel)
        {
            if (requestModel == null)
                throw new ArgumentNullException(nameof(requestModel), "El modelo de solicitud no puede ser nulo");

            if (string.IsNullOrWhiteSpace(requestModel.PersonId))
                throw new ArgumentException("Se requiere el ID de la persona para crear o actualizar los datos del empleador", nameof(requestModel.PersonId));
        }
    }
}
