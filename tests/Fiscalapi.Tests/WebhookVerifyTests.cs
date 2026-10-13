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
    [InlineData("abc")]
    public void A_timestamp_that_is_not_unix_seconds_is_rejected(string timestamp)
    {
        var exception = Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(OfficialMoment, OfficialHeaders(timestamp: timestamp), OfficialBody, OfficialSecret));
        Assert.Contains("segundos Unix", exception.Message);
    }

    [Fact]
    public void The_largest_unix_timestamp_is_rejected_by_the_tolerance_without_overflowing()
    {
        var exception = Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(OfficialMoment, OfficialHeaders(timestamp: "9223372036854775807"), OfficialBody, OfficialSecret));
        Assert.Contains("difiere", exception.Message);
    }

    [Fact]
    public void A_timestamp_altered_within_the_tolerance_is_rejected_because_it_is_signed()
    {
        var alteredTimestamp = OfficialTimestamp + 1;

        var exception = Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(DateTimeOffset.FromUnixTimeSeconds(alteredTimestamp),
                OfficialHeaders(timestamp: alteredTimestamp.ToString()), OfficialBody, OfficialSecret));
        Assert.Contains("Ninguna firma", exception.Message);
    }

    [Theory]
    [InlineData("v1a,g0hM9SsE+OTPJTGt/tmIKtSyZlE3uFJELVlNIOLJ1OE=")]
    [InlineData("v2,g0hM9SsE+OTPJTGt/tmIKtSyZlE3uFJELVlNIOLJ1OE=")]
    [InlineData("g0hM9SsE+OTPJTGt/tmIKtSyZlE3uFJELVlNIOLJ1OE=")]
    [InlineData("v1,")]
    [InlineData("v1,!!!")]
    [InlineData("v1,g0hM9SsE+OTPJTGt/tmIKtSyZlE3uFJELVlNIOLJ1OE")]
    public void A_signature_header_without_a_valid_v1_signature_is_rejected(string signature)
    {
        var exception = Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(OfficialMoment, OfficialHeaders(signature: signature), OfficialBody, OfficialSecret));
        Assert.Contains("Ninguna firma", exception.Message);
    }

    [Fact]
    public void A_body_with_a_lone_surrogate_is_rejected_even_when_signed_over_its_replacement_character()
    {
        // Un sustituto suelto no tiene UTF-8: un codificador tolerante lo cambia por U+FFFD y verificaría otros bytes.
        var body = "{\"test\": \"" + (char)0xD800 + "\"}";
        var signature = BackWebhookSigner.Sign(OfficialId, OfficialTimestamp, body, OfficialSecret);

        var exception = Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(OfficialMoment, OfficialHeaders(signature: signature), body, OfficialSecret));
        Assert.IsType<System.Text.EncoderFallbackException>(exception.InnerException);
    }

    [Fact]
    public void Content_after_the_signed_event_is_rejected()
    {
        const string body = "{\"id\":\"evt_1\"} {\"id\":\"evt_2\"}";
        var signature = BackWebhookSigner.Sign(OfficialId, OfficialTimestamp, body, OfficialSecret);

        Assert.Throws<WebhookVerificationException>(() =>
            VerifyAt(OfficialMoment, OfficialHeaders(signature: signature), body, OfficialSecret));
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
    [InlineData("whsec_MfKQ9r8GKYqrTwjUPD8ILPZIo2LaLaS")]
    public void A_malformed_secret_is_refused_as_a_configuration_error(string secret)
    {
        Assert.Throws<ArgumentException>(() => VerifyAt(OfficialMoment, OfficialHeaders(), OfficialBody, secret));
    }

    [Theory]
    [InlineData(OfficialSecret + "\n")]
    [InlineData(OfficialSecret + "\r\n")]
    [InlineData("whsec_MfKQ9r8GKYqr TwjUPD8ILPZIo2LaLaSw")]
    public void A_secret_that_is_not_canonical_base64_is_refused_without_revealing_it(string secret)
    {
        // Los 5 SDK exigen base64 canónico: un salto de línea o un espacio pegados al copiar no se ignoran en silencio.
        var single = Assert.Throws<ArgumentException>(() => VerifyAt(OfficialMoment, OfficialHeaders(), OfficialBody, secret));
        Assert.DoesNotContain("TwjUPD8ILPZIo2LaLaSw", single.Message);

        var inList = Assert.Throws<ArgumentException>(() =>
            Webhook.Verify(OfficialHeaders(), OfficialBody, new[] { OfficialSecret, secret }));
        Assert.Equal("secrets", inList.ParamName);
        Assert.Contains("posición 1", inList.Message);
        Assert.DoesNotContain("TwjUPD8ILPZIo2LaLaSw", inList.Message);
    }

    [Fact]
    public void A_malformed_secret_in_a_list_names_the_parameter_and_its_position_without_its_value()
    {
        const string malformed = "whsec_%%no-es-base64%%";

        var exception = Assert.Throws<ArgumentException>(() =>
            Webhook.Verify(OfficialHeaders(), OfficialBody, new[] { OfficialSecret, malformed }));
        Assert.Equal("secrets", exception.ParamName);
        Assert.Contains("posición 1", exception.Message);
        Assert.DoesNotContain("no-es-base64", exception.Message);

        var nullInList = Assert.Throws<ArgumentNullException>(() =>
            Webhook.Verify(OfficialHeaders(), OfficialBody, new[] { OfficialSecret, null! }));
        Assert.Equal("secrets", nullInList.ParamName);
        Assert.Contains("posición 1", nullInList.Message);

        var single = Assert.Throws<ArgumentException>(() => Webhook.Verify(OfficialHeaders(), OfficialBody, malformed));
        Assert.Equal("secret", single.ParamName);
        Assert.DoesNotContain("no-es-base64", single.Message);
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
