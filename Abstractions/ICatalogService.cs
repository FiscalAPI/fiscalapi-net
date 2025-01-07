using System.Collections.Generic;
using System.Threading.Tasks;
using FiscalApi.Common;

namespace FiscalApi.Abstractions
{
    public interface ICatalogService : IFiscalApiService<CatalogDto>
    {
        //GET /api/v4/catalogs
        Task<ApiResponse<List<string>>> GetListAsync();


        //GET /api/v4/catalogs/{catalogName}/{searchText}
        Task<ApiResponse<PagedList<CatalogDto>>> SearchCatalogAsync(string catalogName, string searchText,
            int pageNumber = 1,
            int pageSize = 50);
    }
}