using Fiscalapi.Common;
using Fiscalapi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Interface for the Person (emisor, receptor, cliente, usuario) service
    /// </summary>
    public interface IPersonService : IFiscalApiService<Person>
    {
        IEmployerService Employer { get; }
        IEmployeeService Employee { get; }

        /// <summary>Lista los certificados (CSD y FIEL) de una persona, solo metadatos, sin el archivo ni la contraseña.</summary>
        Task<ApiResponse<List<TaxFile>>> GetTaxFilesAsync(string personId);
    }
}