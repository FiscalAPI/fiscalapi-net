using Fiscalapi.Common;
using Fiscalapi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Servicio para gestionar los certificados CSD de emisores y receptores
    /// </summary>
    public interface ITaxFileService : IFiscalApiService<TaxFile>
    {
        /// <summary>
        /// No disponible: el API retiró <c>PUT /api/v4/tax-files/{id}</c>, que responde 405 (Method Not Allowed). Un certificado
        /// no se edita: suba el nuevo con <see cref="IFiscalApiService{T}.CreateAsync"/> y elimine el anterior con
        /// <see cref="IFiscalApiService{T}.DeleteAsync"/>.
        /// </summary>
        [Obsolete("El API ya no permite actualizar certificados: PUT /api/v4/tax-files/{id} responde 405. Suba el nuevo con CreateAsync y elimine el anterior con DeleteAsync.", true)]
        new Task<ApiResponse<TaxFile>> UpdateAsync(string id, TaxFile model);

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