using System.Threading.Tasks;
using FiscalApi.Models;

namespace FiscalApi.Abstractions.Services
{
    public interface IProductService :IFiscalApiScopedService
    {
        Task CreateAsync(Product invoice);
        Task<Product> GetAsync(string id);
        Task CancelAsync(string id);
        // Other methods
    }
}