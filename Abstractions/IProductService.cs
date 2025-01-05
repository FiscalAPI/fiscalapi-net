using System.Collections.Generic;
using System.Threading.Tasks;
using FiscalApi.Common;
using FiscalApi.Models;

namespace FiscalApi.Abstractions
{
    public interface IProductService : IFiscalApiService<Product>
    {
        // GET /api/v4/products/{id}/taxes
        Task<ApiResponse<List<ProductTax>>> GetTaxesAsync(string id);
    }
}