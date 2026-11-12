using Fiscalapi.Common;
using Fiscalapi.Models;
using Fiscalapi.Services;

namespace Fiscalapi.Tests;

/// <summary>
/// GET /api/v4/me: la persona de la credencial con sus features y saldos, con los nombres de MeDto del back.
/// </summary>
public class MeServiceTests
{
    private const string MeResponse = """
        {"data":{"id":"78d380fd-1b69-4e3c-8bc0-4f57737f7d5f","legalName":"KARLA FUENTE NOLASCO","tin":"FUNK671228PH6",
        "email":"persona@example.com","tenantId":"102e5f13-e114-41dd-bea7-507fce177281","environment":"Test","isOwner":true,
        "features":{"isOwner":true,"roles":["Owner"],"permissions":["Invoices.Read","Invoices.Issue"],
        "denied":{"Tickets.Create":{"missingFeatures":["auto_cfdi"]}},
        "features":{"emision_cfdi":{"values":{}},"descarga_cfdi":{"values":{"seats":2}}},
        "subscription":{"status":"active","isAvailable":true},
        "seatAssignments":[{"featureCode":"descarga_cfdi","units":1,"status":"Active"}]},
        "balances":[{"creditType":1,"available":7},{"creditType":2,"available":3}]},
        "succeeded":true,"message":"","details":"","httpStatusCode":200}
        """;

    private readonly FakeApi _api = new();

    private MeService Me => new(_api.HttpClient, FakeApi.ApiVersion);

    [Fact]
    public async Task GetAsync_SendsGetToTheMePath()
    {
        _api.RespondWith(MeResponse);

        await Me.GetAsync();

        var request = Assert.Single(_api.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal(new Uri(FakeApi.BaseAddress, "api/v4/me"), request.Uri);
        Assert.Null(request.Body);
    }

    [Fact]
    public async Task GetAsync_DeserializesThePersonFeaturesAndBalances()
    {
        _api.RespondWith(MeResponse);

        var response = await Me.GetAsync();

        Assert.True(response.Succeeded);
        var me = response.Data;
        Assert.Equal("78d380fd-1b69-4e3c-8bc0-4f57737f7d5f", me.Id);
        Assert.Equal("KARLA FUENTE NOLASCO", me.LegalName);
        Assert.Equal("FUNK671228PH6", me.Tin);
        Assert.Equal("persona@example.com", me.Email);
        Assert.Equal("102e5f13-e114-41dd-bea7-507fce177281", me.TenantId);
        Assert.Equal("Test", me.Environment);
        Assert.True(me.IsOwner);

        var features = me.Features;
        Assert.True(features.IsOwner);
        Assert.Equal(new[] { "Owner" }, features.Roles);
        Assert.Equal(new[] { "Invoices.Read", "Invoices.Issue" }, features.Permissions);
        Assert.Equal(new[] { "auto_cfdi" }, Assert.Single(features.Denied, d => d.Key == "Tickets.Create").Value.MissingFeatures);
        Assert.Empty(features.Features["emision_cfdi"].Values);
        Assert.Equal(2, features.Features["descarga_cfdi"].Values["seats"]);
        Assert.Equal("active", features.Subscription.Status);
        Assert.True(features.Subscription.IsAvailable);
        var seat = Assert.Single(features.SeatAssignments);
        Assert.Equal(("descarga_cfdi", 1, "Active"), (seat.FeatureCode, seat.Units, seat.Status));

        Assert.Equal(
            new[] { (CreditType.Stamp, 7), (CreditType.Validation, 3) },
            me.Balances.Select(b => (b.CreditType, b.Available)));
    }

    [Fact]
    public void Client_ExposesTheMeService()
    {
        var client = FiscalApiClient.Create(new FiscalapiSettings
        {
            ApiUrl = FakeApi.BaseAddress.ToString(),
            ApiKey = "sk_test_sdk",
            ApiVersion = FakeApi.ApiVersion,
            Tenant = "tenant"
        });

        Assert.IsType<MeService>(client.Me);
    }
}
