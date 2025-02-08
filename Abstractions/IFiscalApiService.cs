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

        /// <summary>
        /// Obtiene un recurso por su id y opcionalmente expandir sus objetos relacionados (detalles)
        /// </summary>
        /// <param name="id">Id del recurso</param>
        /// <param name="details">True para obtener los objetos relacionados, de lo contrario False.</param>
        /// <returns></returns>
        Task<ApiResponse<T>> GetByIdAsync(string id, bool details = false);

        Task<ApiResponse<T>> CreateAsync(T model);
        Task<ApiResponse<T>> UpdateAsync(string id, T model);
        Task<ApiResponse<bool>> DeleteAsync(string id);
    }
}