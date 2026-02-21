using Fiscalapi.Abstractions;
using Fiscalapi.Http;
using Fiscalapi.Models;

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
    }
}