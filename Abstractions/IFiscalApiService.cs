using FiscalApi.Models.Common;
using System.Threading.Tasks;

namespace FiscalApi.Abstractions
{
    public interface IFiscalApiService<T> where T : BaseDto
    {
        Task<ApiResponse<PagedList<T>>> GetListAsync(bool includeDetails = false);
        Task<ApiResponse<T>> GetByIdAsync(string id, bool includeDetails = false);
        Task<ApiResponse<T>> CreateAsync(T entity);
        Task<ApiResponse<T>> UpdateAsync(string id, T entity);
        Task<ApiResponse<bool>> DeleteAsync(string id);
    }
}