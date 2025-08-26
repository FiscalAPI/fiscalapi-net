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
            throw new System.NotImplementedException("Utiliza GetListAsync y ListCatalogAsync en su lugar.");
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
            throw new System.NotImplementedException("Utiliza GetListAsync y ListCatalogAsync en su lugar.");
        }

        /// <summary>
        /// No se implementa la paginación.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override Task<ApiResponse<CatalogDto>> CreateAsync(CatalogDto entity)
        {
            throw new System.NotImplementedException("Utiliza GetListAsync y ListCatalogAsync en su lugar.");
        }

        /// <summary>
        /// No se implementa la paginación.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override Task<ApiResponse<bool>> DeleteAsync(string id)
        {
            throw new System.NotImplementedException("Utiliza GetListAsync y ListCatalogAsync en su lugar.");
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
        /// Lista todos los registros de un catálogo pasando el nombre del catálogo.
        /// </summary>
        /// <param name="catalogName">Nombre del catálogo</param>
        /// <returns>Lista de CatalogDto</returns>
        public Task<ApiResponse<List<CatalogDto>>> ListCatalogAsync(string catalogName)
        {
            // api/v4/download-catalogs/<catalogName>/
            var path = $"{catalogName}";
            var endpoint = BuildEndpoint(path);
            return HttpClient.GetAsync<List<CatalogDto>>(endpoint);
        }
    }
}