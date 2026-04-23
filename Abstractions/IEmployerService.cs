using System.Threading.Tasks;
using Fiscalapi.Common;
using Fiscalapi.Models;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Interfaz para el servicio de Empleador — gestiona los datos del empleador anidados bajo una Persona.
    /// Patrón de endpoint: api/{version}/people/{personId}/employer
    /// </summary>
    public interface IEmployerService
    {
        Task<ApiResponse<EmployerData>> GetByIdAsync(string id);
        Task<ApiResponse<EmployerData>> CreateAsync(EmployerData requestModel);
        Task<ApiResponse<EmployerData>> UpdateAsync(EmployerData requestModel);
        Task<ApiResponse<bool>> DeleteAsync(string id);
    }
}
