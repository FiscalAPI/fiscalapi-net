using System.Net;
using Fiscalapi.Models;
using Fiscalapi.Services;

namespace Fiscalapi.Tests;

/// <summary>
/// Llave de idempotencia de Invoices.CreateAsync: si se pasa una, la petición lleva el header Idempotency-Key con ese valor y el
/// cuerpo no cambia; sin llave de idempotencia, el header no se envía; y la sobrecarga de un argumento sigue rechazando una factura nula
/// antes de llamar al API.
/// </summary>
public class InvoiceServiceIdempotencyKeyTests
{
    private const string IdempotencyKey = "pos-01-F-1001";

    private const string CreatedResponse =
        """{"data":null,"succeeded":true,"message":"","details":"","httpStatusCode":201}""";

    private readonly FakeApi _api = new();

    private InvoiceService Invoices => new(_api.HttpClient, FakeApi.ApiVersion);

    private static Invoice NewInvoice() => new() { Series = "A", Number = "1001" };

    [Fact]
    public async Task Create_WithAKey_SendsTheIdempotencyKeyHeader()
    {
        _api.RespondWith(CreatedResponse, HttpStatusCode.Created);

        await Invoices.CreateAsync(NewInvoice(), IdempotencyKey);

        var request = Assert.Single(_api.Requests);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal(new Uri(FakeApi.BaseAddress, "api/v4/invoices"), request.Uri);
        Assert.Equal(IdempotencyKey, request.Headers["Idempotency-Key"]);
        Assert.DoesNotContain(IdempotencyKey, request.Body!);
    }

    [Fact]
    public async Task Create_WithoutAKey_DoesNotSendTheIdempotencyKeyHeader()
    {
        _api.RespondWith(CreatedResponse, HttpStatusCode.Created);

        await Invoices.CreateAsync(NewInvoice());

        var request = Assert.Single(_api.Requests);
        Assert.False(request.Headers.ContainsKey("Idempotency-Key"));
    }

    [Fact]
    public async Task Create_WithANullInvoice_ThrowsBeforeCallingTheApi()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => Invoices.CreateAsync(null!));

        Assert.Empty(_api.Requests);
    }
}
