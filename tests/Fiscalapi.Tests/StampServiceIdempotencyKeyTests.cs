using System.Net;
using Fiscalapi.Models;
using Fiscalapi.Services;

namespace Fiscalapi.Tests;

/// <summary>
/// Llave de idempotencia de Stamps.TransferStamps: con key, la petición lleva el header Idempotency-Key con ese valor y el
/// cuerpo no cambia; sin key, el header no se envía.
/// </summary>
public class StampServiceIdempotencyKeyTests
{
    private const string IdempotencyKey = "pos-01-T-0001";

    private const string TransferredResponse =
        """{"data":true,"succeeded":true,"message":"","details":"","httpStatusCode":200}""";

    private readonly FakeApi _api = new();

    private StampService Stamps => new(_api.HttpClient, FakeApi.ApiVersion);

    private static StampTransactionParams NewTransfer() => new()
    {
        FromPersonId = "4b1c2d3e-0000-4000-8000-000000000001",
        ToPersonId = "4b1c2d3e-0000-4000-8000-000000000002",
        Amount = 25
    };

    [Fact]
    public async Task TransferStamps_WithAKey_SendsTheIdempotencyKeyHeader()
    {
        _api.RespondWith(TransferredResponse);

        await Stamps.TransferStamps(NewTransfer(), IdempotencyKey);

        var request = Assert.Single(_api.Requests);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal(new Uri(FakeApi.BaseAddress, "api/v4/stamps"), request.Uri);
        Assert.Equal(IdempotencyKey, request.Headers["Idempotency-Key"]);
        Assert.DoesNotContain(IdempotencyKey, request.Body!);
    }

    [Fact]
    public async Task TransferStamps_WithoutAKey_DoesNotSendTheIdempotencyKeyHeader()
    {
        _api.RespondWith(TransferredResponse);

        await Stamps.TransferStamps(NewTransfer());

        var request = Assert.Single(_api.Requests);
        Assert.False(request.Headers.ContainsKey("Idempotency-Key"));
    }
}
