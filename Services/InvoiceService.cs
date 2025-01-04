using System;
using FiscalApi.Abstractions;
using FiscalApi.Http;
using FiscalApi.Models.Common;
using FiscalApi.Models.Invoices;
using System.Threading.Tasks;

namespace FiscalApi.Services
{
    public class InvoiceService : BaseFiscalApiService<Invoice>, IInvoiceService
    {
        private const string IncomeEndpoint = "income";
        private const string CreditNoteEndpoint = "credit-note";
        private const string PaymentEndpoint = "payment";

        public InvoiceService(IFiscalApiHttpClient httpClient, string apiVersion)
            : base(httpClient, "invoices", apiVersion)
        {
        }


        public override async Task<ApiResponse<Invoice>> CreateAsync(Invoice entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            string endpoint;

            switch (entity.TypeCode)
            {
                case "I":
                    endpoint = BuildEndpoint(IncomeEndpoint);
                    break;
                case "E":
                    endpoint = BuildEndpoint(CreditNoteEndpoint);
                    break;
                case "P":
                    endpoint = BuildEndpoint(PaymentEndpoint);
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported invoice type: {entity.TypeCode}");
            }

            return await HttpClient.PostAsync<Invoice>(endpoint, entity);
        }


        public Task<ApiResponse<object>> CancelAsync(string id)
            => HttpClient.PostAsync<object>(BuildEndpoint($"{id}/cancel"), null);
    }
}