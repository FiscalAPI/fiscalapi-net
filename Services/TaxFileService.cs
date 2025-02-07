using Fiscalapi.Abstractions;
using Fiscalapi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fiscalapi.Common;
using Fiscalapi.Http;

namespace Fiscalapi.Services
{
    public class TaxFileService : BaseFiscalApiService<TaxFile>, ITaxFileService
    {
        public TaxFileService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "tax-files", apiVersion)
        {
        }


        /// <summary>
        /// Obtiene el último par de ids de certificados válidos y vigente de una persona. Es decir sus certificados por defecto (ids)
        /// </summary>
        /// <param name="personId">Id de la persona propietaria de los certificados</param>
        /// <returns>Lista con un par de certificados, pero sin con tenido, solo sus Ids.</returns>
        public Task<ApiResponse<List<TaxFile>>> GetDefaultReferencesAsync(string personId)
        {
            // GET /api/v4/tax-files/{personId}/default-references
            var path = $"{personId}/default-references";
            var endpoint = BuildEndpoint(path);
            return HttpClient.GetAsync<List<TaxFile>>(endpoint);
        }

        /// <summary>
        /// Obtiene el último par de certificados válidos y vigente de una persona. Es decir sus certificados por defecto
        /// </summary>
        /// <param name="personId">Id de la persona dueña de los certificados</param>
        /// <returns>Lista con un par de certificados</returns>
        public Task<ApiResponse<List<TaxFile>>> GetDefaultValuesAsync(string personId)
        {
            //GET /api/v4/tax-files/{personId}/default-values
            var path = $"{personId}/default-values";
            var endpoint = BuildEndpoint(path);
            return HttpClient.GetAsync<List<TaxFile>>(endpoint);
        }
    }
}