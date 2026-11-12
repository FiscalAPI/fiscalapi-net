using System.Net.Http;
using Fiscalapi.Models;
using Fiscalapi.Services;

namespace Fiscalapi.Tests;

/// <summary>
/// <c>GET /api/v4/people/{personId}/tax-files</c>: lista de certificados de una persona, solo metadatos.
/// </summary>
public class PersonTaxFilesTests
{
    private const string PersonId = "p1";

    private const string TaxFilesResponse = """
        {"data":[
          {"id":"a1","personId":"p1","tin":"EKU9003173C9","fileType":0,"validFrom":"2023-05-18T11:43:51",
           "validTo":"2027-05-18T11:43:51","sequence":1},
          {"id":"a2","personId":"p1","tin":"EKU9003173C9","fileType":1,"validFrom":"2023-05-18T11:43:51",
           "validTo":"2027-05-18T11:43:51","sequence":1}],
         "succeeded":true,"message":"","details":"","httpStatusCode":200}
        """;

    private readonly FakeApi _api = new();

    [Fact]
    public async Task GetTaxFilesAsync_SendsGetToThePersonTaxFilesPath()
    {
        _api.RespondWith(TaxFilesResponse);

        await new PersonService(_api.HttpClient, FakeApi.ApiVersion).GetTaxFilesAsync(PersonId);

        var request = Assert.Single(_api.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal(new Uri(FakeApi.BaseAddress, "api/v4/people/p1/tax-files"), request.Uri);
        Assert.Null(request.Body);
    }

    [Fact]
    public async Task GetTaxFilesAsync_DeserializesTheCertificateList()
    {
        _api.RespondWith(TaxFilesResponse);

        var response = await new PersonService(_api.HttpClient, FakeApi.ApiVersion).GetTaxFilesAsync(PersonId);

        Assert.True(response.Succeeded);
        Assert.Equal(2, response.Data.Count);

        var csdCertificate = response.Data[0];
        Assert.Equal("a1", csdCertificate.Id);
        Assert.Equal("p1", csdCertificate.PersonId);
        Assert.Equal("EKU9003173C9", csdCertificate.Tin);
        Assert.Equal(FileType.CertificateCsd, csdCertificate.FileType);
        Assert.Equal(new DateTime(2023, 5, 18, 11, 43, 51), csdCertificate.ValidFrom);
        Assert.Equal(new DateTime(2027, 5, 18, 11, 43, 51), csdCertificate.ValidTo);
        Assert.Equal(1, csdCertificate.Sequence);
        Assert.Null(csdCertificate.Base64File);
        Assert.Null(csdCertificate.Password);

        var csdPrivateKey = response.Data[1];
        Assert.Equal("a2", csdPrivateKey.Id);
        Assert.Equal(FileType.PrivateKeyCsd, csdPrivateKey.FileType);
    }
}
