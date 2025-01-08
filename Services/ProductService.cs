using System.Collections.Generic;
using System.Threading.Tasks;
using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;

namespace Fiscalapi.Services
{
    public class ProductService : BaseFiscalApiService<Product>, IProductService
    {
        public ProductService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "products", apiVersion)
        {
        }


        // GET /api/v4/products/{id}/taxes
        public Task<ApiResponse<List<ProductTax>>> GetTaxesAsync(string id)
        {
            // GET /api/v4/products/{id}/taxes
            var path = $"{id}/taxes";
            var endpoint = BuildEndpoint(path);
            return HttpClient.GetAsync<List<ProductTax>>(endpoint);
        }
    }
}