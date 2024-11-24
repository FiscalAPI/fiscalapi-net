using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FiscalApi.Models;
using Newtonsoft.Json;

namespace FiscalApi.Abstractions.Services
{
    public abstract class FiscalApiService
    {
        protected readonly HttpClient HttpClient;
        protected JsonSerializerSettings JsonSettings { get; set; }
        protected string Path { get; set; }

        protected FiscalApiService(HttpClient httpClient)
        {
            HttpClient = httpClient;
            JsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
        }

        protected async Task<ApiResponse<T>> SendAsync<T, TPayload>(HttpMethod method, string endpoint,
            TPayload content = default)
        {
            var request = new HttpRequestMessage(method, endpoint);

            if (content != null)
            {
                string jsonContent = JsonConvert.SerializeObject(content, JsonSettings);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            }

            var response = await HttpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Directly deserialize the API response into ApiResponse<T>
            return JsonConvert.DeserializeObject<ApiResponse<T>>(responseContent, JsonSettings);
        }

        public async Task<ApiResponse<List<T>>> GetAsync<T>(string endpoint, bool details = false)
        {
            var query = $"?details={details.ToString().ToLower()}";
            return await SendAsync<List<T>, object>(HttpMethod.Get, endpoint + query);
        }

        public async Task<ApiResponse<T>> GetByIdAsync<T>(string endpoint, bool details = false)
        {
            var query = $"?details={details.ToString().ToLower()}";
            return await SendAsync<T, object>(HttpMethod.Get, endpoint + query);
        }

        public async Task<ApiResponse<T>> PostAsync<T, TPayload>(string endpoint, TPayload content)
        {
            return await SendAsync<T, TPayload>(HttpMethod.Post, endpoint, content);
        }

        public async Task<ApiResponse<T>> PutAsync<T, TPayload>(string endpoint, TPayload content)
        {
            return await SendAsync<T, TPayload>(HttpMethod.Put, endpoint, content);
        }

        public async Task<ApiResponse<T>> PatchAsync<T, TPayload>(string endpoint, TPayload content)
        {
            //return await SendAsync<T, TPayload>(HttpMethod.Patch, endpoint, content);
            return await SendAsync<T, TPayload>(new HttpMethod("PATCH"), endpoint, content);
        }

        public async Task<ApiResponse<object>> DeleteAsync(string endpoint)
        {
            return await SendAsync<object, object>(HttpMethod.Delete, endpoint);
        }
    }


    //public interface IFiscalApiService
    //{
    //}


    //public abstract class FiscalApiService
    //{
    //    protected readonly HttpClient HttpClient;
    //    protected string Path { get; set; }
    //    protected JsonSerializerSettings JsonSettings { get; set; }

    //    protected FiscalApiService(HttpClient httpClient)
    //    {
    //        HttpClient = httpClient;
    //        JsonSettings = new JsonSerializerSettings
    //        {
    //            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
    //        };
    //    }
    //}
}