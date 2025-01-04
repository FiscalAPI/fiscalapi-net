using FiscalApi.Http;
using FiscalApi.Models.Common;
using System;
using System.Threading.Tasks;

namespace FiscalApi.Abstractions
{
    public abstract class BaseFiscalApiService<T> : IFiscalApiService<T> where T : BaseDto
    {
        protected readonly IFiscalApiHttpClient HttpClient;
        protected readonly string ResourcePath;
        protected readonly string ApiVersion;

        protected BaseFiscalApiService(IFiscalApiHttpClient httpClient, string resourcePath, string apiVersion)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            ResourcePath = resourcePath ?? throw new ArgumentNullException(nameof(resourcePath));
            ApiVersion = apiVersion ?? throw new ArgumentNullException(nameof(apiVersion));
        }

        protected virtual string BuildEndpoint(string path = "")
        {
            var baseEndpoint = $"api/{ApiVersion}/{ResourcePath}";
            return string.IsNullOrEmpty(path) ? baseEndpoint : $"{baseEndpoint}/{path}";
        }

        public virtual Task<ApiResponse<PagedList<T>>> GetListAsync(bool includeDetails = false)
            => HttpClient.GetAsync<PagedList<T>>(BuildEndpoint(), includeDetails);

        public virtual Task<ApiResponse<T>> GetByIdAsync(string id, bool includeDetails = false)
            => HttpClient.GetByIdAsync<T>(BuildEndpoint(id), includeDetails);

        public virtual Task<ApiResponse<T>> CreateAsync(T entity)
            => HttpClient.PostAsync<T>(BuildEndpoint(), entity);

        public virtual Task<ApiResponse<T>> UpdateAsync(string id, T entity)
            => HttpClient.PutAsync<T>(BuildEndpoint(id), entity);

        public virtual Task<ApiResponse<bool>> DeleteAsync(string id)
            => HttpClient.DeleteAsync(BuildEndpoint(id));
    }
}