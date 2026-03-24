using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;
using System;
using System.Threading.Tasks;

namespace Fiscalapi.Services
{
    public class EmployeeService : IEmployeeService
    {
        private IFiscalApiHttpClient HttpClient { get; }
        private readonly string _baseEndpoint;

        public EmployeeService(IFiscalApiHttpClient fiscalApiHttpClient, string apiVersion)
        {
            HttpClient = fiscalApiHttpClient;
            _baseEndpoint = $"api/{apiVersion}/people";
        }

        private string BuildEndpoint(string personId)
            => $"{_baseEndpoint}/{personId}/employee";

        // GET /api/{version}/people/{personId}/employee
        public async Task<ApiResponse<EmployeeData>> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Employee person ID is required to build endpoint", nameof(id));
            return await HttpClient.GetAsync<EmployeeData>(BuildEndpoint(id));
        }

        // POST /api/{version}/people/{personId}/employee
        public async Task<ApiResponse<EmployeeData>> CreateAsync(EmployeeData requestModel)
        {
            ValidateRequestModel(requestModel);
            return await HttpClient.PostAsync<EmployeeData>(BuildEndpoint(requestModel.EmployeePersonId), requestModel);
        }

        // PUT /api/{version}/people/{personId}/employee
        public async Task<ApiResponse<EmployeeData>> UpdateAsync(EmployeeData requestModel)
        {
            ValidateRequestModel(requestModel);
            return await HttpClient.PutAsync<EmployeeData>(BuildEndpoint(requestModel.EmployeePersonId), requestModel);
        }

        // DELETE /api/{version}/people/{personId}/employee
        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("El ID de la persona requerido", nameof(id));
            return await HttpClient.DeleteAsync(BuildEndpoint(id));
        }

        private void ValidateRequestModel(EmployeeData requestModel)
        {
            if (requestModel == null)
                throw new ArgumentNullException(nameof(requestModel), "El modelo de solicitud no puede ser nulo");

            if (string.IsNullOrWhiteSpace(requestModel.EmployeePersonId))
                throw new ArgumentException("Se requiere el ID de la persona para crear o actualizar los datos del empleado", nameof(requestModel.EmployeePersonId));
        }
    }
}
