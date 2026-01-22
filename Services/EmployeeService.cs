using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;
using System.Threading.Tasks;

namespace Fiscalapi.Services
{
    public class EmployeeService
    {
        private IFiscalApiHttpClient HttpClient { get; }
        private string baseEndpoint = "api/v4/people";

        public EmployeeService(IFiscalApiHttpClient fiscalApiHttpClient)
        {
            HttpClient = fiscalApiHttpClient;
        }

        // GET /api/v4/people/{personId}/employee
        public async Task<ApiResponse<EmployeeData>> GetByIdAsync(string id)
        {
            string endpoint = $"{baseEndpoint}/{id}/employee";

            return await HttpClient.GetAsync<EmployeeData>(endpoint);
        }

        //POST /api/v4/people/{personId}/employee
        public async Task<ApiResponse<EmployeeData>> CreateAsync(EmployeeData requestModel)
        {
            string endpoint = $"{baseEndpoint}/{requestModel.EmployeePersonId}/employee";

            return await HttpClient.PostAsync<EmployeeData>(endpoint, requestModel);
        }

        // PUT /api/v4/people/{personId}/employee
        public async Task<ApiResponse<EmployeeData>> UpdateAsync(EmployeeData requestModel)
        {
            string endpoint = $"{baseEndpoint}/{requestModel.EmployeePersonId}/employee";

            return await HttpClient.PutAsync<EmployeeData>(endpoint, requestModel);
        }

        // DELETE /api/v4/people/{personId}/employee
        public async Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            string endpoint = $"{baseEndpoint}/{id}/employee";

            return await HttpClient.DeleteAsync(endpoint);
        }
    }
}
