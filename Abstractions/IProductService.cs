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
        // GET /api/v4/products/{id}/taxes
        Task<ApiResponse<List<ProductTax>>> GetTaxesAsync(string id);
    }
}