using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;
using System.Threading.Tasks;

namespace Fiscalapi.Services
{
    public class EmployerService
    {
        private IFiscalApiHttpClient HttpClient { get; }
        private string baseEndpoint = "api/v4/people";

        public EmployerService(IFiscalApiHttpClient fiscalApiHttpClient)
        {
            HttpClient = fiscalApiHttpClient;
        }

        // GET /api/v4/people/{personId}/employer
        public async Task<ApiResponse<EmployerData>> GetByIdAsync(string id)
        {
            string endpoint = $"{baseEndpoint}/{id}/employer";

            return await HttpClient.GetAsync<EmployerData>(endpoint);
        }

        //POST /api/v4/people/{personId}/employer
        public async Task<ApiResponse<EmployerData>> CreateAsync(EmployerData requestModel)
        {
            string endpoint = $"{baseEndpoint}/{requestModel.PersonId}/employer";

            return await HttpClient.PostAsync<EmployerData>(endpoint, requestModel);
        }

        // PUT /api/v4/people/{personId}/employer
        public async Task<ApiResponse<EmployerData>> UpdateAsync(EmployerData requestModel)
        {
            string endpoint = $"{baseEndpoint}/{requestModel.PersonId}/employer";

            return await HttpClient.PutAsync<EmployerData>(endpoint, requestModel);
        }

        // DELETE /api/v4/people/{personId}/employer
        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            string endpoint = $"{baseEndpoint}/{id}/employer";

            return await HttpClient.DeleteAsync(endpoint);
        }
    }
}
