using Fiscalapi.Models;
using Fiscalapi.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fiscalapi.Tests;

/// <summary>
/// Tolerancia del SDK a respuestas del API con valores y campos que el SDK modela o todavía no conoce.
/// Los casos son:
/// 1. transacciones con creditType 3 (CreditType.Ticket) y con un tipo de crédito que el SDK no conoce (99);
/// 2. una persona con el saldo de créditos de ticket en balances;
/// 3. propiedades desconocidas en la envoltura, en data y en objetos anidados;
/// 4. globalInformation en la respuesta de una factura.
/// </summary>
public class ResponseToleranceTests
{
    private const int TicketCreditType = 3;
    private const int UnknownCreditType = 99;
    private const string TicketTransactionId = "8d0f6a52-6f1e-4c3a-9a10-000000000103";
    private const string PersonId = "4b1c2d3e-0000-4000-8000-000000000001";
    private const string InvoiceId = "3f2a9c1e-0b7d-4e55-9a61-2c8f0e4d7b10";

    private readonly FakeApi _api = new();

    // 1. creditType 3 (Ticket) y un tipo desconocido

    [Fact]
    public async Task StampList_ReadsCreditType3AsTicket()
    {
        _api.RespondWith(FakeApi.ReadFixture("stamps-page-credit-type-3.json"));

        var response = await new StampService(_api.HttpClient, FakeApi.ApiVersion).GetListAsync(1, 10);

        Assert.True(response.Succeeded);
        Assert.Equal(
            new[] { CreditType.Stamp, CreditType.Validation, CreditType.Ticket },
            response.Data.Items.Select(t => t.CreditType));
        Assert.True(Enum.IsDefined(typeof(CreditType), TicketCreditType));
        Assert.Equal(
            new Uri(FakeApi.BaseAddress, "api/v4/stamps?PageNumber=1&PageSize=10"),
            Assert.Single(_api.RequestedUris));
    }

    [Fact]
    public async Task StampById_ReadsCreditType3AsTicket()
    {
        _api.RespondWith(FakeApi.ReadFixture("stamp-credit-type-3.json"));

        var response = await new StampService(_api.HttpClient, FakeApi.ApiVersion).GetByIdAsync(TicketTransactionId);

        Assert.True(response.Succeeded);
        Assert.Equal(CreditType.Ticket, response.Data.CreditType);
        Assert.Equal(25, response.Data.Amount);
    }

    [Fact]
    public async Task StampList_KeepsAnUnknownCreditTypeAsAnUndefinedEnumValue()
    {
        _api.RespondWith(WithCreditType(FakeApi.ReadFixture("stamps-page-credit-type-3.json"), UnknownCreditType));

        var response = await new StampService(_api.HttpClient, FakeApi.ApiVersion).GetListAsync(1, 10);

        Assert.True(response.Succeeded);
        Assert.Equal(
            new[] { CreditType.Stamp, CreditType.Validation, (CreditType)UnknownCreditType },
            response.Data.Items.Select(t => t.CreditType));
        Assert.False(Enum.IsDefined(typeof(CreditType), UnknownCreditType));
    }

    [Fact]
    public async Task StampById_KeepsAnUnknownCreditTypeAsAnUndefinedEnumValue()
    {
        _api.RespondWith(WithCreditType(FakeApi.ReadFixture("stamp-credit-type-3.json"), UnknownCreditType));

        var response = await new StampService(_api.HttpClient, FakeApi.ApiVersion).GetByIdAsync(TicketTransactionId);

        Assert.True(response.Succeeded);
        Assert.Equal(UnknownCreditType, (int)response.Data.CreditType);
        Assert.Equal(25, response.Data.Amount);
    }

    [Fact]
    public async Task TransferStamps_WithTicket_SendsCreditType3()
    {
        _api.RespondWith("""{"data":true,"succeeded":true,"message":"","details":"","httpStatusCode":200}""");

        var response = await new StampService(_api.HttpClient, FakeApi.ApiVersion).TransferStamps(new StampTransactionParams
        {
            FromPersonId = "1",
            ToPersonId = PersonId,
            Amount = 25,
            CreditType = CreditType.Ticket
        });

        Assert.True(response.Succeeded);
        var request = Assert.Single(_api.Requests);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal(new Uri(FakeApi.BaseAddress, "api/v4/stamps"), request.Uri);
        Assert.Equal(TicketCreditType, JObject.Parse(request.Body!).Value<int>("creditType"));
    }

    // 2. Saldo de créditos de ticket en balances

    [Fact]
    public async Task Person_ReadsTheTicketBalanceFromBalances()
    {
        _api.RespondWith(FakeApi.ReadFixture("person-ticket-balance.json"));

        var response = await new PersonService(_api.HttpClient, FakeApi.ApiVersion).GetByIdAsync(PersonId);

        Assert.True(response.Succeeded);
        Assert.Equal(100, response.Data.AvailableBalance);
        Assert.Equal(0, response.Data.CommittedBalance);
        Assert.Equal(50, response.Data.AvailableValidationBalance);
        Assert.Equal(
            new[] { (CreditType.Stamp, 100), (CreditType.Validation, 50), (CreditType.Ticket, 25) },
            response.Data.Balances.Select(b => (b.CreditType, b.Available)));
    }

    // 3. Propiedades desconocidas

    [Fact]
    public async Task UnknownProperties_OnTheEnvelopeThePageTheDataAndNestedObjects_AreIgnored()
    {
        _api.RespondWith(FakeApi.ReadFixture("stamps-page-credit-type-3.json"));

        var response = await new StampService(_api.HttpClient, FakeApi.ApiVersion).GetListAsync(1, 10);

        var ticketTransaction = response.Data.Items.Last();
        Assert.True(response.Succeeded);
        Assert.Equal(200, response.HttpStatusCode);
        Assert.Equal(3, response.Data.TotalCount);
        Assert.Equal(TicketTransactionId, ticketTransaction.Id);
        Assert.Equal("FISCALAPI", ticketTransaction.FromPerson.LegalName);
        Assert.DoesNotContain("unknown", JsonConvert.SerializeObject(ticketTransaction), StringComparison.OrdinalIgnoreCase);
    }

    // 4. globalInformation en la respuesta de una factura

    [Fact]
    public async Task Invoice_WithGlobalInformation_DeserializesIt()
    {
        _api.RespondWith(FakeApi.ReadFixture("invoice-global-information.json"));

        var response = await new InvoiceService(_api.HttpClient, FakeApi.ApiVersion).GetByIdAsync(InvoiceId);

        Assert.True(response.Succeeded);
        Assert.Equal("04", response.Data.GlobalInformation.PeriodicityCode);
        Assert.Equal("09", response.Data.GlobalInformation.MonthCode);
        Assert.Equal(2026, response.Data.GlobalInformation.Year);
        Assert.Equal("EKU9003173C9", response.Data.Issuer.Tin);
    }

    [Fact]
    public async Task Invoice_WithNullGlobalInformation_DeserializesItAsNull()
    {
        _api.RespondWith(FakeApi.ReadFixture("invoice-global-information-null.json"));

        var response = await new InvoiceService(_api.HttpClient, FakeApi.ApiVersion).GetByIdAsync(InvoiceId);

        Assert.True(response.Succeeded);
        Assert.Null(response.Data.GlobalInformation);
        Assert.Equal(1160m, response.Data.Total);
    }

    [Fact]
    public async Task Invoice_WithNullMembersInGlobalInformation_IgnoresTheNullsAndLeavesYearAtZero()
    {
        _api.RespondWith(FakeApi.ReadFixture("invoice-global-information-null-members.json"));

        var response = await new InvoiceService(_api.HttpClient, FakeApi.ApiVersion).GetByIdAsync(InvoiceId);

        Assert.True(response.Succeeded);
        Assert.Null(response.Data.GlobalInformation.PeriodicityCode);
        Assert.Null(response.Data.GlobalInformation.MonthCode);
        Assert.Equal(0, response.Data.GlobalInformation.Year);
    }

    private static string WithCreditType(string fixture, int creditType) =>
        fixture.Replace($"\"creditType\": {TicketCreditType}", $"\"creditType\": {creditType}");
}
