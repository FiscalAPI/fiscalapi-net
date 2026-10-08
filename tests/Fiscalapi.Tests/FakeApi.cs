using System.Net;
using System.Text;
using Fiscalapi.Http;

namespace Fiscalapi.Tests;

/// <summary>
/// API simulada: un <see cref="HttpMessageHandler"/> responde a cada petición con el cuerpo configurado,
/// así que el SDK recorre su camino real (servicio -> <see cref="FiscalApiHttpClient"/> -> Newtonsoft)
/// sin red.
/// </summary>
internal sealed class FakeApi : HttpMessageHandler
{
    public const string ApiVersion = "v4";
    public static readonly Uri BaseAddress = new("https://sdk-tests.fiscalapi.invalid/");

    private readonly List<Uri> _requestedUris = new();
    private string _body = string.Empty;

    public FakeApi()
    {
        HttpClient = new FiscalApiHttpClient(new HttpClient(this) { BaseAddress = BaseAddress });
    }

    public FiscalApiHttpClient HttpClient { get; }

    public IReadOnlyList<Uri> RequestedUris => _requestedUris;

    public static string ReadFixture(string name) =>
        File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", name));

    public void RespondWith(string body) => _body = body;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _requestedUris.Add(request.RequestUri!);
        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(_body, Encoding.UTF8, "application/json")
        });
    }
}
