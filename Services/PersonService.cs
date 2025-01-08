using System.Collections.Generic;
using System.Threading.Tasks;
using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;

namespace Fiscalapi.Services
{
    public class PersonService : BaseFiscalApiService<Person>, IPersonService
    {
        public PersonService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "users", apiVersion)
        {
        }

        /// <summary>
        /// Agrega un archivo certificado o llave privada a una persona.
        /// </summary>
        /// <param name="taxFile"></param>
        /// <returns></returns>
        public Task<ApiResponse<TaxFile>> AddTaxFileAsync(TaxFile taxFile)
        {
            // POST /api/v4/users/{userId}/taxfiles
            var path = $"{taxFile.PersonId}/taxfiles";
            var endpoint = BuildEndpoint(path);
            return HttpClient.PostAsync<TaxFile>(endpoint, taxFile);
        }


        /// <summary>
        /// Obtiene un archivo certificado o llave privada de una persona.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ApiResponse<TaxFile>> GetTaxFileAsync(string userId, string id)
        {
            // GET /api/v4/users/{userId}/taxfiles/{id}
            var path = $"{userId}/taxfiles/{id}";
            var endpoint = BuildEndpoint(path);
            return HttpClient.GetAsync<TaxFile>(endpoint);
        }


        /// <summary>
        /// Elimina un archivo certificado o llave privada de una persona.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ApiResponse<bool>> DeleteTaxFileAsync(string userId, string id)
        {
            // DELETE /api/v4/users/{userId}/taxfiles/{id}
            var path = $"{userId}/taxfiles/{id}";
            var endpoint = BuildEndpoint(path);
            return HttpClient.DeleteAsync(endpoint);
        }


        /// <summary>
        /// Obtiene los últimos archivos certificados o llaves privadas válidos de una persona.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<ApiResponse<List<TaxFile>>> GetLatestTaxFilesAsync(string userId)
        {
            // GET /api/v4/users/{userId}/taxfiles/last
            var path = $"{userId}/taxfiles/last";
            var endpoint = BuildEndpoint(path);
            return HttpClient.GetAsync<List<TaxFile>>(endpoint);
        }
    }
}