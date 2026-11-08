using System.Net;
using Fiscalapi.Models;
using Fiscalapi.Services;

namespace Fiscalapi.Tests;

/// <summary>Cancelar factura con rechazo del SAT: 422, Succeeded en false y el acuse en ErrorData.</summary>
public class CancelInvoiceRejectionTests
{
    private const string InvoiceId = "21c39425-9d0a-449b-a7ba-49335b85b062";
    private const string InvoiceUuid = "de841944-bd4f-4bb8-adfe-2a2282787c62";

    private const string SatRejectionResponse = """
        {"data":{"base64CancellationAcknowledgement":"PD94bWwgdmVyc2lvbj0iMS4wIj8+...",
        "invoiceUuids":{"de841944-bd4f-4bb8-adfe-2a2282787c62":"203"}},"succeeded":false,
        "message":"Sorry, it's not me, it's you.",
        "details":"SAT 203: el folio fiscal no corresponde al emisor de la solicitud de cancelación.",
        "httpStatusCode":422}
        """;

    private readonly FakeApi _api = new();

    private InvoiceService Invoices => new(_api.HttpClient, FakeApi.ApiVersion);

    [Fact]
    public async Task Cancel_RejectedBySat_Is422WithTheSatDetailsAndTheAcknowledgementInErrorData()
    {
        _api.RespondWith(SatRejectionResponse, (HttpStatusCode)422);

        var response = await Invoices.CancelAsync(new CancelInvoiceRequest { Id = InvoiceId, CancellationReasonCode = "02" });

        Assert.False(response.Succeeded);
        Assert.Equal(422, response.HttpStatusCode);
        Assert.Equal("Sorry, it's not me, it's you.", response.Message);
        Assert.Equal("SAT 203: el folio fiscal no corresponde al emisor de la solicitud de cancelación.", response.Details);
        Assert.Null(response.Data);
        var rejection = response.ErrorData!.ToObject<CancelInvoiceResponse>()!;
        Assert.Equal("PD94bWwgdmVyc2lvbj0iMS4wIj8+...", rejection.Base64CancellationAcknowledgement);
        Assert.Equal("203", Assert.Single(rejection.InvoiceUuids).Value);
        Assert.Equal(InvoiceUuid, Assert.Single(rejection.InvoiceUuids).Key);
        Assert.Equal(HttpMethod.Delete, Assert.Single(_api.Requests).Method);
    }
}
