using System.Security.Cryptography;
using Fiscalapi.Webhooks;

namespace Fiscalapi.Tests;

/// <summary>
/// Webhook.Verify: la firma Standard Webhooks (HMAC-SHA256 sobre webhook-id.webhook-timestamp.cuerpo) y la tolerancia del
/// timestamp. Los mismos escenarios viven en los SDK de Node.js, Python, PHP y Java. El momento de la verificación se fija
/// con la sobrecarga interna, así el vector oficial (de 2021) verifica sin depender del reloj.
/// </summary>
public class WebhookVerifyTests
{
    // El vector publicado por Standard Webhooks (https://github.com/standard-webhooks/standard-webhooks): no es un secreto.
    private const string OfficialSecret = "whsec_MfKQ9r8GKYqrTwjUPD8ILPZIo2LaLaSw";
    private const string OfficialId = "msg_p5jXN8AQM9LWM0D4loKWxJek";
    private const long OfficialTimestamp = 1614265330;
    private const string OfficialBody = "{\"test\": 2432232314}";
    private const string OfficialSignature = "v1,g0hM9SsE+OTPJTGt/tmIKtSyZlE3uFJELVlNIOLJ1OE=";

    private static readonly DateTimeOffset OfficialMoment = DateTimeOffset.FromUnixTimeSeconds(OfficialTimestamp);

    [Fact]
    public void The_official_vector_verifies()
    {
        var webhookEvent = VerifyAt(OfficialMoment, OfficialHeaders(), OfficialBody, OfficialSecret);

        Assert.NotNull(webhookEvent);
    }

    [Fact]
    public void A_tampered_signature_is_rejected()
    {
        var headers = OfficialHeaders(signature: "v1,g0hM9SsF+OTPJTGt/tmIKtSyZlE3uFJELVlNIOLJ1OE=");

        Assert.Throws<WebhookVerificationException>(() => VerifyAt(OfficialMoment, headers, OfficialBody, OfficialSecret));
    }

    [Fact]
    public void A_tampered_body_is_rejected()
    {
        Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(OfficialMoment, OfficialHeaders(), "{\"test\": 2432232315}", OfficialSecret));
    }

    [Fact]
    public void A_body_reformatted_after_signing_is_rejected()
    {
        // Mismo JSON sin el espacio: verificar exige el cuerpo crudo, no uno reserializado.
        Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(OfficialMoment, OfficialHeaders(), "{\"test\":2432232314}", OfficialSecret));
    }

    [Fact]
    public void A_tampered_id_is_rejected()
    {
        Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(OfficialMoment, OfficialHeaders(id: "msg_otro"), OfficialBody, OfficialSecret));
    }

    [Fact]
    public void A_timestamp_older_than_the_tolerance_is_rejected_and_the_boundary_is_accepted()
    {
        var tolerance = Webhook.DefaultTolerance;

        VerifyAt(OfficialMoment + tolerance, OfficialHeaders(), OfficialBody, OfficialSecret);
        var exception = Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(OfficialMoment + tolerance + TimeSpan.FromSeconds(1), OfficialHeaders(), OfficialBody, OfficialSecret));
        Assert.Contains("webhook-timestamp", exception.Message);
    }

    [Fact]
    public void A_timestamp_further_in_the_future_than_the_tolerance_is_rejected_and_the_boundary_is_accepted()
    {
        var tolerance = Webhook.DefaultTolerance;

        VerifyAt(OfficialMoment - tolerance, OfficialHeaders(), OfficialBody, OfficialSecret);
        Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(OfficialMoment - tolerance - TimeSpan.FromSeconds(1), OfficialHeaders(), OfficialBody, OfficialSecret));
    }

    [Fact]
    public void The_default_tolerance_is_five_minutes()
    {
        Assert.Equal(TimeSpan.FromMinutes(5), Webhook.DefaultTolerance);
    }

    [Fact]
    public void A_custom_tolerance_replaces_the_default()
    {
        var tolerance = TimeSpan.FromSeconds(30);

        VerifyAt(OfficialMoment + tolerance, OfficialHeaders(), OfficialBody, OfficialSecret, tolerance);
        Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(OfficialMoment + TimeSpan.FromSeconds(31), OfficialHeaders(), OfficialBody, OfficialSecret, tolerance));
    }

    [Fact]
    public void A_negative_tolerance_is_refused()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Webhook.Verify(OfficialHeaders(), OfficialBody, OfficialSecret, TimeSpan.FromSeconds(-1)));
    }

    [Theory]
    [InlineData("1614265330.5")]
    [InlineData("-1614265330")]
    [InlineData(" 1614265330")]
    [InlineData("1614265330000000000000")]
    [InlineData("")]
    public void A_timestamp_that_is_not_unix_seconds_is_rejected(string timestamp)
    {
        Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(OfficialMoment, OfficialHeaders(timestamp: timestamp), OfficialBody, OfficialSecret));
    }

    [Theory]
    [InlineData("v1a,g0hM9SsE+OTPJTGt/tmIKtSyZlE3uFJELVlNIOLJ1OE=")]
    [InlineData("v2,g0hM9SsE+OTPJTGt/tmIKtSyZlE3uFJELVlNIOLJ1OE=")]
    [InlineData("g0hM9SsE+OTPJTGt/tmIKtSyZlE3uFJELVlNIOLJ1OE=")]
    [InlineData("")]
    public void A_signature_header_without_a_v1_signature_is_rejected(string signature)
    {
        Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(OfficialMoment, OfficialHeaders(signature: signature), OfficialBody, OfficialSecret));
    }

    [Theory]
    [InlineData("webhook-id")]
    [InlineData("webhook-timestamp")]
    [InlineData("webhook-signature")]
    public void A_missing_header_is_rejected_naming_it(string missing)
    {
        var headers = OfficialHeaders().Where(header => header.Key != missing).ToList();

        var exception = Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(OfficialMoment, headers, OfficialBody, OfficialSecret));
        Assert.Contains(missing, exception.Message);
    }

    [Fact]
    public void Header_names_are_case_insensitive()
    {
        var headers = new Dictionary<string, string>
        {
            ["Webhook-Id"] = OfficialId,
            ["WEBHOOK-TIMESTAMP"] = OfficialTimestamp.ToString(),
            ["Webhook-Signature"] = OfficialSignature
        };

        VerifyAt(OfficialMoment, headers, OfficialBody, OfficialSecret);
    }

    [Fact]
    public void A_secret_without_the_whsec_prefix_verifies()
    {
        VerifyAt(OfficialMoment, OfficialHeaders(), OfficialBody, OfficialSecret.Substring("whsec_".Length));
    }

    [Fact]
    public void Any_valid_v1_signature_among_several_verifies()
    {
        var signature = "v1,AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA= v1a,ignorada malformada " + OfficialSignature;

        VerifyAt(OfficialMoment, OfficialHeaders(signature: signature), OfficialBody, OfficialSecret);
    }

    [Fact]
    public void Any_of_several_secrets_verifies_during_a_rotation()
    {
        var otherSecret = "whsec_" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        VerifyAt(OfficialMoment, OfficialHeaders(), OfficialBody, new[] { otherSecret, OfficialSecret });
        Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(OfficialMoment, OfficialHeaders(), OfficialBody, new[] { otherSecret }));
    }

    [Theory]
    [InlineData("")]
    [InlineData("whsec_")]
    [InlineData("whsec_%%no-es-base64%%")]
    public void A_malformed_secret_is_refused_as_a_configuration_error(string secret)
    {
        Assert.Throws<ArgumentException>(() => VerifyAt(OfficialMoment, OfficialHeaders(), OfficialBody, secret));
    }

    [Fact]
    public void Missing_arguments_are_refused()
    {
        Assert.Throws<ArgumentNullException>(() => Webhook.Verify(null!, OfficialBody, OfficialSecret));
        Assert.Throws<ArgumentNullException>(() => Webhook.Verify(OfficialHeaders(), null!, OfficialSecret));
        Assert.Throws<ArgumentNullException>(() => Webhook.Verify(OfficialHeaders(), OfficialBody, (string)null!));
        Assert.Throws<ArgumentNullException>(() => Webhook.Verify(OfficialHeaders(), OfficialBody, (IEnumerable<string>)null!));
        Assert.Throws<ArgumentException>(() => Webhook.Verify(OfficialHeaders(), OfficialBody, Array.Empty<string>()));
    }

    [Fact]
    public void A_correctly_signed_body_that_is_not_json_is_rejected()
    {
        const string body = "no es json";
        var signature = BackWebhookSigner.Sign(OfficialId, OfficialTimestamp, body, OfficialSecret);

        var exception = Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(OfficialMoment, OfficialHeaders(signature: signature), body, OfficialSecret));
        Assert.NotNull(exception.InnerException);
    }

    [Fact]
    public void The_public_overloads_verify_against_the_current_time()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var headers = OfficialHeaders(timestamp: timestamp.ToString(),
            signature: BackWebhookSigner.Sign(OfficialId, timestamp, OfficialBody, OfficialSecret));

        Assert.NotNull(Webhook.Verify(headers, OfficialBody, OfficialSecret));
        Assert.NotNull(Webhook.Verify(headers, OfficialBody, new[] { OfficialSecret }));
        Assert.Throws<WebhookVerificationException>(() => Webhook.Verify(OfficialHeaders(), OfficialBody, OfficialSecret));
    }

    private static Dictionary<string, string> OfficialHeaders(string id = OfficialId, string? timestamp = null,
        string signature = OfficialSignature) => new()
    {
        ["webhook-id"] = id,
        ["webhook-timestamp"] = timestamp ?? OfficialTimestamp.ToString(),
        ["webhook-signature"] = signature
    };

    private static Models.WebhookEvent VerifyAt(DateTimeOffset now, IEnumerable<KeyValuePair<string, string>> headers,
        string body, string secret, TimeSpan? tolerance = null) =>
        VerifyAt(now, headers, body, new[] { secret }, tolerance);

    private static Models.WebhookEvent VerifyAt(DateTimeOffset now, IEnumerable<KeyValuePair<string, string>> headers,
        string body, IEnumerable<string> secrets, TimeSpan? tolerance = null) =>
        Webhook.Verify(headers, body, secrets, tolerance ?? Webhook.DefaultTolerance, now);
}
