using Fiscalapi.Webhooks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fiscalapi.Tests;

/// <summary>
/// JsonConvert.DefaultSettings es global: estas pruebas lo cambian y corren solas, sin otras clases en paralelo.
/// </summary>
[CollectionDefinition(Name, DisableParallelization = true)]
public class JsonConvertDefaultSettingsCollection
{
    public const string Name = "JsonConvert.DefaultSettings";
}

/// <summary>
/// Webhook.Verify lee el sobre con sus propias opciones: los ajustes globales de Newtonsoft de la aplicación del
/// integrador (JsonConvert.DefaultSettings) no cambian cómo se lee un evento.
/// </summary>
[Collection(JsonConvertDefaultSettingsCollection.Name)]
public class WebhookGlobalJsonSettingsTests
{
    private const long Timestamp = 1791830873; // 2026-10-12T18:47:53Z

    private const string Body =
        """{"id":"evt_1","type":"invoice.canceled","createdAt":"2026-10-12T18:47:53Z","personId":null,"unknown":1,"data":{"relatedObject":{"id":"r1","type":"invoice","url":"/api/v4/invoices/r1"},"status":"X","previousStatus":"C"}}""";

    [Fact]
    public void Hostile_global_json_settings_do_not_change_how_the_event_is_read()
    {
        var secret = BackWebhookSigner.GenerateSecret();
        var headers = BackWebhookSigner.Headers("evt_1", Timestamp, Body, secret);
        var previous = JsonConvert.DefaultSettings;
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Error,
            DateParseHandling = DateParseHandling.None,
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            Converters = { new JavaScriptDateTimeConverter() }
        };
        try
        {
            var webhookEvent = Webhook.Verify(headers, Body, new[] { secret }, Webhook.DefaultTolerance,
                DateTimeOffset.FromUnixTimeSeconds(Timestamp));

            Assert.Equal("evt_1", webhookEvent.Id);
            Assert.Equal(new DateTime(2026, 10, 12, 18, 47, 53, DateTimeKind.Utc), webhookEvent.CreatedAt);
            Assert.Equal(DateTimeKind.Utc, webhookEvent.CreatedAt.Kind);
        }
        finally
        {
            JsonConvert.DefaultSettings = previous;
        }
    }
}
