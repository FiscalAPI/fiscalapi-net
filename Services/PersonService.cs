using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fiscalapi.Services
{
    public class PersonService : BaseFiscalApiService<Person>, IPersonService
    {
        public IEmployerService Employer { get; }
        public IEmployeeService Employee { get; }

        public PersonService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "people", apiVersion)
        {
            Employer = new EmployerService(httpClient, apiVersion);
            Employee = new EmployeeService(httpClient, apiVersion);
        }

        public Task<ApiResponse<PagedList<Person>>> GetListAsync(PersonListFilters filters)
            => GetFilteredListAsync(filters?.ToQueryString());

        /// <summary>Obtiene una persona por su id. GET /api/v4/people/{id}</summary>
        /// <param name="id">Id de la persona.</param>
        /// <param name="details">Obsoleto y sin efecto en personas: el API responde la misma persona con cualquier valor. Omítelo.</param>
        public override Task<ApiResponse<Person>> GetByIdAsync(string id, bool details = false)
            => base.GetByIdAsync(id, details);

        /// <summary>Lista los certificados (CSD y FIEL) de una persona, solo metadatos, sin el archivo ni la contraseña.</summary>
        public Task<ApiResponse<List<TaxFile>>> GetTaxFilesAsync(string personId)
        {
            // GET /api/v4/people/{personId}/tax-files
            var endpoint = BuildEndpoint($"{personId}/tax-files");
            return HttpClient.GetAsync<List<TaxFile>>(endpoint);
        }
    }
}