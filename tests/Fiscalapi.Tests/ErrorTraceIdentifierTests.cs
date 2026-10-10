using System.Net;
using Fiscalapi.Models;
using Fiscalapi.Services;

namespace Fiscalapi.Tests;

/// <summary>
/// SDK-060 (DEC-184, consistencia DEC-136 con Java): las respuestas de error exponen el traceIdentifier que manda el API
/// (los textos de los 5xx piden enviarlo a soporte). Aditivo: message, details y el status no cambian.
/// </summary>
public class ErrorTraceIdentifierTests
{
    private const string TraceIdentifier = "5C1E0B7A92D44F3E8A6B1D07F3C29E41";

    private const string UnexpectedErrorResponse = """
        {"data":null,"succeeded":false,"message":"Something went wrong on our end.",
        "details":"Ocurrió un error inesperado en el servidor. Escríbenos a soporte@fiscalapi.com con el traceIdentifier de esta respuesta.",
        "httpStatusCode":500,"traceIdentifier":"5C1E0B7A92D44F3E8A6B1D07F3C29E41"}
        """;

    private const string NotFoundResponse = """
        {"data":null,"succeeded":false,"message":"Sorry, it's not me, it's you.",
        "details":"No existe un usuario con RFC EKU9003173C9 en el tenant actual.",
        "httpStatusCode":404,"traceIdentifier":"5C1E0B7A92D44F3E8A6B1D07F3C29E41"}
        """;

    private const string ValidationResponse = """
        {"data":[{"propertyName":"Password","errorMessage":"La contraseña de la llave privada (.key) es incorrecta o el archivo no es una llave privada.","attemptedValue":null}],
        "succeeded":false,"message":"Sorry, it's not me, it's you.","details":"One or more validation failures have occurred.",
        "httpStatusCode":400,"traceIdentifier":"5C1E0B7A92D44F3E8A6B1D07F3C29E41"}
        """;

    private readonly FakeApi _api = new();

    private ManifestService Manifests => new(_api.HttpClient, FakeApi.ApiVersion);

    private static SignManifestRequest SignRequest() => new()
    {
        Base64Cer = "base64CerHere...",
        Base64Key = "base64KeyHere...",
        Password = "passwordPhraseHere...",
    };

    [Fact]
    public async Task ServerError_ExposesTheTraceIdentifier_AndKeepsMessageAndDetails()
    {
        _api.RespondWith(UnexpectedErrorResponse, HttpStatusCode.InternalServerError);

        var response = await Manifests.SignAsync(SignRequest());

        Assert.False(response.Succeeded);
        Assert.Equal(500, response.HttpStatusCode);
        Assert.Equal("Something went wrong on our end.", response.Message);
        Assert.Equal(
            "Ocurrió un error inesperado en el servidor. Escríbenos a soporte@fiscalapi.com con el traceIdentifier de esta respuesta.",
            response.Details);
        Assert.Equal(TraceIdentifier, response.TraceIdentifier);
        Assert.Null(response.Data);
    }

    [Fact]
    public async Task ClientError_ExposesTheTraceIdentifier()
    {
        _api.RespondWith(NotFoundResponse, HttpStatusCode.NotFound);

        var response = await Manifests.SignAsync(SignRequest());

        Assert.Equal(404, response.HttpStatusCode);
        Assert.Equal("Sorry, it's not me, it's you.", response.Message);
        Assert.Equal("No existe un usuario con RFC EKU9003173C9 en el tenant actual.", response.Details);
        Assert.Equal(TraceIdentifier, response.TraceIdentifier);
    }

    [Fact]
    public async Task ValidationError_ExposesTheTraceIdentifier_AndKeepsTheJoinedDetails()
    {
        _api.RespondWith(ValidationResponse, HttpStatusCode.BadRequest);

        var response = await Manifests.SignAsync(SignRequest());

        Assert.Equal(400, response.HttpStatusCode);
        Assert.Equal(
            "Password: La contraseña de la llave privada (.key) es incorrecta o el archivo no es una llave privada.",
            response.Details);
        Assert.Equal(TraceIdentifier, response.TraceIdentifier);
    }

    [Fact]
    public async Task ErrorWithoutTraceIdentifier_LeavesItNull()
    {
        _api.RespondWith("""{"data":null,"succeeded":false,"message":"Too Many Requests","details":"x","httpStatusCode":429}""",
            HttpStatusCode.TooManyRequests);

        var response = await Manifests.SignAsync(SignRequest());

        Assert.Equal(429, response.HttpStatusCode);
        Assert.Null(response.TraceIdentifier);
    }
}
