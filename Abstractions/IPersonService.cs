using Fiscalapi.Models;
using Fiscalapi.Services;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Interface for the Person (emisor, receptor, cliente, usuario) service
    /// </summary>
    public interface IPersonService : IFiscalApiService<Person>
    {
        EmployerService Employer {  get; }
        EmployeeService Employee { get; }
    }
}