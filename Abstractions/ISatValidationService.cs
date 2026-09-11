using System.Collections.Generic;
using System.Threading.Tasks;
using Fiscalapi.Common;
using Fiscalapi.Models;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Validaciones SAT sobre CFDI: catálogo de tipos y estatus, y ejecución de validaciones
    /// (estructura, vigencia de certificado, sellos, estatus en el SAT y listas negras 69-B / 69-B Bis).
    /// </summary>
    public interface ISatValidationService
    {
        /// <summary>Lista los tipos de validación activos en el orden del catálogo. GET /api/v4/sat-validations</summary>
        Task<ApiResponse<List<SatValidationType>>> GetTypesAsync();

        /// <summary>Obtiene un tipo de validación por su id (por ejemplo sat.cfdi.status). GET /api/v4/sat-validations/{id}</summary>
        Task<ApiResponse<SatValidationType>> GetTypeByIdAsync(string id);

        /// <summary>
        /// Lista los estatus que un tipo de validación puede tomar al ejecutarse (aprobatorios primero, luego por id).
        /// Solo lectura, no consume crédito. GET /api/v4/sat-validations/{id}/statuses
        /// </summary>
        Task<ApiResponse<List<SatValidationTypeStatus>>> GetStatusesAsync(string id);

        /// <summary>
        /// Ejecuta las validaciones solicitadas sobre un CFDI (Xml) o un RFC (Tin, solo listas negras).
        /// Cada tipo consume un crédito de validación; los resultados vienen en el orden del catálogo.
        /// POST /api/v4/sat-validations
        /// </summary>
        Task<ApiResponse<List<SatValidationResult>>> ValidateAsync(SatValidationRequest requestModel);
    }
}
