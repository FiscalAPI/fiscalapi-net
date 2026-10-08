using Fiscalapi.Abstractions;
using Fiscalapi.Models;
using Fiscalapi.Services;
using Newtonsoft.Json.Linq;

namespace Fiscalapi.Tests;

/// <summary>
/// Cambios de código del SDK en el ciclo de deuda técnica 2 (Fase III, C22). Los mismos escenarios viven en los SDK de
/// Node.js, Python, PHP y Java (DEC-136), cuando el cambio aplica a cada uno:
/// 1. persona: validTo de solo lectura y committedBalance heredado; CapitalRegime, stripeCustomerId y
///    subscriptionStatus fuera del modelo (SDK-043, SDK-044);
/// 2. certificados: tin opcional al subir (SDK-045) y FileType de la FIEL, 2 y 3 (SDK-058);
/// 3. certificados sin UpdateAsync: el API retiró PUT tax-files (SDK-051); los servicios que sí actualizan lo conservan.
/// </summary>
public class CycleContractTests
{
    private const string PersonId = "4b1c2d3e-0000-4000-8000-000000000002";
    private const string TaxFileId = "7a1b2c3d-0000-4000-8000-0000000000f1";

    private const string TaxFileResponse = """
        {"data":{"id":"7a1b2c3d-0000-4000-8000-0000000000f1","personId":"4b1c2d3e-0000-4000-8000-000000000002",
        "tin":"EKU9003173C9","fileType":2,"sequence":1},"succeeded":true,"message":"","details":"","httpStatusCode":200}
        """;

    private const string DeletedResponse = """{"data":true,"succeeded":true,"message":"","details":"","httpStatusCode":200}""";

    private readonly FakeApi _api = new();

    // 1. Persona (SDK-043, SDK-044)

    [Fact]
    public async Task Person_FromResponse_ReadsValidToAndCommittedBalance_AndDropsTheRetiredFields()
    {
        _api.RespondWith(FakeApi.ReadFixture("person-valid-to.json"));

        var response = await new PersonService(_api.HttpClient, FakeApi.ApiVersion).GetByIdAsync(PersonId);

        Assert.True(response.Succeeded);
        Assert.Equal(new DateTime(2025, 7, 15, 10, 30, 0), response.Data.ValidTo);
        Assert.Equal(0, response.Data.CommittedBalance);
        Assert.Equal(100, response.Data.AvailableBalance);
        Assert.Null(typeof(Person).GetProperty("CapitalRegime"));
        Assert.Null(typeof(Person).GetProperty("StripeCustomerId"));
        Assert.Null(typeof(Person).GetProperty("SubscriptionStatus"));
    }

    [Fact]
    public async Task Person_Create_SendsNeitherValidToNorTheRetiredFields()
    {
        _api.RespondWith(FakeApi.ReadFixture("person-valid-to.json"));
        var person = new Person
        {
            LegalName = "ESCUELA KEMPER URGATE",
            Email = "someone@example.com",
            Password = "UserPass123!",
            Tin = "EKU9003173C9",
        };

        await new PersonService(_api.HttpClient, FakeApi.ApiVersion).CreateAsync(person);

        var request = Assert.Single(_api.Requests);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal(new Uri(FakeApi.BaseAddress, "api/v4/people"), request.Uri);
        var body = JObject.Parse(request.Body!);
        Assert.Equal("ESCUELA KEMPER URGATE", body.Value<string>("legalName"));
        Assert.False(body.ContainsKey("validTo"));
        Assert.False(body.ContainsKey("capitalRegime"));
        Assert.False(body.ContainsKey("stripeCustomerId"));
        Assert.False(body.ContainsKey("subscriptionStatus"));
    }

    // 2. Certificados: tin opcional (SDK-045) y FileType de la FIEL (SDK-058)

    [Fact]
    public void FileType_HasTheApiValues_CsdAndFiel_WithoutPfx()
    {
        Assert.Equal(0, (int)FileType.CertificateCsd);
        Assert.Equal(1, (int)FileType.PrivateKeyCsd);
        Assert.Equal(2, (int)FileType.CertificateFiel);
        Assert.Equal(3, (int)FileType.PrivateKeyFiel);
        Assert.Equal(4, Enum.GetValues(typeof(FileType)).Length);
    }

    [Theory]
    [InlineData(FileType.CertificateFiel, 2)]
    [InlineData(FileType.PrivateKeyFiel, 3)]
    public async Task TaxFile_CreateWithoutTin_SendsTheFileTypeAsANumberAndNoTin(FileType fileType, int expected)
    {
        _api.RespondWith(TaxFileResponse);
        var taxFile = new TaxFile
        {
            PersonId = PersonId,
            Base64File = "MIIFsDCCA5igAwIBAgIUMzAwMDEwMDAwMDA1MDAwMDM0MTYwDQYJKoZIhvcNAQELBQAw",
            FileType = fileType,
            Password = "12345678a",
        };

        var response = await new TaxFileService(_api.HttpClient, FakeApi.ApiVersion).CreateAsync(taxFile);

        Assert.True(response.Succeeded);
        Assert.Equal("EKU9003173C9", response.Data.Tin);
        var request = Assert.Single(_api.Requests);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal(new Uri(FakeApi.BaseAddress, "api/v4/tax-files"), request.Uri);
        var body = JObject.Parse(request.Body!);
        Assert.Equal(JTokenType.Integer, body["fileType"]!.Type);
        Assert.Equal(expected, body.Value<int>("fileType"));
        Assert.False(body.ContainsKey("tin"));
        Assert.Equal(PersonId, body.Value<string>("personId"));
    }

    // 3. Certificados sin UpdateAsync (SDK-051)

    [Fact]
    public void TaxFileService_HasNoUpdateAsync()
    {
        Assert.Null(typeof(ITaxFileService).GetMethod("UpdateAsync"));
        Assert.Null(typeof(TaxFileService).GetMethod("UpdateAsync"));
        Assert.False(typeof(IFiscalApiService<TaxFile>).IsAssignableFrom(typeof(TaxFileService)));
        Assert.True(typeof(IImmutableFiscalApiService<TaxFile>).IsAssignableFrom(typeof(TaxFileService)));
    }

    [Fact]
    public async Task PersonService_KeepsUpdateAsync_AfterTheImmutableServiceSplit()
    {
        _api.RespondWith(FakeApi.ReadFixture("person-valid-to.json"));
        var person = new Person { Id = PersonId, LegalName = "ESCUELA KEMPER URGATE" };

        var response = await new PersonService(_api.HttpClient, FakeApi.ApiVersion).UpdateAsync(PersonId, person);

        Assert.True(response.Succeeded);
        var request = Assert.Single(_api.Requests);
        Assert.Equal(HttpMethod.Put, request.Method);
        Assert.Equal(new Uri(FakeApi.BaseAddress, $"api/v4/people/{PersonId}"), request.Uri);
        Assert.Equal(PersonId, JObject.Parse(request.Body!).Value<string>("id"));
    }

    [Fact]
    public async Task TaxFileService_Delete_SendsDeleteToTheTaxFile()
    {
        _api.RespondWith(DeletedResponse);

        var response = await new TaxFileService(_api.HttpClient, FakeApi.ApiVersion).DeleteAsync(TaxFileId);

        Assert.True(response.Succeeded);
        Assert.True(response.Data);
        var request = Assert.Single(_api.Requests);
        Assert.Equal(HttpMethod.Delete, request.Method);
        Assert.Equal(new Uri(FakeApi.BaseAddress, $"api/v4/tax-files/{TaxFileId}"), request.Uri);
    }
}
