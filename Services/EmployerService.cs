using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;
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
            => await HttpClient.GetAsync<EmployerData>(BuildEndpoint(id));

        // POST /api/{version}/people/{personId}/employer
        public async Task<ApiResponse<EmployerData>> CreateAsync(EmployerData requestModel)
            => await HttpClient.PostAsync<EmployerData>(BuildEndpoint(requestModel.PersonId), requestModel);

        // PUT /api/{version}/people/{personId}/employer
        public async Task<ApiResponse<EmployerData>> UpdateAsync(EmployerData requestModel)
            => await HttpClient.PutAsync<EmployerData>(BuildEndpoint(requestModel.PersonId), requestModel);

        // DELETE /api/{version}/people/{personId}/employer
        public async Task<ApiResponse<bool>> DeleteAsync(string id)
            => await HttpClient.DeleteAsync(BuildEndpoint(id));
    }
}
