using Fiscalapi.Models;
using Fiscalapi.Services;
using Newtonsoft.Json;

namespace Fiscalapi.Tests;

/// <summary>
/// Tolerancia del SDK a respuestas que el API devolverá en las próximas fases (SDK-001).
/// Son pruebas de caracterización: fijan el comportamiento ACTUAL. Una prueba cuyo nombre cita
/// SDK-010 documenta lo que esa tarea cambia. Los casos son:
/// 1. una transacción de timbres con creditType 3 (créditos de ticket, Fase 1);
/// 2. una persona con el campo nuevo availableTicketBalance (Fase 1);
/// 3. propiedades desconocidas en la envoltura, en data y en objetos anidados;
/// 4. globalInformation en la respuesta de una factura (BE-008).
/// </summary>
public class ResponseToleranceTests
{
    private const int TicketCreditType = 3;
    private const string TicketTransactionId = "8d0f6a52-6f1e-4c3a-9a10-000000000103";
    private const string PersonId = "4b1c2d3e-0000-4000-8000-000000000001";
    private const string InvoiceId = "3f2a9c1e-0b7d-4e55-9a61-2c8f0e4d7b10";

    private readonly FakeApi _api = new();

    // 1. creditType 3

    [Fact]
    public async Task StampList_KeepsCreditType3AsAnUndefinedEnumValue_Sdk010AddsTicket()
    {
        _api.RespondWith(FakeApi.ReadFixture("stamps-page-credit-type-3.json"));

        var response = await new StampService(_api.HttpClient, FakeApi.ApiVersion).GetListAsync(1, 10);

        Assert.True(response.Succeeded);
        Assert.Equal(
            new[] { CreditType.Stamp, CreditType.Validation, (CreditType)TicketCreditType },
            response.Data.Items.Select(t => t.CreditType));
        Assert.False(Enum.IsDefined(typeof(CreditType), TicketCreditType));
        Assert.Equal(
            new Uri(FakeApi.BaseAddress, "api/v4/stamps?PageNumber=1&PageSize=10"),
            Assert.Single(_api.RequestedUris));
    }

    [Fact]
    public async Task StampById_KeepsCreditType3AsAnUndefinedEnumValue()
    {
        _api.RespondWith(FakeApi.ReadFixture("stamp-credit-type-3.json"));

        var response = await new StampService(_api.HttpClient, FakeApi.ApiVersion).GetByIdAsync(TicketTransactionId);

        Assert.True(response.Succeeded);
        Assert.Equal(TicketCreditType, (int)response.Data.CreditType);
        Assert.Equal(25, response.Data.Amount);
    }

    // 2. availableTicketBalance

    [Fact]
    public async Task Person_WithAvailableTicketBalance_DeserializesAndDropsTheField_ExposedBySdk010()
    {
        _api.RespondWith(FakeApi.ReadFixture("person-ticket-balance.json"));

        var response = await new PersonService(_api.HttpClient, FakeApi.ApiVersion).GetByIdAsync(PersonId);

        Assert.True(response.Succeeded);
        Assert.Equal(100, response.Data.AvailableBalance);
        Assert.Equal(0, response.Data.CommittedBalance);
        Assert.Equal(50, response.Data.AvailableValidationBalance);
        Assert.DoesNotContain("TicketBalance", JsonConvert.SerializeObject(response.Data), StringComparison.OrdinalIgnoreCase);
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

    // 4. globalInformation en la respuesta de una factura (BE-008)

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
}
