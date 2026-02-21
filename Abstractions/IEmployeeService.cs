using System.Threading.Tasks;
using Fiscalapi.Common;
using Fiscalapi.Models;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Interfaz para el servicio de Empleado — gestiona los datos del empleado anidados bajo una Persona.
    /// Patrón de endpoint: api/{version}/people/{personId}/employee
    /// </summary>
    public interface IEmployeeService
    {
        Task<ApiResponse<EmployeeData>> GetByIdAsync(string id);
        Task<ApiResponse<EmployeeData>> CreateAsync(EmployeeData requestModel);
        Task<ApiResponse<EmployeeData>> UpdateAsync(EmployeeData requestModel);
        Task<ApiResponse<bool>> DeleteAsync(string id);
    }
}
