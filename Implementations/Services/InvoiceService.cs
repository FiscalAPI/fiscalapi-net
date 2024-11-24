using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FiscalApi.Abstractions.Services;
using FiscalApi.Models;

namespace FiscalApi.Implementations.Services
{
    //public class InvoiceService : FiscalApiService, IInvoiceService
    //{
    //    public InvoiceService(HttpClient client) : base(client)
    //    {
    //        Path = "/api/v4/invoices";
    //    }

    //    // Implement service methods using _httpClient
    //    public Task CreateAsync(Invoice invoice)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public async Task<ApiResponse<Invoice>> GetAsync(string id)
    //    {
    //        var response = await HttpClient.GetAsync($"{Path}/{id}/?details=true");

    //        var contentString = await response.Content.ReadAsStringAsync();


    //        var apiResponse = JsonConvert.DeserializeObject<ApiResponse<Invoice>>(contentString, JsonSettings);

    //        return apiResponse;
    //    }

    //    public Task CancelAsync(string id)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


    public class InvoiceService : FiscalApiService, IInvoiceService
    {
        public InvoiceService(HttpClient client) : base(client)
        {
            Path = "/api/v4/invoices";
        }

        public async Task<ApiResponse<List<Invoice>>> GetListAsync(bool details = false)
        {
            return await GetAsync<Invoice>($"{Path}", details);
        }

        public async Task<ApiResponse<Invoice>> GetByIdAsync(string id, bool details = false)
        {
            return await GetByIdAsync<Invoice>($"{Path}/{id}", details);
        }

        public async Task<ApiResponse<Invoice>> CreateAsync(Invoice payload)
        {
            return await PostAsync<Invoice, Invoice>($"{Path}", payload);
        }

        public async Task<ApiResponse<Invoice>> UpdateAsync(string id, Invoice payload)
        {
            return await PutAsync<Invoice, Invoice>($"{Path}/{id}", payload);
        }

        public async Task<ApiResponse<object>> CancelAsync(string id)
        {
            var payload = new { status = "canceled" };
            return await PatchAsync<object, object>($"{Path}/{id}", payload);
        }
    }
}