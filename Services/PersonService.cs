using Fiscalapi.Abstractions;
using Fiscalapi.Http;
using Fiscalapi.Models;

namespace Fiscalapi.Services
{
    public class PersonService : BaseFiscalApiService<Person>, IPersonService
    {
        public EmployerService Employer { get; }
        public EmployeeService Employee { get; }

        public PersonService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "people", apiVersion)
        {
            Employer = new EmployerService(httpClient);
            Employee = new EmployeeService(httpClient);
        }
    }
}