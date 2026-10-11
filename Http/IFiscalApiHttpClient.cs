using System.Collections.Generic;
using System.Threading.Tasks;
using Fiscalapi.Common;

namespace Fiscalapi.Http
{
    public interface IFiscalApiHttpClient
    {
        Task<ApiResponse<T>> GetAsync<T>(string endpoint);
        Task<ApiResponse<T>> GetByIdAsync<T>(string id);
        Task<ApiResponse<T>> PostAsync<T>(string endpoint, object payload);

        /// <summary>POST con headers propios de la petición (por ejemplo Idempotency-Key), además de los del cliente.</summary>
        Task<ApiResponse<T>> PostAsync<T>(string endpoint, object payload, IReadOnlyDictionary<string, string> headers);

        Task<ApiResponse<T>> PutAsync<T>(string endpoint, object payload);
        Task<ApiResponse<T>> PatchAsync<T>(string endpoint, object payload);
        Task<ApiResponse<bool>> DeleteAsync(string endpoint);
        Task<ApiResponse<T>> DeleteAsync<T>(string endpoint, object payload);
    }
}