using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fiscalapi.Abstractions;
using Fiscalapi.Common;
using Fiscalapi.Http;
using Fiscalapi.Models;

namespace Fiscalapi.Services
{
    public class TicketService : ITicketService
    {
        private const string IdempotencyKeyHeader = "Idempotency-Key";

        private readonly IFiscalApiHttpClient _httpClient;
        private readonly string _apiVersion;

        public TicketService(IFiscalApiHttpClient httpClient, string apiVersion)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiVersion = apiVersion ?? throw new ArgumentNullException(nameof(apiVersion));
        }

        private string BaseEndpoint => $"api/{_apiVersion}/tickets";

        public Task<ApiResponse<TicketIngestionResult>> CreateAsync(Ticket ticket, string idempotencyKey = null)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket), "No se acepta un ticket nulo");

            if (idempotencyKey == null)
                return _httpClient.PostAsync<TicketIngestionResult>(BaseEndpoint, ticket);

            var headers = new Dictionary<string, string> { [IdempotencyKeyHeader] = idempotencyKey };
            return _httpClient.PostAsync<TicketIngestionResult>(BaseEndpoint, ticket, headers);
        }

        public Task<ApiResponse<PagedList<Ticket>>> GetListAsync(TicketListFilters filters)
        {
            var query = filters?.ToQueryString();
            var endpoint = string.IsNullOrEmpty(query) ? BaseEndpoint : $"{BaseEndpoint}?{query}";
            return _httpClient.GetAsync<PagedList<Ticket>>(endpoint);
        }

        public Task<ApiResponse<Ticket>> GetByIdAsync(string id, bool details = false)
        {
            ValidateId(id);
            return _httpClient.GetAsync<Ticket>($"{BaseEndpoint}/{id}?details={details.ToString().ToLowerInvariant()}");
        }

        public Task<ApiResponse<TicketCodeResult>> GetCodeAsync(string id)
        {
            ValidateId(id);
            return _httpClient.GetAsync<TicketCodeResult>($"{BaseEndpoint}/{id}/code");
        }

        public Task<ApiResponse<bool>> CancelAsync(string id)
        {
            ValidateId(id);
            return _httpClient.DeleteAsync($"{BaseEndpoint}/{id}");
        }

        // Sin id, la ruta sería la de la lista (GET) o la colección (DELETE): se rechaza antes de llamar al API.
        private static void ValidateId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Se requiere el id del ticket", nameof(id));
        }
    }
}
