using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fiscalapi.Services
{
   
    public class DownloadCatalogService : BaseFiscalApiService<CatalogDto>, IDownloadCatalogService
    {
        public DownloadCatalogService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "download-catalogs", apiVersion)
        {
        }

        /// <summary>
        /// No se implementa la paginación.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override Task<ApiResponse<PagedList<CatalogDto>>> GetListAsync(int pageNumber, int pageSize)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// No se implementa la paginación.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override Task<ApiResponse<CatalogDto>> UpdateAsync(string id, CatalogDto entity)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// No se implementa la paginación.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override Task<ApiResponse<CatalogDto>> CreateAsync(CatalogDto entity)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// No se implementa la paginación.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// Recupera todos los nombres de los catálogos de descarga masiva disponibles para listarlos por nombre.
        /// </summary>
        /// <returns></returns>
        public Task<ApiResponse<List<string>>> GetListAsync()
        {
            return HttpClient.GetAsync<List<string>>(BuildEndpoint());
        }


        /// <summary>
        /// Recupera un registro de un catálogo por catalogName.
        /// </summary>
        /// <param name="catalogName">Nombre del catálogo</param>
        /// <returns>CatalogDto</returns>
        public Task<ApiResponse<List<CatalogDto>>> GetRecordByNameAsync(string catalogName)
        {
            var path = $"{catalogName}";
            var endpoint = BuildEndpoint(path);
            // api/v4/download-catalogs/<catalogName>/
            return HttpClient.GetAsync<List<CatalogDto>>(endpoint);
        }

        
    }
}
