using Fiscalapi.Common;
using Fiscalapi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Interface for the Person (emisor, receptor, cliente, usuario) service
    /// </summary>
    /// <remarks>
    /// En <c>GetByIdAsync(id, details)</c>, <c>details</c> es obsoleto en personas y no tiene efecto: el API responde la
    /// misma persona con cualquier valor. Se conserva para no romper la compilación; omítelo.
    /// </remarks>
    public interface IPersonService : IFiscalApiService<Person>
    {
        IEmployerService Employer { get; }
        IEmployeeService Employee { get; }

        /// <summary>
        /// Lista las personas del tenant con los filtros asignados (null o sin filtros: la lista de siempre, primera
        /// página). GET /api/v4/people
        /// </summary>
        Task<ApiResponse<PagedList<Person>>> GetListAsync(PersonListFilters filters);

        /// <summary>Lista los certificados (CSD y FIEL) de una persona, solo metadatos, sin el archivo ni la contraseña.</summary>
        Task<ApiResponse<List<TaxFile>>> GetTaxFilesAsync(string personId);
    }
}