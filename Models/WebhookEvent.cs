using System;

namespace Fiscalapi.Models
{
    /// <summary>
    /// Un evento de webhook de FiscalAPI: el mismo sobre para todo tipo de evento. Lo devuelve <c>Webhook.Verify</c>
    /// (<see cref="Fiscalapi.Webhooks.Webhook"/>) después de validar la firma. Es una notificación ligera: para el estado
    /// autoritativo del recurso (el orden de llegada no está garantizado), consulte
    /// <see cref="WebhookEventData.RelatedObject"/> con el servicio de su tipo (por ejemplo <c>Invoices.GetByIdAsync</c>).
    /// </summary>
    public class WebhookEvent
    {
        /// <summary>
        /// Id del evento; es también el header <c>webhook-id</c> y no cambia entre reintentos: úselo para descartar
        /// entregas repetidas.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Tipo de evento (ver <see cref="WebhookEventTypes"/>). Es texto y no un enum: un tipo que esta versión del SDK no
        /// conoce llega tal cual, sin error, y se ignora.
        /// </summary>
        public string Type { get; set; }

        /// <summary>Momento del cambio de estado, en UTC (<see cref="DateTimeKind.Utc"/>), al segundo.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>Persona del tenant dueña del recurso; null si el recurso no tiene una (factura emitida por valores).</summary>
        public string PersonId { get; set; }

        /// <summary>El recurso que cambió y sus estados.</summary>
        public WebhookEventData Data { get; set; }
    }

    /// <summary>Datos de un <see cref="WebhookEvent"/>.</summary>
    public class WebhookEventData
    {
        /// <summary>El recurso que cambió de estado.</summary>
        public WebhookRelatedObject RelatedObject { get; set; }

        /// <summary>
        /// Estado al que llegó el recurso, con los códigos públicos de la API v4 (factura <c>V</c>, <c>C</c>, <c>X</c>;
        /// solicitud de descarga <c>1</c>, <c>2</c>, <c>3</c>, <c>-1</c>; regla de descarga <c>1</c>..<c>4</c>, <c>-1</c>);
        /// null si el tipo de evento no lo lleva.
        /// </summary>
        public string Status { get; set; }

        /// <summary>Estado del que salió el recurso, con los mismos códigos que <see cref="Status"/>; null si no aplica.</summary>
        public string PreviousStatus { get; set; }
    }

    /// <summary>El recurso al que se refiere un <see cref="WebhookEvent"/>.</summary>
    public class WebhookRelatedObject
    {
        /// <summary>Id del recurso: el que reciben los <c>GetByIdAsync</c> de los servicios del SDK.</summary>
        public string Id { get; set; }

        /// <summary>Tipo de recurso: <c>invoice</c>, <c>download_request</c> o <c>download_rule</c>; el conjunto puede crecer.</summary>
        public string Type { get; set; }

        /// <summary>
        /// Ruta relativa del GET v4 del recurso (por ejemplo <c>/api/v4/invoices/{id}</c>), sin host: el mismo evento es
        /// idéntico en pruebas y en producción. Para leer el recurso use su servicio con <see cref="Id"/>.
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// Tipos de evento de webhook, como llegan en <see cref="WebhookEvent.Type"/>. Cada uno nombra el estado al que llegó
    /// el recurso. Son una ayuda para comparar: el catálogo puede crecer y un tipo desconocido se ignora.
    /// </summary>
    public static class WebhookEventTypes
    {
        /// <summary>La factura pasó de vigente a cancelación en proceso (<c>V</c> → <c>C</c>).</summary>
        public const string InvoiceCancellationRequested = "invoice.cancellation_requested";

        /// <summary>La cancelación en proceso terminó y la factura quedó cancelada (<c>C</c> → <c>X</c>).</summary>
        public const string InvoiceCanceled = "invoice.canceled";

        /// <summary>La cancelación en proceso se rechazó y la factura volvió a vigente (<c>C</c> → <c>V</c>).</summary>
        public const string InvoiceCancellationRejected = "invoice.cancellation_rejected";

        /// <summary>La solicitud de descarga masiva terminó con éxito (→ <c>3</c>).</summary>
        public const string DownloadRequestCompleted = "download_request.completed";

        /// <summary>
        /// La solicitud de descarga masiva se abandonó (→ <c>-1</c>). No implica un error: el motivo está en la solicitud
        /// (<c>DownloadRequests.GetByIdAsync</c>).
        /// </summary>
        public const string DownloadRequestAbandoned = "download_request.abandoned";

        /// <summary>La regla de descarga masiva quedó suspendida (<c>2</c> → <c>4</c>).</summary>
        public const string DownloadRuleSuspended = "download_rule.suspended";
    }
}
