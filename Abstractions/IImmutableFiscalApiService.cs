using System.Threading.Tasks;
using Fiscalapi.Common;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Servicio de un recurso que se consulta, se crea y se elimina, pero no se actualiza (por ejemplo, los certificados).
    /// <see cref="IFiscalApiService{T}"/> lo extiende con <see cref="IFiscalApiService{T}.UpdateAsync"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IImmutableFiscalApiService<T> where T : BaseDto
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
        Task<ApiResponse<bool>> DeleteAsync(string id);
    }
}
