using System.Collections.Generic;
using System.Threading.Tasks;
using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;

namespace Fiscalapi.Services
{
    public class CatalogService : BaseFiscalApiService<CatalogDto>, ICatalogService
    {
        public CatalogService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "catalogs", apiVersion)
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
        /// Recupera todos los nombres de los catálogos disponibles para realizar búsquedas.
        /// </summary>
        /// <returns></returns>
        public Task<ApiResponse<List<string>>> GetListAsync()
        {
            return HttpClient.GetAsync<List<string>>(BuildEndpoint());
        }


        /// <summary>
        /// Recupera un registro de un catálogo por catalogName y id.
        /// </summary>
        /// <param name="catalogName">Nombre del catálogo</param>
        /// <param name="id">Id del registro en el catalogName</param>
        /// <returns>CatalogDto</returns>
        public Task<ApiResponse<CatalogDto>> GetRecordByIdAsync(string catalogName, string id)
        {
            var path = $"{catalogName}/key/{id}";
            var endpoint = BuildEndpoint(path);
            ///api/v4/catalogs/<catalogName>/key/<id>
            return HttpClient.GetAsync<CatalogDto>(endpoint);
        }

        /// <summary>
        /// Busca en un catálogo.
        /// </summary>
        /// <param name="catalogName">Catalog name. Must be a catalog retrieved from GetListAsync() </param>
        /// <param name="searchText">Criterio de búsqueda. Debe tener 4 caracteres de longitud como mínimo.</param>
        /// <param name="pageNumber">Numero de pagina a recuperar</param>
        /// <param name="pageSize">Tamaño de la página entre 1 y 100 registros por página. </param>
        /// <returns></returns>
        public async Task<ApiResponse<PagedList<CatalogDto>>> SearchCatalogAsync(string catalogName, string searchText,
            int pageNumber = 1, int pageSize = 50)
        {
            var path = $"{catalogName}/{searchText}";
            var queryParams = new Dictionary<string, string>
            {
                { "pageNumber", pageNumber.ToString() },
                { "pageSize", pageSize.ToString() }
            };

            var endpoint = BuildEndpoint(path, queryParams);

            var response = await HttpClient.GetAsync<PagedList<CatalogDto>>(endpoint);
            return response;
        }
    }
}