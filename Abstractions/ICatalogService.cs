using System.Collections.Generic;
using System.Threading.Tasks;
using Fiscalapi.Common;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Interface for the Catalog service
    /// </summary>
    public interface ICatalogService : IFiscalApiService<CatalogDto>
    {
        //GET /api/v4/catalogs
        Task<ApiResponse<List<string>>> GetListAsync();


        // /api/v4/catalogs/<catalogName>/key/<id>
        Task<ApiResponse<CatalogDto>> GetRecordByIdAsync(string catalogName, string id);


        //GET /api/v4/catalogs/{catalogName}/{searchText}
        Task<ApiResponse<PagedList<CatalogDto>>> SearchCatalogAsync(string catalogName, string searchText,
            int pageNumber = 1,
            int pageSize = 50);
    }
}