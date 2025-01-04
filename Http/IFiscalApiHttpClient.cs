using FiscalApi.Models.Common;
using System.Threading.Tasks;

namespace FiscalApi.Http
{
    public interface IFiscalApiHttpClient
    {
        Task<ApiResponse<T>> GetAsync<T>(string endpoint, bool includeDetails = false);
        Task<ApiResponse<T>> GetByIdAsync<T>(string id, bool includeDetails = false);
        Task<ApiResponse<T>> PostAsync<T>(string endpoint, object payload);
        Task<ApiResponse<T>> PutAsync<T>(string endpoint, object payload);
        Task<ApiResponse<T>> PatchAsync<T>(string endpoint, object payload);
        Task<ApiResponse<bool>> DeleteAsync(string endpoint);
    }
}