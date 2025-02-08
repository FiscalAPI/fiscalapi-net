using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Fiscalapi.Common;
using Fiscalapi.Http;

namespace Fiscalapi.Abstractions
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

        /// <summary>
        /// Construye la URL base y opcionalmente agrega path o query params.
        /// Ejemplo: 
        ///   BuildEndpoint() -> "api/v4/users"
        ///   BuildEndpoint("123") -> "api/v4/users/123"
        ///   BuildEndpoint("", new Dictionary{string,string} {["PageNumber"]="1",["PageSize"]="2"}) -> "api/v4/users?PageNumber=1&PageSize=2"
        /// </summary>
        protected virtual string BuildEndpoint(string path = "", IDictionary<string, string> queryParams = null)
        {
            var baseEndpoint = $"api/{ApiVersion}/{ResourcePath}";
            if (!string.IsNullOrEmpty(path))
            {
                baseEndpoint += $"/{path}";
            }

            if (queryParams == null || queryParams.Count <= 0)
                return baseEndpoint;


            // query string
            var queryString = string.Join("&", queryParams
                .Where(kvp => !string.IsNullOrEmpty(kvp.Key))
                .Select(kvp => $"{kvp.Key}={kvp.Value}"));

            baseEndpoint += $"?{queryString}";

            return baseEndpoint;
        }

        public virtual Task<ApiResponse<PagedList<T>>> GetListAsync(int pageNumber, int pageSize)
        {
            // query params
            var queryParams = new Dictionary<string, string>
            {
                { "PageNumber", pageNumber.ToString() },
                { "PageSize", pageSize.ToString() }
            };

            return HttpClient.GetAsync<PagedList<T>>(BuildEndpoint(queryParams: queryParams));
        }

        public virtual Task<ApiResponse<T>> GetByIdAsync(string id, bool details = false)
        {
            var parameters = new Dictionary<string, string>
            {
                { "details", details.ToString().ToLower() }
            };
            return HttpClient.GetByIdAsync<T>(BuildEndpoint(id, parameters));
        }

        public virtual Task<ApiResponse<T>> CreateAsync(T entity)
            => HttpClient.PostAsync<T>(BuildEndpoint(), entity);

        public virtual Task<ApiResponse<T>> UpdateAsync(string id, T entity)
            => HttpClient.PutAsync<T>(BuildEndpoint(id), entity);

        public virtual Task<ApiResponse<bool>> DeleteAsync(string id)
            => HttpClient.DeleteAsync(BuildEndpoint(id));
    }
}