using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Fiscalapi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fiscalapi.Webhooks
{
    /// <summary>
    /// Verificación de las entregas de webhooks de FiscalAPI, que se firman con Standard Webhooks
    /// (https://www.standardwebhooks.com): HMAC-SHA256 sobre <c>webhook-id.webhook-timestamp.cuerpo</c> con el secreto
    /// <c>whsec_…</c> del endpoint, en el header <c>webhook-signature</c> como <c>v1,&lt;base64&gt;</c>. La firma es la
    /// única prueba de que una entrega viene de FiscalAPI.
    /// </summary>
    public static class Webhook
    {
        /// <summary>
        /// Diferencia máxima entre <c>webhook-timestamp</c> y el reloj del receptor, hacia atrás o hacia adelante: una
        /// entrega capturada no se puede repetir después.
        /// </summary>
        public static readonly TimeSpan DefaultTolerance = TimeSpan.FromMinutes(5);

        private const string IdHeader = "webhook-id";
        private const string TimestampHeader = "webhook-timestamp";
        private const string SignatureHeader = "webhook-signature";
        private const string SignatureVersion = "v1";
        private const char SignatureSeparator = ' ';
        private const char VersionSeparator = ',';
        private const string SecretPrefix = "whsec_";

        // Todas explícitas y aplicadas con JsonSerializer.Create, que no lee JsonConvert.DefaultSettings: los ajustes
        // globales de la aplicación del integrador no cambian cómo se lee el sobre. createdAt llega en UTC con Z y así se
        // conserva; un campo nuevo del sobre no rompe a un SDK anterior.
        private static readonly JsonSerializerSettings EnvelopeSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            TypeNameHandling = TypeNameHandling.None,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DateParseHandling = DateParseHandling.DateTime,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            CheckAdditionalContent = true
        };

        // Estricta, como la librería de referencia: un texto que no es UTF-8 válido se rechaza en vez de firmarse con
        // caracteres de reemplazo.
        private static readonly UTF8Encoding StrictUtf8 = new UTF8Encoding(false, true);

        /// <summary>
        /// Verifica una entrega con el secreto del endpoint y devuelve su evento.
        /// </summary>
        /// <param name="headers">
        /// Los headers de la petición recibida; los nombres se comparan sin distinguir mayúsculas. En ASP.NET Core:
        /// <c>Request.Headers.ToDictionary(h =&gt; h.Key, h =&gt; h.Value.ToString())</c>.
        /// </param>
        /// <param name="body">
        /// El cuerpo <b>crudo</b> tal como llegó, leído como texto UTF-8 antes de deserializarlo. Un cuerpo deserializado y
        /// vuelto a serializar cambia sus bytes (espacios, orden, escapes) y su firma deja de coincidir.
        /// </param>
        /// <param name="secret">El secreto del endpoint (<c>whsec_…</c>); se acepta también sin el prefijo.</param>
        /// <param name="tolerance">Tolerancia del timestamp; sin valor, <see cref="DefaultTolerance"/>.</param>
        /// <returns>El evento del cuerpo verificado.</returns>
        /// <exception cref="WebhookVerificationException">
        /// Falta un header, el timestamp está fuera de la tolerancia, ninguna firma <c>v1</c> coincide o el cuerpo firmado
        /// no es un evento: rechace la entrega sin procesarla.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Un argumento es null, el secreto no es base64 o la tolerancia es negativa: es un error de configuración del
        /// receptor, no de la entrega.
        /// </exception>
        public static WebhookEvent Verify(IEnumerable<KeyValuePair<string, string>> headers, string body, string secret,
            TimeSpan? tolerance = null) =>
            Verify(headers, body, new[] { DecodeSecret(secret, nameof(secret), "El secreto de firma") },
                tolerance ?? DefaultTolerance, DateTimeOffset.UtcNow);

        /// <summary>
        /// Verifica una entrega con varios secretos y devuelve su evento: basta que una firma <c>v1</c> del header coincida
        /// con uno de ellos. Sirve durante una rotación, mientras el receptor despliega el secreto nuevo y conserva el
        /// anterior (FiscalAPI firma con ambos durante 24 h).
        /// </summary>
        /// <param name="headers">
        /// Los headers de la petición recibida; los nombres se comparan sin distinguir mayúsculas. En ASP.NET Core:
        /// <c>Request.Headers.ToDictionary(h =&gt; h.Key, h =&gt; h.Value.ToString())</c>.
        /// </param>
        /// <param name="body">
        /// El cuerpo <b>crudo</b> tal como llegó, leído como texto UTF-8 antes de deserializarlo. Un cuerpo deserializado y
        /// vuelto a serializar cambia sus bytes (espacios, orden, escapes) y su firma deja de coincidir.
        /// </param>
        /// <param name="secrets">Los secretos aceptados (<c>whsec_…</c>, o sin el prefijo); al menos uno.</param>
        /// <param name="tolerance">Tolerancia del timestamp; sin valor, <see cref="DefaultTolerance"/>.</param>
        /// <returns>El evento del cuerpo verificado.</returns>
        /// <exception cref="WebhookVerificationException">
        /// Falta un header, el timestamp está fuera de la tolerancia, ninguna firma <c>v1</c> coincide o el cuerpo firmado
        /// no es un evento: rechace la entrega sin procesarla.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Un argumento es null, no hay secretos, uno no es base64 o la tolerancia es negativa: es un error de
        /// configuración del receptor, no de la entrega.
        /// </exception>
        public static WebhookEvent Verify(IEnumerable<KeyValuePair<string, string>> headers, string body,
            IEnumerable<string> secrets, TimeSpan? tolerance = null) =>
            Verify(headers, body, secrets, tolerance ?? DefaultTolerance, DateTimeOffset.UtcNow);

        /// <summary>La verificación contra el momento <paramref name="now"/> del receptor.</summary>
        internal static WebhookEvent Verify(IEnumerable<KeyValuePair<string, string>> headers, string body,
            IEnumerable<string> secrets, TimeSpan tolerance, DateTimeOffset now) =>
            Verify(headers, body, DecodeSecrets(secrets), tolerance, now);

        private static WebhookEvent Verify(IEnumerable<KeyValuePair<string, string>> headers, string body,
            IReadOnlyCollection<byte[]> keys, TimeSpan tolerance, DateTimeOffset now)
        {
            if (headers == null)
                throw new ArgumentNullException(nameof(headers));
            if (body == null)
                throw new ArgumentNullException(nameof(body));
            if (tolerance < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(tolerance), tolerance, "La tolerancia no puede ser negativa.");

            var id = RequiredHeader(headers, IdHeader);
            var timestamp = RequiredHeader(headers, TimestampHeader);
            var signatures = RequiredHeader(headers, SignatureHeader);

            EnsureWithinTolerance(timestamp, tolerance, now);
            EnsureSigned(keys, SignedContent(id, timestamp, body), signatures);
            return ReadEvent(body);
        }

        private static List<byte[]> DecodeSecrets(IEnumerable<string> secrets)
        {
            if (secrets == null)
                throw new ArgumentNullException(nameof(secrets));

            var keys = secrets
                .Select((secret, index) =>
                    DecodeSecret(secret, nameof(secrets), $"El secreto de firma en la posición {index}"))
                .ToList();
            if (keys.Count == 0)
                throw new ArgumentException("Se necesita al menos un secreto de firma (whsec_…).", nameof(secrets));

            return keys;
        }

        // Los mensajes nombran el secreto por su posición y nunca incluyen su valor: terminarían en los logs del receptor.
        private static byte[] DecodeSecret(string secret, string parameterName, string description)
        {
            if (secret == null)
                throw new ArgumentNullException(parameterName, $"{description} es null.");

            var encoded = secret.StartsWith(SecretPrefix, StringComparison.Ordinal)
                ? secret.Substring(SecretPrefix.Length)
                : secret;
            byte[] key;
            try
            {
                key = Convert.FromBase64String(encoded);
            }
            catch (FormatException)
            {
                throw new ArgumentException(
                    $"{description} no es whsec_ seguido de base64: cópielo completo del endpoint de webhooks.",
                    parameterName);
            }

            if (key.Length == 0)
                throw new ArgumentException($"{description} está vacío.", parameterName);

            return key;
        }

        private static string RequiredHeader(IEnumerable<KeyValuePair<string, string>> headers, string name)
        {
            var value = headers
                .Where(header => string.Equals(header.Key, name, StringComparison.OrdinalIgnoreCase))
                .Select(header => header.Value)
                .FirstOrDefault();
            if (string.IsNullOrEmpty(value))
                throw new WebhookVerificationException($"Falta el header {name}.");

            return value;
        }

        private static void EnsureWithinTolerance(string timestamp, TimeSpan tolerance, DateTimeOffset now)
        {
            // Solo dígitos: un signo, espacios o decimales no son segundos Unix.
            long seconds;
            if (!long.TryParse(timestamp, NumberStyles.None, CultureInfo.InvariantCulture, out seconds))
                throw new WebhookVerificationException($"El header {TimestampHeader} no es un número de segundos Unix.");

            // En segundos, sin pasar por DateTimeOffset: un valor enorme se rechaza por la tolerancia, no con otra excepción.
            if (Math.Abs(now.ToUnixTimeSeconds() - seconds) > tolerance.TotalSeconds)
                throw new WebhookVerificationException(
                    $"El header {TimestampHeader} difiere más de {tolerance.TotalSeconds.ToString(CultureInfo.InvariantCulture)} s " +
                    "del reloj del receptor: puede ser una entrega repetida o un reloj desajustado.");
        }

        private static byte[] SignedContent(string id, string timestamp, string body)
        {
            try
            {
                return StrictUtf8.GetBytes(id + "." + timestamp + "." + body);
            }
            catch (EncoderFallbackException exception)
            {
                throw new WebhookVerificationException("El cuerpo o un header no es texto UTF-8 válido.", exception);
            }
        }

        private static void EnsureSigned(IReadOnlyCollection<byte[]> keys, byte[] signedContent, string signatureHeader)
        {
            var expected = keys.Select(key => Sign(key, signedContent)).ToList();
            var verified = signatureHeader
                .Split(SignatureSeparator)
                .Select(V1Signature)
                .Where(signature => signature != null)
                .Any(signature => expected.Any(candidate => FixedTimeEquals(candidate, signature)));
            if (!verified)
                throw new WebhookVerificationException(
                    $"Ninguna firma {SignatureVersion} del header {SignatureHeader} coincide con el cuerpo y el secreto.");
        }

        private static string Sign(byte[] key, byte[] content)
        {
            using (var hmac = new HMACSHA256(key))
                return Convert.ToBase64String(hmac.ComputeHash(content));
        }

        // La firma de una entrada "v1,<base64>"; null si es de otra versión o no tiene versión.
        private static string V1Signature(string entry)
        {
            var separator = entry.IndexOf(VersionSeparator);
            if (separator < 0 || !string.Equals(entry.Substring(0, separator), SignatureVersion, StringComparison.Ordinal))
                return null;

            return entry.Substring(separator + 1);
        }

        // Recorre siempre todos los caracteres: el tiempo de respuesta no revela cuántos coinciden. El largo de una firma
        // v1 es público (44), así que salir antes por largo no filtra nada.
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool FixedTimeEquals(string expected, string actual)
        {
            if (expected.Length != actual.Length)
                return false;

            var difference = 0;
            for (var i = 0; i < expected.Length; i++)
                difference |= expected[i] ^ actual[i];

            return difference == 0;
        }

        private static WebhookEvent ReadEvent(string body)
        {
            const string notAnEvent = "El cuerpo firmado no es un evento de webhook en JSON.";
            WebhookEvent webhookEvent;
            try
            {
                using (var reader = new JsonTextReader(new StringReader(body)))
                    webhookEvent = JsonSerializer.Create(EnvelopeSettings).Deserialize<WebhookEvent>(reader);
            }
            catch (JsonException exception)
            {
                throw new WebhookVerificationException(notAnEvent, exception);
            }

            // El JSON literal null deserializa sin error, pero no es un evento.
            return webhookEvent ?? throw new WebhookVerificationException(notAnEvent);
        }
    }
}
