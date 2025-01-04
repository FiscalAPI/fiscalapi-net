using FiscalApi.Models;
using FiscalApi.Models.Common;
using System.Threading.Tasks;

namespace FiscalApi.Abstractions
{
    public interface IProductService : IFiscalApiService<Product>
    {
        Task<ApiResponse<object>> CancelAsync(string id);
    }
}