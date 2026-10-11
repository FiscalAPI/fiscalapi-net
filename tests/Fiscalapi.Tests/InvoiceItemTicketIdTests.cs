using System.Net;
using Fiscalapi.Http;
using Fiscalapi.Models;
using Fiscalapi.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Fiscalapi.Tests;

/// <summary>
/// InvoiceItem.TicketId (Auto CFDI): liga cada concepto de la factura global a su ticket. Los mismos escenarios viven en
/// los SDK de Node.js, Python, PHP y Java: sin TicketId el cuerpo de Invoices.CreateAsync es exactamente el de la versión
/// anterior; con él, cada concepto lleva "ticketId" (un ticket puede ir en varios conceptos); un TicketId nulo nunca viaja
/// como null, y el 409 de tickets no disponibles expone data.unavailableTickets en ErrorData.
/// </summary>
public class InvoiceItemTicketIdTests
{
    private const string IssuerId = "4b1c2d3e-0000-4000-8000-000000000002";
    private const string TicketId = "9f8e7d6c-0000-4000-8000-0000000000a1";
    private const string OtherTicketId = "9f8e7d6c-0000-4000-8000-0000000000a2";

    // Cuerpo que enviaba la versión anterior del SDK (sin TicketId en InvoiceItem) para la misma global; fijado con el
    // SDK anterior al cambio. BaseDto siempre serializa createdAt/updatedAt (DateTime no nullable).
    private const string PreviousPayload =
        """{"versionCode":"4.0","series":"G","number":"1001","date":"2026-10-01T10:00:00","paymentFormCode":"01","currencyCode":"MXN","typeCode":"I","expeditionZipCode":"06000","exportCode":"01","issuer":{"id":"4b1c2d3e-0000-4000-8000-000000000002","createdAt":"0001-01-01T00:00:00","updatedAt":"0001-01-01T00:00:00"},"recipient":{"tin":"XAXX010101000","legalName":"PUBLICO EN GENERAL","zipCode":"06000","taxRegimeCode":"616","cfdiUseCode":"S01","createdAt":"0001-01-01T00:00:00","updatedAt":"0001-01-01T00:00:00"},"items":[{"itemCode":"01010101","quantity":1.0,"unitOfMeasurementCode":"ACT","description":"Venta","unitPrice":100.00,"taxObjectCode":"02","itemSku":"F-1001","discount":0.0,"itemTaxes":[{"taxCode":"002","taxTypeCode":"Tasa","taxRate":0.160000,"taxFlagCode":"T"}],"createdAt":"0001-01-01T00:00:00","updatedAt":"0001-01-01T00:00:00"},{"itemCode":"01010101","quantity":1.0,"unitOfMeasurementCode":"ACT","description":"Venta","unitPrice":50.00,"taxObjectCode":"02","itemSku":"F-1001","discount":0.0,"itemTaxes":[{"taxCode":"002","taxTypeCode":"Tasa","taxRate":0.000000,"taxFlagCode":"T"}],"createdAt":"0001-01-01T00:00:00","updatedAt":"0001-01-01T00:00:00"}],"globalInformation":{"periodicityCode":"04","monthCode":"09","year":2026},"exchangeRate":1,"paymentMethodCode":"PUE","subtotal":0.0,"discount":0.0,"total":0.0,"createdAt":"0001-01-01T00:00:00","updatedAt":"0001-01-01T00:00:00"}""";

    private const string UnavailableTicketsResponse = """
        {"data":{"unavailableTickets":[{"ticketId":"9f8e7d6c-0000-4000-8000-0000000000a1","status":"invoiced"},
        {"ticketId":"9f8e7d6c-0000-4000-8000-0000000000a2","status":"globalizing"}]},"succeeded":false,
        "message":"Sorry, it's not me, it's you.",
        "details":"Uno o más tickets ya no están disponibles para la factura global (solo entra un ticket en estado pending): 'data.unavailableTickets' dice cuáles y su estado. No se cobró el timbre ni quedó tomado ningún ticket.",
        "httpStatusCode":409}
        """;

    private readonly FakeApi _api = new();

    private InvoiceService Invoices => new(_api.HttpClient, FakeApi.ApiVersion);

    // Global de un ticket con IVA 16 % y 0 %: un concepto por grupo de impuestos, ambos con el mismo ticket.
    private static Invoice GlobalInvoice(string? firstTicketId = null, string? secondTicketId = null) => new()
    {
        Series = "G",
        Number = "1001",
        Date = new DateTime(2026, 10, 1, 10, 0, 0),
        PaymentFormCode = "01",
        ExpeditionZipCode = "06000",
        Issuer = new InvoiceIssuer { Id = IssuerId },
        Recipient = new InvoiceRecipient
        {
            Tin = "XAXX010101000",
            LegalName = "PUBLICO EN GENERAL",
            ZipCode = "06000",
            TaxRegimeCode = "616",
            CfdiUseCode = "S01",
        },
        GlobalInformation = new GlobalInformation { PeriodicityCode = "04", MonthCode = "09", Year = 2026 },
        Items = new List<InvoiceItem>
        {
            Concept(100.00m, 0.160000m, firstTicketId),
            Concept(50.00m, 0.000000m, secondTicketId),
        },
    };

    private static InvoiceItem Concept(decimal unitPrice, decimal taxRate, string? ticketId) => new()
    {
        ItemCode = "01010101",
        Quantity = 1,
        UnitOfMeasurementCode = "ACT",
        Description = "Venta",
        UnitPrice = unitPrice,
        TaxObjectCode = "02",
        ItemSku = "F-1001",
        ItemTaxes = new List<InvoiceItemTax>
        {
            new() { TaxCode = "002", TaxTypeCode = "Tasa", TaxRate = taxRate, TaxFlagCode = "T" },
        },
        TicketId = ticketId,
    };

    // Los números del cuerpo se leen como decimal para comparar sin pasar por double.
    private static JObject Parse(string json) =>
        JObject.Load(new JsonTextReader(new StringReader(json)) { FloatParseHandling = FloatParseHandling.Decimal });

    private string SentBody()
    {
        var request = Assert.Single(_api.Requests);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal(new Uri(FakeApi.BaseAddress, "api/v4/invoices"), request.Uri);
        return request.Body!;
    }

    [Fact]
    public async Task WithoutTicketId_SendsExactlyThePreviousPayload()
    {
        await Invoices.CreateAsync(GlobalInvoice());

        Assert.Equal(PreviousPayload, SentBody());
    }

    [Fact]
    public async Task WithTicketId_EachItemCarriesItsTicketId_AndNothingElseChanges()
    {
        await Invoices.CreateAsync(GlobalInvoice(TicketId, TicketId));

        var expected = Parse(PreviousPayload);
        foreach (var item in expected["items"]!.Children<JObject>())
            item.Add("ticketId", TicketId);
        Assert.True(JToken.DeepEquals(expected, Parse(SentBody())), SentBody());
    }

    [Fact]
    public async Task OnlyTheItemsWithTicketIdCarryIt()
    {
        await Invoices.CreateAsync(GlobalInvoice(secondTicketId: OtherTicketId));

        var items = (JArray)Parse(SentBody())["items"]!;
        Assert.Null(items[0]["ticketId"]);
        Assert.Equal(OtherTicketId, (string?)items[1]["ticketId"]);
    }

    [Fact]
    public async Task ANullTicketId_IsOmittedEvenWithSettingsThatIncludeNulls()
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Include,
        };
        var httpClient = new FiscalApiHttpClient(new HttpClient(_api) { BaseAddress = FakeApi.BaseAddress }, settings);

        await new InvoiceService(httpClient, FakeApi.ApiVersion).CreateAsync(GlobalInvoice());

        var body = SentBody();
        Assert.Contains("\"number\":\"1001\"", body);
        Assert.Contains("\"pacConfirmation\":null", body); // los demás nulos sí viajan con esos settings
        Assert.DoesNotContain("ticketId", body);
    }

    [Fact]
    public async Task UnavailableTickets_Is409WithTheApiDetailsAndTheTicketsInErrorData()
    {
        _api.RespondWith(UnavailableTicketsResponse, HttpStatusCode.Conflict);

        var response = await Invoices.CreateAsync(GlobalInvoice(TicketId, OtherTicketId));

        Assert.False(response.Succeeded);
        Assert.Equal(409, response.HttpStatusCode);
        Assert.StartsWith("Uno o más tickets ya no están disponibles para la factura global", response.Details);
        Assert.Null(response.Data);
        var unavailable = response.ErrorData!["unavailableTickets"]!
            .Select(ticket => ((string?)ticket["ticketId"], (string?)ticket["status"]));
        Assert.Equal(new[] { (TicketId, "invoiced"), (OtherTicketId, "globalizing") }, unavailable);
    }
}
