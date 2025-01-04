using FiscalApi.Abstractions;
using FiscalApi.Http;
using FiscalApi.Models;
using FiscalApi.Models.Common;
using System.Threading.Tasks;

namespace FiscalApi.Services
{
    public class ProductService : BaseFiscalApiService<Product>, IProductService
    {
        public ProductService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "products", apiVersion)
        {
        }

        public Task<ApiResponse<object>> CancelAsync(string id)
            => HttpClient.PostAsync<object>(BuildEndpoint($"{id}/cancel"), null);
    }
}