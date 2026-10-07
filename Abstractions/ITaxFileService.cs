using Fiscalapi.Common;
using Fiscalapi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Servicio para gestionar los certificados CSD de emisores y receptores.
    /// No tiene <c>UpdateAsync</c>: el API no actualiza certificados (<c>PUT /api/v4/tax-files/{id}</c> responde 405).
    /// Para cambiar un certificado, suba el nuevo con <see cref="IImmutableFiscalApiService{T}.CreateAsync"/> y elimine el
    /// anterior con <see cref="IImmutableFiscalApiService{T}.DeleteAsync"/>.
    /// </summary>
    public interface ITaxFileService : IImmutableFiscalApiService<TaxFile>
    {
        /// <summary>
        /// Obtiene el último par de ids de certificados válidos y vigente de una persona. Es decir sus certificados por defecto (ids)
        /// </summary>
        /// <param name="personId">Id de la persona dueña de los certificados</param>
        /// <returns>Lista con un par de certificados, pero sin con tenido, solo sus Ids.</returns>
        Task<ApiResponse<List<TaxFile>>> GetDefaultReferencesAsync(string personId);


        /// <summary>
        /// Obtiene el último par de certificados válidos y vigente de una persona. Es decir sus certificados por defecto
        /// </summary>
        /// <param name="personId">Id de la persona dueña de los certificados</param>
        /// <returns>Lista con un par de certificados</returns>
        Task<ApiResponse<List<TaxFile>>> GetDefaultValuesAsync(string personId);
    }
}