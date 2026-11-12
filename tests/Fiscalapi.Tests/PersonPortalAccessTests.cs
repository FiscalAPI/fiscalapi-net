using Fiscalapi.Models;
using Fiscalapi.Services;
using Newtonsoft.Json.Linq;

namespace Fiscalapi.Tests;

/// <summary>
/// Acceso al portal de una persona (loginEnabled) y contraseña opcional: sin asignar, ninguno de los dos viaja, así que
/// crear una persona sin contraseña no manda password y un update que no fija LoginEnabled no cambia el acceso.
/// </summary>
public class PersonPortalAccessTests
{
    private const string PersonId = "4b1c2d3e-0000-4000-8000-000000000002";

    private const string PersonResponse = """
        {"data":{"id":"4b1c2d3e-0000-4000-8000-000000000002","legalName":"Persona de Prueba",
        "email":"someone@somewhere.com","loginEnabled":true,"isOwner":false},
        "succeeded":true,"message":"","details":"","httpStatusCode":200}
        """;

    private readonly FakeApi _api = new();

    public PersonPortalAccessTests() => _api.RespondWith(PersonResponse);

    private PersonService Persons => new(_api.HttpClient, FakeApi.ApiVersion);

    private static Person NewPerson() => new() { LegalName = "Persona de Prueba", Email = "someone@somewhere.com" };

    private JObject SentBody() => JObject.Parse(Assert.Single(_api.Requests).Body!);

    [Fact]
    public async Task Create_WithoutPassword_SendsNeitherPasswordNorLoginEnabled()
    {
        await Persons.CreateAsync(NewPerson());

        var body = SentBody();
        Assert.Equal("Persona de Prueba", (string?)body["legalName"]);
        Assert.False(body.ContainsKey("password"));
        Assert.False(body.ContainsKey("loginEnabled"));
    }

    [Fact]
    public async Task Create_WithPortalAccess_SendsPasswordAndLoginEnabled()
    {
        var person = NewPerson();
        person.Password = "YourStrongPassword123!";
        person.LoginEnabled = true;

        await Persons.CreateAsync(person);

        var body = SentBody();
        Assert.Equal("YourStrongPassword123!", (string?)body["password"]);
        Assert.Equal(JTokenType.Boolean, body["loginEnabled"]!.Type);
        Assert.True((bool)body["loginEnabled"]!);
    }

    [Fact]
    public async Task Update_WithoutLoginEnabled_DoesNotSendIt()
    {
        await Persons.UpdateAsync(PersonId, NewPerson());

        var request = Assert.Single(_api.Requests);
        Assert.Equal(HttpMethod.Put, request.Method);
        Assert.Equal($"/api/v4/people/{PersonId}", request.Uri.AbsolutePath);
        Assert.False(SentBody().ContainsKey("loginEnabled"));
    }

    [Fact]
    public async Task Update_WithLoginEnabledFalse_SendsFalse()
    {
        var person = NewPerson();
        person.LoginEnabled = false;

        await Persons.UpdateAsync(PersonId, person);

        var body = SentBody();
        Assert.Equal(JTokenType.Boolean, body["loginEnabled"]!.Type);
        Assert.False((bool)body["loginEnabled"]!);
    }

    [Fact]
    public async Task Response_ReadsTheEffectiveLoginEnabled()
    {
        var response = await Persons.GetByIdAsync(PersonId);

        Assert.True(response.Data.LoginEnabled);
    }
}
