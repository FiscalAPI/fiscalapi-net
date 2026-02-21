using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;
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
            => await HttpClient.GetAsync<EmployeeData>(BuildEndpoint(id));

        // POST /api/{version}/people/{personId}/employee
        public async Task<ApiResponse<EmployeeData>> CreateAsync(EmployeeData requestModel)
            => await HttpClient.PostAsync<EmployeeData>(BuildEndpoint(requestModel.EmployeePersonId), requestModel);

        // PUT /api/{version}/people/{personId}/employee
        public async Task<ApiResponse<EmployeeData>> UpdateAsync(EmployeeData requestModel)
            => await HttpClient.PutAsync<EmployeeData>(BuildEndpoint(requestModel.EmployeePersonId), requestModel);

        // DELETE /api/{version}/people/{personId}/employee
        public async Task<ApiResponse<bool>> DeleteAsync(string id)
            => await HttpClient.DeleteAsync(BuildEndpoint(id));
    }
}
