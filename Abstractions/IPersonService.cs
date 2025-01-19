using Fiscalapi.Models;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Interface for the Person (emisor, receptor, cliente, usuario) service
    /// </summary>
    public interface IPersonService : IFiscalApiService<Person>
    {
    }
}