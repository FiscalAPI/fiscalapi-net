using System;
using System.Threading.Tasks;
using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;

namespace Fiscalapi.Services
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


        public override async Task<ApiResponse<Invoice>> CreateAsync(Invoice requestModel)
        {
            if (requestModel == null)
                throw new ArgumentNullException(nameof(requestModel));

            string endpoint;

            switch (requestModel.TypeCode)
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
                    throw new InvalidOperationException($"Unsupported invoice type: {requestModel.TypeCode}");
            }

            return await HttpClient.PostAsync<Invoice>(endpoint, requestModel);
        }

        public async Task<ApiResponse<CancelInvoiceResponse>> CancelAsync(CancelInvoiceRequest requestModel)
        {
            if (requestModel == null)
                throw new ArgumentNullException(nameof(requestModel));

            // var endpoint = BuildEndpoint("cancel");

            // POST /api/v4/invoices/cancel
            return await HttpClient.DeleteAsync<CancelInvoiceResponse>(BuildEndpoint(), requestModel);
        }

        public async Task<ApiResponse<FileResponse>> GetPdfAsync(CreatePdfRequest requestModel)
        {
            if (requestModel == null)
                throw new ArgumentNullException(nameof(requestModel));

            // POST /api/v4/invoices/pdf
            return await HttpClient.PostAsync<FileResponse>(BuildEndpoint("pdf"), requestModel);
        }

        public Task<ApiResponse<FileResponse>> GetXmlAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            //GET /api/v4/invoices/{id}/xml
            return HttpClient.GetAsync<FileResponse>(BuildEndpoint($"{id}/xml"));
        }

        public Task<ApiResponse<bool>> SendAsync(SendInvoiceRequest requestModel)
        {
            // POST  /api/v4/invoices/send
            return HttpClient.PostAsync<bool>(BuildEndpoint("send"), requestModel);
        }

        public Task<ApiResponse<InvoiceStatusResponse>> GetStatusAsync(InvoiceStatusRequest requestModel)
        {
            // POST /api/v4/invoices/status
            return HttpClient.PostAsync<InvoiceStatusResponse>(BuildEndpoint("status"), requestModel);
        }
    }
}