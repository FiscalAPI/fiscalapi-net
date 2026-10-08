using System.Threading.Tasks;
using Fiscalapi.Common;

namespace Fiscalapi.Abstractions
{
    /// <summary>
    /// Interface for the FiscalApi service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFiscalApiService<T> : IImmutableFiscalApiService<T> where T : BaseDto
    {
        Task<ApiResponse<T>> UpdateAsync(string id, T model);
    }
}