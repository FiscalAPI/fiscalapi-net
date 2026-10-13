using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Fiscalapi.Models;
using Fiscalapi.Webhooks;

namespace Fiscalapi.Tests;

/// <summary>
/// Interoperabilidad con el back: una entrega firmada como la firma WebhookSigner de fapi-back (dos secretos durante una
/// rotación, el vigente primero) y con el sobre tal como lo serializa WebhookEnvelopeSerializer (camelCase, sin espacios,
/// createdAt al segundo con Z y las nulas presentes) verifica con Webhook.Verify y se lee completa en WebhookEvent.
/// </summary>
public class WebhookBackInteropTests
{
    private const string EventId = "0f6b2c1e-7d3a-4e8b-9c51-2a4d6e8f0b13";
    private const string PersonId = "4b1c2d3e-0000-4000-8000-000000000002";
    private const string InvoiceId = "9f8e7d6c-0000-4000-8000-0000000000a1";
    private const string DownloadRequestId = "3c2b1a09-0000-4000-8000-0000000000b7";
    private const long Timestamp = 1791830873; // 2026-10-12T18:47:53Z, el createdAt de los sobres

    // Los sobres de WebhookEnvelopeTests de fapi-back, con ids fijos.
    private const string CanceledInvoiceBody =
        """{"id":"0f6b2c1e-7d3a-4e8b-9c51-2a4d6e8f0b13","type":"invoice.canceled","createdAt":"2026-10-12T18:47:53Z","personId":"4b1c2d3e-0000-4000-8000-000000000002","data":{"relatedObject":{"id":"9f8e7d6c-0000-4000-8000-0000000000a1","type":"invoice","url":"/api/v4/invoices/9f8e7d6c-0000-4000-8000-0000000000a1"},"status":"X","previousStatus":"C"}}""";

    private const string CompletedDownloadRequestBody =
        """{"id":"0f6b2c1e-7d3a-4e8b-9c51-2a4d6e8f0b13","type":"download_request.completed","createdAt":"2026-10-12T18:47:53Z","personId":null,"data":{"relatedObject":{"id":"3c2b1a09-0000-4000-8000-0000000000b7","type":"download_request","url":"/api/v4/download-requests/3c2b1a09-0000-4000-8000-0000000000b7"},"status":null,"previousStatus":null}}""";

    private static readonly DateTimeOffset DeliveredAt = DateTimeOffset.FromUnixTimeSeconds(Timestamp);

    [Fact]
    public void A_back_delivery_signed_during_a_rotation_verifies_with_either_secret_and_reads_every_field()
    {
        var current = BackWebhookSigner.GenerateSecret();
        var previous = BackWebhookSigner.GenerateSecret();
        var headers = BackWebhookSigner.Headers(EventId, Timestamp, CanceledInvoiceBody, current, previous);

        foreach (var receiverSecret in new[] { current, previous })
        {
            var webhookEvent = Webhook.Verify(headers, CanceledInvoiceBody, new[] { receiverSecret },
                Webhook.DefaultTolerance, DeliveredAt);

            Assert.Equal(EventId, webhookEvent.Id);
            Assert.Equal(WebhookEventTypes.InvoiceCanceled, webhookEvent.Type);
            Assert.Equal(new DateTime(2026, 10, 12, 18, 47, 53, DateTimeKind.Utc), webhookEvent.CreatedAt);
            Assert.Equal(DateTimeKind.Utc, webhookEvent.CreatedAt.Kind);
            Assert.Equal(PersonId, webhookEvent.PersonId);
            Assert.Equal(InvoiceId, webhookEvent.Data.RelatedObject.Id);
            Assert.Equal("invoice", webhookEvent.Data.RelatedObject.Type);
            Assert.Equal("/api/v4/invoices/" + InvoiceId, webhookEvent.Data.RelatedObject.Url);
            Assert.Equal("X", webhookEvent.Data.Status);
            Assert.Equal("C", webhookEvent.Data.PreviousStatus);
        }
    }

    [Fact]
    public void A_back_delivery_without_person_or_statuses_reads_them_as_null()
    {
        var secret = BackWebhookSigner.GenerateSecret();
        var headers = BackWebhookSigner.Headers(EventId, Timestamp, CompletedDownloadRequestBody, secret);

        var webhookEvent = Webhook.Verify(headers, CompletedDownloadRequestBody, new[] { secret },
            Webhook.DefaultTolerance, DeliveredAt);

        Assert.Equal(WebhookEventTypes.DownloadRequestCompleted, webhookEvent.Type);
        Assert.Null(webhookEvent.PersonId);
        Assert.Equal(DownloadRequestId, webhookEvent.Data.RelatedObject.Id);
        Assert.Equal("download_request", webhookEvent.Data.RelatedObject.Type);
        Assert.Equal("/api/v4/download-requests/" + DownloadRequestId, webhookEvent.Data.RelatedObject.Url);
        Assert.Null(webhookEvent.Data.Status);
        Assert.Null(webhookEvent.Data.PreviousStatus);
    }

    [Fact]
    public void An_unknown_type_and_unknown_fields_are_read_without_error()
    {
        const string body =
            """{"id":"evt_1","type":"invoice.future_state","createdAt":"2026-10-12T18:47:53Z","personId":null,"extra":1,"data":{"relatedObject":{"id":"r1","type":"future_resource","url":"/api/v4/future/r1","extra":true},"status":"Z","previousStatus":null}}""";
        var secret = BackWebhookSigner.GenerateSecret();
        var headers = BackWebhookSigner.Headers("evt_1", Timestamp, body, secret);

        var webhookEvent = Webhook.Verify(headers, body, new[] { secret }, Webhook.DefaultTolerance, DeliveredAt);

        Assert.Equal("invoice.future_state", webhookEvent.Type);
        Assert.Equal("future_resource", webhookEvent.Data.RelatedObject.Type);
    }

    [Fact]
    public void The_event_type_constants_are_the_six_types_of_the_back()
    {
        Assert.Equal("invoice.cancellation_requested", WebhookEventTypes.InvoiceCancellationRequested);
        Assert.Equal("invoice.canceled", WebhookEventTypes.InvoiceCanceled);
        Assert.Equal("invoice.cancellation_rejected", WebhookEventTypes.InvoiceCancellationRejected);
        Assert.Equal("download_request.completed", WebhookEventTypes.DownloadRequestCompleted);
        Assert.Equal("download_request.abandoned", WebhookEventTypes.DownloadRequestAbandoned);
        Assert.Equal("download_rule.suspended", WebhookEventTypes.DownloadRuleSuspended);
    }
}

/// <summary>
/// La firma de WebhookSigner de fapi-back, copiada sin depender del back: HMAC-SHA256 sobre {id}.{timestamp}.{body} en
/// UTF-8 con los bytes base64 del secreto (sin whsec_), una firma "v1,&lt;base64&gt;" por secreto unidas por un espacio.
/// </summary>
internal static class BackWebhookSigner
{
    private const string SecretPrefix = "whsec_";

    /// <summary>Un secreto como WebhookSecret.Generate del back: whsec_ + 32 bytes aleatorios en base64.</summary>
    public static string GenerateSecret() => SecretPrefix + Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

    public static string Sign(string id, long timestamp, string body, params string[] secrets)
    {
        var content = Encoding.UTF8.GetBytes($"{id}.{timestamp.ToString(CultureInfo.InvariantCulture)}.{body}");
        return string.Join(' ', secrets.Select(secret =>
            "v1," + Convert.ToBase64String(HMACSHA256.HashData(KeyBytes(secret), content))));
    }

    public static Dictionary<string, string> Headers(string id, long timestamp, string body, params string[] secrets) => new()
    {
        ["webhook-id"] = id,
        ["webhook-timestamp"] = timestamp.ToString(CultureInfo.InvariantCulture),
        ["webhook-signature"] = Sign(id, timestamp, body, secrets)
    };

    private static byte[] KeyBytes(string secret) => Convert.FromBase64String(secret.Substring(SecretPrefix.Length));
}
