using System.Collections.Generic;
using System.Threading.Tasks;
using Fiscalapi.Common;
using Fiscalapi.Models;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Interface for the Product service
    /// </summary>
    public interface IProductService : IFiscalApiService<Product>
    {
        /// <summary>
        /// Lista los productos del tenant con los filtros asignados (null o sin filtros: la lista de siempre, primera
        /// página). GET /api/v4/products
        /// </summary>
        Task<ApiResponse<PagedList<Product>>> GetListAsync(ProductListFilters filters);

        // GET /api/v4/products/{id}/taxes
        Task<ApiResponse<List<ProductTax>>> GetTaxesAsync(string id);
    }
}