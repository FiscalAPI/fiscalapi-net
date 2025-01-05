using System.Threading.Tasks;
using FiscalApi.Common;

namespace FiscalApi.Abstractions
{
    public interface IFiscalApiService<T> where T : BaseDto
    {
        Task<ApiResponse<PagedList<T>>> GetListAsync(bool includeDetails = false);
        Task<ApiResponse<T>> GetByIdAsync(string id, bool includeDetails = false);
        Task<ApiResponse<T>> CreateAsync(T model);
        Task<ApiResponse<T>> UpdateAsync(string id, T model);
        Task<ApiResponse<bool>> DeleteAsync(string id);
    }
}