using System.Collections.Generic;
using FiscalApi.Models;
using System.Threading.Tasks;
using FiscalApi.Common;

namespace FiscalApi.Abstractions
{
    public interface IPersonService : IFiscalApiService<Person>
    {
        // Add tax file

        // POST /api/v4/users/{userId}/taxfiles
        Task<ApiResponse<TaxFile>> AddTaxFileAsync(TaxFile taxFile);


        // GET /api/v4/users/{userId}/taxfiles/{id}
        Task<ApiResponse<TaxFile>> GetTaxFileAsync(string userId, string id);
        

        // DELETE /api/v4/users/{userId}/taxfiles/{id}
        Task<ApiResponse<bool>> DeleteTaxFileAsync(string userId, string id);
       
        // GET /api/v4/users/{userId}/taxfiles/last
        Task<ApiResponse<List<TaxFile>>> GetLatestTaxFilesAsync(string userId);

    }
}