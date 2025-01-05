using FiscalApi.Abstractions;
using FiscalApi.Http;
using FiscalApi.Models;

namespace FiscalApi.Services
{
    public class ProductService : BaseFiscalApiService<Product>, IProductService
    {
        public ProductService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "products", apiVersion)
        {
        }
    }
}