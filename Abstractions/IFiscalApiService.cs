using System.Threading.Tasks;
using Fiscalapi.Common;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Interface for the FiscalApi service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFiscalApiService<T> where T : BaseDto
    {
        Task<ApiResponse<PagedList<T>>> GetListAsync(int pageNumber, int pageSize);
        Task<ApiResponse<T>> GetByIdAsync(string id);
        Task<ApiResponse<T>> CreateAsync(T model);
        Task<ApiResponse<T>> UpdateAsync(string id, T model);
        Task<ApiResponse<bool>> DeleteAsync(string id);
    }

}