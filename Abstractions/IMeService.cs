using System.Threading.Tasks;
using Fiscalapi.Common;
using Fiscalapi.Models;

namespace Fiscalapi.Abstractions
{
    /// <summary>La persona de la credencial con que se llama al API.</summary>
    public interface IMeService
    {
        /// <summary>
        /// Obtiene la persona de la API key: su id, nombre, RFC y correo, su tenant, el ambiente que responde, si es el
        /// owner, lo que puede hacer y sus saldos. Sirve para validar una credencial: con una API key o un tenant que no
        /// son válidos responde 401. GET /api/v4/me
        /// </summary>
        Task<ApiResponse<MeInfo>> GetAsync();
    }
}
