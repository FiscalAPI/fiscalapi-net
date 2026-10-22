using System.Net;
using Fiscalapi.Models;
using Fiscalapi.Services;

namespace Fiscalapi.Tests;

/// <summary>
/// Servicio de tickets de Auto CFDI (POST/GET/DELETE /api/v4/tickets). Los mismos escenarios viven en los SDK de Node.js,
/// Python, PHP y Java: el cuerpo exacto de la ingesta y el header Idempotency-Key solo si se pasa; la respuesta 201 de la
/// ingesta; el 409 y el 422 con la forma de error del SDK; la query de la lista con status repetido; el detalle con y
/// sin details; el código; la cancelación con DELETE, y un estado que el SDK no conoce, conservado como texto.
/// </summary>
public class TicketServiceTests
{
    private const string IssuerId = "4b1c2d3e-0000-4000-8000-000000000002";
    private const string TicketId = "9f8e7d6c-0000-4000-8000-0000000000a1";
    private const string IdempotencyKey = "550e8400-e29b-41d4-a716-446655440000";

    private const string IngestedResponseTemplate = """
        {"data":{"id":"9f8e7d6c-0000-4000-8000-0000000000a1","ticketCode":"K7M2Q9X4TB3H",
        "qrUrl":"https://portal.fiscalapi.com/t/K7M2Q9X4TB3H","status":"pending","selfInvoiceEligible":true,
        "eligibleUntil":"2026-10-31T23:59:59.000","subtotal":100,"totalTransferredTaxes":16,"totalWithheldTaxes":0,
        "total":116},"succeeded":true,"message":"","details":"","httpStatusCode":STATUS}
        """;

    private const string ConflictResponse = """
        {"data":{"existingTicketId":"9f8e7d6c-0000-4000-8000-0000000000a1"},"succeeded":false,
        "message":"Sorry, it's not me, it's you.",
        "details":"Ya hay un ticket activo con esa serie y folio para el emisor: 'data.existingTicketId' es el ticket que lo tiene. Para volver a ingerirlo, cancela ese ticket primero.",
        "httpStatusCode":409}
        """;

    private const string TotalMismatchResponse = """
        {"data":{"expectedTotal":116.01,"computedTotal":116},"succeeded":false,
        "message":"Sorry, it's not me, it's you.",
        "details":"'total' no cuadra al centavo con el total calculado de los conceptos e impuestos: 'data.expectedTotal' es el enviado y 'data.computedTotal' el calculado.",
        "httpStatusCode":422}
        """;

    private const string NotFoundResponse = """
        {"data":null,"succeeded":false,"message":"Sorry, it's not me, it's you.",
        "details":"No existe el ticket.","httpStatusCode":404}
        """;

    private const string PageResponse = """
        {"data":{"items":[{"id":"9f8e7d6c-0000-4000-8000-0000000000a1","personId":"4b1c2d3e-0000-4000-8000-000000000002",
        "folio":"F-1001","series":"A","paymentDate":"2026-09-01T00:00:00.000","paymentFormCode":"01","currencyCode":"MXN",
        "exchangeRate":null,"expeditionZipCode":"06000","subtotal":100,"totalTransferredTaxes":16,"totalWithheldTaxes":0,
        "total":116,"status":"pending","selfInvoiceEligible":true,"eligibleUntil":"2026-09-30T23:59:59.000",
        "invoiceId":null,"globalInvoiceId":null,"createdAt":"2026-09-01T00:05:00.000"}],
        "pageNumber":2,"totalPages":3,"totalCount":21,"hasPreviousPage":true,"hasNextPage":true},
        "succeeded":true,"message":"","details":"","httpStatusCode":200}
        """;

    private const string SummaryResponse = """
        {"data":{"id":"9f8e7d6c-0000-4000-8000-0000000000a1","personId":"4b1c2d3e-0000-4000-8000-000000000002",
        "folio":"F-1001","series":"A","paymentDate":"2026-10-15T12:30:00.000","paymentFormCode":"01","currencyCode":"MXN",
        "exchangeRate":null,"expeditionZipCode":"06000","subtotal":100,"totalTransferredTaxes":16,"totalWithheldTaxes":0,
        "total":116,"status":"pending","selfInvoiceEligible":true,"eligibleUntil":"2026-10-31T23:59:59.000",
        "invoiceId":null,"globalInvoiceId":null,"createdAt":"2026-10-15T12:31:00.000"},
        "succeeded":true,"message":"","details":"","httpStatusCode":200}
        """;

    private const string DetailResponse = """
        {"data":{"id":"9f8e7d6c-0000-4000-8000-0000000000a1","personId":"4b1c2d3e-0000-4000-8000-000000000002",
        "folio":"F-1001","series":"A","paymentDate":"2026-10-15T12:30:00.000","paymentFormCode":"01","currencyCode":"USD",
        "exchangeRate":17.25,"expeditionZipCode":"06000","subtotal":100,"totalTransferredTaxes":16,"totalWithheldTaxes":0,
        "total":116,"status":"cancelled","selfInvoiceEligible":false,"eligibleUntil":null,"invoiceId":null,
        "globalInvoiceId":null,"createdAt":"2026-10-15T12:31:00.000",
        "items":[{"itemCode":"50202306","itemSku":"SKU-1","quantity":1,"unitOfMeasurementCode":"H87","description":"Refresco",
        "unitPrice":100,"discount":0,"taxObjectCode":"02",
        "itemTaxes":[{"taxCode":"002","taxTypeCode":"Tasa","taxRate":0.16,"taxFlagCode":"T"}]}],
        "events":[{"type":"ingested","occurredAt":"2026-10-15T12:31:00.000","actorType":"tenant","invoiceId":null},
        {"type":"cancelled","occurredAt":"2026-10-15T13:00:00.000","actorType":"tenant","invoiceId":null}]},
        "succeeded":true,"message":"","details":"","httpStatusCode":200}
        """;

    private const string UnknownStateResponse = """
        {"data":{"id":"9f8e7d6c-0000-4000-8000-0000000000a1","personId":"4b1c2d3e-0000-4000-8000-000000000002",
        "folio":"F-1001","series":null,"paymentDate":"2026-10-15T12:30:00.000","paymentFormCode":"01","currencyCode":"MXN",
        "exchangeRate":null,"expeditionZipCode":"06000","subtotal":100,"totalTransferredTaxes":16,"totalWithheldTaxes":0,
        "total":116,"status":"unknownState","selfInvoiceEligible":false,"eligibleUntil":null,"invoiceId":null,
        "globalInvoiceId":null,"createdAt":"2026-10-15T12:31:00.000","items":[],
        "events":[{"type":"unknownEvent","occurredAt":"2026-10-15T12:31:00.000","actorType":"unknownActor","invoiceId":null}]},
        "succeeded":true,"message":"","details":"","httpStatusCode":200}
        """;

    private const string CodeResponse = """
        {"data":{"ticketCode":"K7M2Q9X4TB3H","qrUrl":"https://portal.fiscalapi.com/t/K7M2Q9X4TB3H"},
        "succeeded":true,"message":"","details":"","httpStatusCode":200}
        """;

    private const string DeletedResponse = """{"data":true,"succeeded":true,"message":"","details":"","httpStatusCode":200}""";

    // Lo que serializa el SDK: nombres camelCase, decimales con la escala que les dio el integrador y sin los nulos
    // (opcionales sin asignar y miembros de solo lectura).
    private const string ExpectedCreateBody =
        """{"issuer":{"id":"4b1c2d3e-0000-4000-8000-000000000002"},"folio":"F-1001","series":"A","paymentDate":"2026-10-15T12:30:00","paymentFormCode":"01","currencyCode":"MXN","expeditionZipCode":"06000","total":116.00,"items":[{"itemCode":"50202306","itemSku":"SKU-1","quantity":1.5,"unitOfMeasurementCode":"H87","description":"Refresco","unitPrice":66.666667,"discount":0.00,"taxObjectCode":"02","itemTaxes":[{"taxCode":"002","taxTypeCode":"Tasa","taxRate":0.160000,"taxFlagCode":"T"}]}]}""";

    private readonly FakeApi _api = new();

    private TicketService Tickets => new(_api.HttpClient, FakeApi.ApiVersion);

    private static Ticket NewTicket() => new()
    {
        Issuer = new TicketIssuer { Id = IssuerId },
        Folio = "F-1001",
        Series = "A",
        PaymentDate = new DateTime(2026, 10, 15, 12, 30, 0),
        PaymentFormCode = "01",
        CurrencyCode = "MXN",
        ExpeditionZipCode = "06000",
        Total = 116.00m,
        Items = new List<TicketItem>
        {
            new()
            {
                ItemCode = "50202306",
                ItemSku = "SKU-1",
                Quantity = 1.5m,
                UnitOfMeasurementCode = "H87",
                Description = "Refresco",
                UnitPrice = 66.666667m,
                Discount = 0.00m,
                TaxObjectCode = "02",
                ItemTaxes = new List<TicketItemTax>
                {
                    new() { TaxCode = "002", TaxTypeCode = "Tasa", TaxRate = 0.160000m, TaxFlagCode = "T" }
                }
            }
        }
    };

    private RecordedRequest SingleRequest() => Assert.Single(_api.Requests);

    // a. Cuerpo e Idempotency-Key

    [Fact]
    public async Task Create_PostsTheExactContract_WithTheIdempotencyKeyHeader()
    {
        _api.RespondWith(IngestedResponseTemplate.Replace("STATUS", "201"), HttpStatusCode.Created);

        await Tickets.CreateAsync(NewTicket(), IdempotencyKey);

        var request = SingleRequest();
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("/api/v4/tickets", request.Uri.AbsolutePath);
        Assert.Equal(ExpectedCreateBody, request.Body);
        Assert.Equal(IdempotencyKey, request.Headers["Idempotency-Key"]);
    }

    [Fact]
    public async Task Create_WithoutAKey_DoesNotSendTheIdempotencyKeyHeader()
    {
        _api.RespondWith(IngestedResponseTemplate.Replace("STATUS", "201"), HttpStatusCode.Created);

        await Tickets.CreateAsync(NewTicket());

        var request = SingleRequest();
        Assert.Equal(ExpectedCreateBody, request.Body);
        Assert.False(request.Headers.ContainsKey("Idempotency-Key"));
    }

    // b. Respuesta de la ingesta: la repetición con la misma llave de idempotencia devuelve esta misma 201

    [Fact]
    public async Task Create_ReadsTheNewTicketWithItsShape()
    {
        _api.RespondWith(IngestedResponseTemplate.Replace("STATUS", "201"), HttpStatusCode.Created);

        var response = await Tickets.CreateAsync(NewTicket(), IdempotencyKey);

        Assert.True(response.Succeeded);
        Assert.Equal(201, response.HttpStatusCode);
        Assert.Null(response.ErrorData);
        var ingested = response.Data;
        Assert.Equal(TicketId, ingested.Id);
        Assert.Equal("K7M2Q9X4TB3H", ingested.TicketCode);
        Assert.Equal("https://portal.fiscalapi.com/t/K7M2Q9X4TB3H", ingested.QrUrl);
        Assert.Equal(TicketStatus.Pending, ingested.Status);
        Assert.True(ingested.SelfInvoiceEligible);
        Assert.Equal(new DateTime(2026, 10, 31, 23, 59, 59), ingested.EligibleUntil);
        Assert.Equal((100m, 16m, 0m, 116m),
            (ingested.Subtotal, ingested.TotalTransferredTaxes, ingested.TotalWithheldTaxes, ingested.Total));
    }

    [Fact]
    public async Task Create_DuplicateFolio_Is409WithTheApiDetailsAndExistingTicketIdInErrorData()
    {
        _api.RespondWith(ConflictResponse, HttpStatusCode.Conflict);

        var response = await Tickets.CreateAsync(NewTicket());

        Assert.False(response.Succeeded);
        Assert.Equal(409, response.HttpStatusCode);
        Assert.Equal("Sorry, it's not me, it's you.", response.Message);
        Assert.StartsWith("Ya hay un ticket activo con esa serie y folio para el emisor", response.Details);
        Assert.Null(response.Data);
        Assert.Equal(TicketId, (string?)response.ErrorData!["existingTicketId"]);
    }

    [Fact]
    public async Task Create_TotalMismatch_Is422WithTheApiDetailsAndTheTotalsInErrorData()
    {
        _api.RespondWith(TotalMismatchResponse, (HttpStatusCode)422);

        var response = await Tickets.CreateAsync(NewTicket());

        Assert.False(response.Succeeded);
        Assert.Equal(422, response.HttpStatusCode);
        Assert.StartsWith("'total' no cuadra al centavo", response.Details);
        Assert.Null(response.Data);
        Assert.Equal((116.01m, 116m),
            ((decimal)response.ErrorData!["expectedTotal"]!, (decimal)response.ErrorData["computedTotal"]!));
    }

    [Fact]
    public async Task AnErrorWithoutData_LeavesErrorDataNull()
    {
        _api.RespondWith(NotFoundResponse, HttpStatusCode.NotFound);

        var response = await Tickets.GetByIdAsync(TicketId);

        Assert.False(response.Succeeded);
        Assert.Equal(404, response.HttpStatusCode);
        Assert.Null(response.ErrorData);
    }

    // c. Lista con filtros

    [Fact]
    public async Task GetList_SendsEveryFilter_RepeatingStatus()
    {
        _api.RespondWith(PageResponse);

        var response = await Tickets.GetListAsync(new TicketListFilters
        {
            Status = new List<string> { TicketStatus.Pending, TicketStatus.Invoiced },
            PersonId = IssuerId,
            From = new DateTime(2026, 9, 1),
            To = new DateTime(2026, 9, 30, 18, 45, 0),
            PageNumber = 2,
            PageSize = 50
        });

        var request = SingleRequest();
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/api/v4/tickets", request.Uri.AbsolutePath);
        Assert.Equal(
            $"?status=pending&status=invoiced&personId={IssuerId}&from=2026-09-01&to=2026-09-30&pageNumber=2&pageSize=50",
            request.Uri.Query);
        var page = response.Data;
        Assert.Equal((2, 3, 21), (page.PageNumber, page.TotalPages, page.TotalCount));
        var ticket = Assert.Single(page.Items);
        Assert.Equal(IssuerId, ticket.PersonId);
        Assert.Equal(new DateTime(2026, 9, 1), ticket.PaymentDate);
        Assert.Equal(TicketStatus.Pending, ticket.Status);
        Assert.Null(ticket.ExchangeRate);
        Assert.Null(ticket.Items);
        Assert.Null(ticket.Events);
    }

    [Fact]
    public async Task GetList_WithDetailsAndGlobalInvoiceId_SendsBoth()
    {
        _api.RespondWith(PageResponse);

        await Tickets.GetListAsync(new TicketListFilters
        {
            Details = true,
            GlobalInvoiceId = "7c1d2e3f-0000-4000-8000-0000000000b2"
        });

        Assert.Equal(
            "?details=true&globalInvoiceId=7c1d2e3f-0000-4000-8000-0000000000b2",
            SingleRequest().Uri.Query);
    }

    [Fact]
    public async Task GetList_DetailsFalse_IsNotSent()
    {
        _api.RespondWith(PageResponse);

        await Tickets.GetListAsync(new TicketListFilters { Details = false });

        Assert.Equal("", SingleRequest().Uri.Query);
    }

    [Fact]
    public async Task GetList_WithoutFilters_SendsNoQuery()
    {
        _api.RespondWith(PageResponse);

        await Tickets.GetListAsync(new TicketListFilters());

        Assert.Equal("", SingleRequest().Uri.Query);
    }

    // d. Detalle

    [Fact]
    public async Task GetById_WithoutDetails_AsksWithoutDetails()
    {
        _api.RespondWith(SummaryResponse);

        var response = await Tickets.GetByIdAsync(TicketId);

        var request = SingleRequest();
        Assert.Equal($"/api/v4/tickets/{TicketId}", request.Uri.AbsolutePath);
        Assert.Equal("?details=false", request.Uri.Query);
        Assert.Equal(TicketId, response.Data.Id);
        Assert.Null(response.Data.Items);
    }

    [Fact]
    public async Task GetById_WithDetails_ReadsItemsAndEvents()
    {
        _api.RespondWith(DetailResponse);

        var response = await Tickets.GetByIdAsync(TicketId, details: true);

        Assert.Equal("?details=true", SingleRequest().Uri.Query);
        var ticket = response.Data;
        Assert.Equal(TicketStatus.Cancelled, ticket.Status);
        Assert.Equal(17.25m, ticket.ExchangeRate);
        Assert.Equal(new DateTime(2026, 10, 15, 12, 30, 0), ticket.PaymentDate);
        var item = Assert.Single(ticket.Items);
        Assert.Equal(("50202306", "SKU-1", 1m, 100m, 0m), (item.ItemCode, item.ItemSku, item.Quantity, item.UnitPrice, item.Discount));
        var tax = Assert.Single(item.ItemTaxes);
        Assert.Equal(("002", "Tasa", 0.16m, "T"), (tax.TaxCode, tax.TaxTypeCode, tax.TaxRate, tax.TaxFlagCode));
        Assert.Equal(new[] { TicketEventType.Ingested, TicketEventType.Cancelled }, ticket.Events.Select(e => e.Type));
        Assert.All(ticket.Events, e => Assert.Equal(TicketActorType.Tenant, e.ActorType));
        Assert.Equal(new DateTime(2026, 10, 15, 13, 0, 0), ticket.Events[1].OccurredAt);
    }

    // e. Código

    [Fact]
    public async Task GetCode_ReadsTheCodeAndTheQrUrl()
    {
        _api.RespondWith(CodeResponse);

        var response = await Tickets.GetCodeAsync(TicketId);

        var request = SingleRequest();
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal($"/api/v4/tickets/{TicketId}/code", request.Uri.AbsolutePath);
        Assert.Equal(("K7M2Q9X4TB3H", "https://portal.fiscalapi.com/t/K7M2Q9X4TB3H"),
            (response.Data.TicketCode, response.Data.QrUrl));
    }

    // f. Cancelación

    [Fact]
    public async Task Cancel_UsesDelete()
    {
        _api.RespondWith(DeletedResponse);

        var response = await Tickets.CancelAsync(TicketId);

        var request = SingleRequest();
        Assert.Equal(HttpMethod.Delete, request.Method);
        Assert.Equal($"/api/v4/tickets/{TicketId}", request.Uri.AbsolutePath);
        Assert.Null(request.Body);
        Assert.True(response.Data);
    }

    // g. Valores que el SDK no conoce

    [Fact]
    public async Task GetById_KeepsAStatusEventTypeAndActorTypeThatTheSdkDoesNotKnow()
    {
        _api.RespondWith(UnknownStateResponse);

        var response = await Tickets.GetByIdAsync(TicketId, details: true);

        Assert.True(response.Succeeded);
        Assert.Equal("unknownState", response.Data.Status);
        var ticketEvent = Assert.Single(response.Data.Events);
        Assert.Equal(("unknownEvent", "unknownActor"), (ticketEvent.Type, ticketEvent.ActorType));
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    public async Task EveryCallById_RejectsAMissingIdWithoutCallingTheApi(string? id)
    {
        await Assert.ThrowsAsync<ArgumentException>(() => Tickets.GetByIdAsync(id!));
        await Assert.ThrowsAsync<ArgumentException>(() => Tickets.GetCodeAsync(id!));
        await Assert.ThrowsAsync<ArgumentException>(() => Tickets.CancelAsync(id!));
        Assert.Empty(_api.Requests);
    }
}
