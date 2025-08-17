using System.Collections.Generic;
using System.Threading.Tasks;
using Fiscalapi.Common;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Interface for the download catalog service
    /// </summary>
    public interface IDownloadCatalogService : IFiscalApiService<CatalogDto>
    {
        //GET /api/v4/download-catalogs
        Task<ApiResponse<List<string>>> GetListAsync();


        // /api/v4/download-catalogs/<catalogName>  ListCatalog
        Task<ApiResponse<List<CatalogDto>>> ListCatalogAsync(string catalogName);
    }
}