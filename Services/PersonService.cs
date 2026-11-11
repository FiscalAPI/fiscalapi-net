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

        /// <summary>Lista los certificados (CSD y FIEL) de una persona, solo metadatos, sin el archivo ni la contraseña.</summary>
        public Task<ApiResponse<List<TaxFile>>> GetTaxFilesAsync(string personId)
        {
            // GET /api/v4/people/{personId}/tax-files
            var endpoint = BuildEndpoint($"{personId}/tax-files");
            return HttpClient.GetAsync<List<TaxFile>>(endpoint);
        }
    }
}