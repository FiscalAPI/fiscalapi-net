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
    private readonly List<RecordedRequest> _requests = new();
    private string _body = string.Empty;
    private HttpStatusCode _status = HttpStatusCode.OK;

    public FakeApi()
    {
        HttpClient = new FiscalApiHttpClient(new HttpClient(this) { BaseAddress = BaseAddress });
    }

    public FiscalApiHttpClient HttpClient { get; }

    public IReadOnlyList<Uri> RequestedUris => _requestedUris;

    /// <summary>Peticiones recibidas: método, URI, cuerpo JSON tal como lo serializó el SDK (null si no lleva) y headers.</summary>
    public IReadOnlyList<RecordedRequest> Requests => _requests;

    public static string ReadFixture(string name) =>
        File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", name));

    public void RespondWith(string body, HttpStatusCode status = HttpStatusCode.OK)
    {
        _body = body;
        _status = status;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _requestedUris.Add(request.RequestUri!);
        var body = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
        var headers = request.Headers.ToDictionary(h => h.Key, h => string.Join(",", h.Value));
        _requests.Add(new RecordedRequest(request.Method, request.RequestUri!, body, headers));
        return new HttpResponseMessage(_status)
        {
            Content = new StringContent(_body, Encoding.UTF8, "application/json")
        };
    }
}

/// <summary>Una petición recibida; <see cref="Headers"/> son los headers propios de la petición (no los del cliente).</summary>
internal sealed record RecordedRequest(HttpMethod Method, Uri Uri, string? Body, IReadOnlyDictionary<string, string> Headers);
