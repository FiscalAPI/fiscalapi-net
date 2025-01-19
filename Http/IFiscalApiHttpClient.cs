using System.Threading.Tasks;
using Fiscalapi.Common;

namespace Fiscalapi.Http
{
    public interface IFiscalApiHttpClient
    {
        Task<ApiResponse<T>> GetAsync<T>(string endpoint);
        Task<ApiResponse<T>> GetByIdAsync<T>(string id);
        Task<ApiResponse<T>> PostAsync<T>(string endpoint, object payload);
        Task<ApiResponse<T>> PutAsync<T>(string endpoint, object payload);
        Task<ApiResponse<T>> PatchAsync<T>(string endpoint, object payload);
        Task<ApiResponse<bool>> DeleteAsync(string endpoint);
        Task<ApiResponse<T>> DeleteAsync<T>(string endpoint, object payload);
    }
}