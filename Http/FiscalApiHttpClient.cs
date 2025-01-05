using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FiscalApi.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FiscalApi.Http
{
    public class FiscalApiHttpClient : IFiscalApiHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerSettings _jsonSettings;

        public FiscalApiHttpClient(HttpClient httpClient, JsonSerializerSettings jsonSettings = null)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _jsonSettings = jsonSettings ?? new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string endpoint, bool includeDetails = false)
        {
            var query = includeDetails ? $"?details=true" : string.Empty;
            return await SendRequestAsync<T>(HttpMethod.Get, $"{endpoint}{query}");
        }

        public async Task<ApiResponse<T>> GetByIdAsync<T>(string id, bool includeDetails = false)
        {
            var query = includeDetails ? $"?details=true" : string.Empty;
            return await SendRequestAsync<T>(HttpMethod.Get, $"{id}{query}");
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object payload)
            => await SendRequestAsync<T>(HttpMethod.Post, endpoint, payload);

        public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object payload)
            => await SendRequestAsync<T>(HttpMethod.Put, endpoint, payload);

        public async Task<ApiResponse<T>> PatchAsync<T>(string endpoint, object payload)
            => await SendRequestAsync<T>(new HttpMethod("PATCH"), endpoint, payload);

        public async Task<ApiResponse<bool>> DeleteAsync(string endpoint)
            => await SendRequestAsync<bool>(HttpMethod.Delete, endpoint);

        private async Task<ApiResponse<T>> SendRequestAsync<T>(HttpMethod method, string endpoint,
            object content = null)
        {
            var request = new HttpRequestMessage(method, endpoint);

            if (content != null)
            {
                var json = JsonConvert.SerializeObject(content, _jsonSettings);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();


            return response.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<ApiResponse<T>>(responseContent, _jsonSettings)
                : HandleFailureAsync<T>(responseContent);
        }


        private ApiResponse<T> HandleFailureAsync<T>(string responseContent)
        {
            var failureResponse =
                JsonConvert.DeserializeObject<ApiResponse<List<ValidationFailure>>>(responseContent, _jsonSettings);

            var failures = failureResponse.Data;


            var friendlyErrorMessage = "";
            if (failures != null && failures.Count > 0)
            {
                friendlyErrorMessage = string.Join("; ",
                    failures.Select(x => $"{x.PropertyName}: {x.ErrorMessage}"));
            }

            // Retornamos un nuevo ApiResponse<T> con Data = null y los mensajes
            return new ApiResponse<T>
            {
                Succeeded = false,
                HttpStatusCode = failureResponse.HttpStatusCode,
                Message = failureResponse.Message,
                Details = !string.IsNullOrEmpty(friendlyErrorMessage) ? friendlyErrorMessage : failureResponse.Details,
                Data = default
            };
        }
    }
}