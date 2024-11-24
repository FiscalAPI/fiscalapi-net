using System;
using System.Net.Http;
using System.Threading.Tasks;
using FiscalApi.Abstractions.Services;
using FiscalApi.Models;

namespace FiscalApi.Implementations.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        // Implement service methods using _httpClient
        public Task CreateAsync(Product invoice)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task CancelAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}