using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Fiscalapi.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fiscalapi.Http
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

        public async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
        {
            return await SendRequestAsync<T>(HttpMethod.Get, endpoint);
        }

        public async Task<ApiResponse<T>> GetByIdAsync<T>(string id)
        {
            return await SendRequestAsync<T>(HttpMethod.Get, id);
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object payload)
            => await SendRequestAsync<T>(HttpMethod.Post, endpoint, payload);

        public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object payload)
            => await SendRequestAsync<T>(HttpMethod.Put, endpoint, payload);

        public async Task<ApiResponse<T>> PatchAsync<T>(string endpoint, object payload)
            => await SendRequestAsync<T>(new HttpMethod("PATCH"), endpoint, payload);

        public async Task<ApiResponse<bool>> DeleteAsync(string endpoint)
            => await SendRequestAsync<bool>(HttpMethod.Delete, endpoint);

        public async Task<ApiResponse<T>> DeleteAsync<T>(string endpoint, object payload)
        {
            // SendRequestAsync<T>(HttpMethod.Delete, endpoint, payload);   

            return await SendRequestAsync<T>(HttpMethod.Delete, endpoint, payload);
        }


        public async Task<ApiResponse<T>> SendRequestAsync<T>(HttpMethod method, string endpoint, object content = null)
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
                : HandleFailureResponse<T>(responseContent, (int)response.StatusCode);
        }

        private ApiResponse<T> HandleFailureResponse<T>(string responseContent, int statusCode)
        {
            try
            {
                // First try to deserialize as a generic response
                var failureResponse =
                    JsonConvert.DeserializeObject<ApiResponse<object>>(responseContent, _jsonSettings);

                // If status is 400, try to deserialize ValidationFailures
                if (statusCode == 400)
                {
                    var validationResponse =
                        JsonConvert.DeserializeObject<ApiResponse<List<ValidationFailure>>>(responseContent,
                            _jsonSettings);
                    var failures = validationResponse?.Data;

                    var validationErrors = failures != null && failures.Count > 0
                        ? string.Join("; ", failures.Select(x => $"{x.PropertyName}: {x.ErrorMessage}"))
                        : null;

                    return new ApiResponse<T>
                    {
                        Succeeded = false,
                        HttpStatusCode = statusCode,
                        Message = validationResponse?.Message,
                        Details = validationErrors ?? validationResponse?.Details,
                        Data = default
                    };
                }

                // For other error codes (401, 403, 404, 500, etc.)
                return new ApiResponse<T>
                {
                    Succeeded = false,
                    HttpStatusCode = statusCode,
                    Message = failureResponse?.Message,
                    Details = failureResponse?.Details,
                    Data = default
                };
            }
            catch (JsonException)
            {
                // If we can't deserialize the response, create a generic error response
                return new ApiResponse<T>
                {
                    Succeeded = false,
                    HttpStatusCode = statusCode,
                    Message = "Error processing server response",
                    Details = responseContent,
                    Data = default
                };
            }
        }
    }
}