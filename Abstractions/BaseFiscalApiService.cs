using System.Threading.Tasks;
using Fiscalapi.Common;
using Fiscalapi.Http;

namespace Fiscalapi.Abstractions
{
    public abstract class BaseFiscalApiService<T> : BaseImmutableFiscalApiService<T>, IFiscalApiService<T> where T : BaseDto
    {
        protected BaseFiscalApiService(IFiscalApiHttpClient httpClient, string resourcePath, string apiVersion)
            : base(httpClient, resourcePath, apiVersion)
        {
        }

        public virtual Task<ApiResponse<T>> UpdateAsync(string id, T entity)
            => HttpClient.PutAsync<T>(BuildEndpoint(id), entity);
    }
}