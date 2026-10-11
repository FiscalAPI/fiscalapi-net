using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace Fiscalapi.Models
{
    /// <summary>
    /// Ticket de Auto CFDI: una venta pagada del punto de venta que el receptor puede autofacturar o que entra a una
    /// factura global. Se envía en <c>Tickets.CreateAsync</c> (POST /api/v4/tickets) y llega en <c>Tickets.GetListAsync</c>
    /// y <c>Tickets.GetByIdAsync</c>. Los conceptos e impuestos llevan los nombres de los conceptos de las facturas.
    /// Los miembros de solo lectura (Id, PersonId, importes calculados, Status, vigencia, facturas, CreatedAt y Events)
    /// los asigna el API: al crear quedan en null y no se envían.
    /// </summary>
    public class Ticket
    {
        /// <summary>Id del ticket. Solo lectura.</summary>
        public string Id { get; set; }

        /// <summary>Emisor del ticket, solo por referencia (Id de una persona del tenant). Se envía al crear; no llega en las consultas.</summary>
        public TicketIssuer Issuer { get; set; }

        /// <summary>Persona emisora del tenant. Solo lectura (al crear se envía en <see cref="Issuer"/>).</summary>
        public string PersonId { get; set; }

        /// <summary>Folio del punto de venta (no es el folio del CFDI).</summary>
        public string Folio { get; set; }

        /// <summary>Serie del punto de venta. Opcional.</summary>
        public string Series { get; set; }

        /// <summary>
        /// Momento del pago en hora civil de México (sin zona): se envía y llega con el formato yyyy-MM-ddTHH:mm:ss.
        /// </summary>
        [JsonIgnore]
        public DateTime PaymentDate { get; set; }

        [JsonProperty("paymentDate")]
        private string PaymentDateText
        {
            get => PaymentDate.ToString(SdkConstants.SatDateFormat, CultureInfo.InvariantCulture);
            set => PaymentDate = DateTime.Parse(value, CultureInfo.InvariantCulture);
        }

        /// <summary>Forma de pago (c_FormaPago), por ejemplo 01 (efectivo).</summary>
        public string PaymentFormCode { get; set; }

        /// <summary>Moneda (c_Moneda). Opcional: sin ella, el API usa MXN.</summary>
        public string CurrencyCode { get; set; }

        /// <summary>Tipo de cambio. Opcional; se envía tal cual, sin redondear.</summary>
        public decimal? ExchangeRate { get; set; }

        /// <summary>Código postal de expedición. Opcional: sin él, el API usa el del emisor.</summary>
        public string ExpeditionZipCode { get; set; }

        /// <summary>Subtotal calculado por el API. Solo lectura.</summary>
        public decimal? Subtotal { get; set; }

        /// <summary>Total de impuestos trasladados calculado por el API. Solo lectura.</summary>
        public decimal? TotalTransferredTaxes { get; set; }

        /// <summary>Total de impuestos retenidos calculado por el API. Solo lectura.</summary>
        public decimal? TotalWithheldTaxes { get; set; }

        /// <summary>
        /// Total que cobró el punto de venta: debe cuadrar al centavo con el que el API calcula de los conceptos e
        /// impuestos (si no, 422). Se envía tal cual, sin redondear.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Estado del ticket en camelCase (ver <see cref="TicketStatus"/>). Solo lectura. Es texto para conservar un estado
        /// que esta versión del SDK todavía no conoce.
        /// </summary>
        public string Status { get; set; }

        /// <summary>Si hoy el receptor puede autofacturarlo. Solo lectura.</summary>
        public bool? SelfInvoiceEligible { get; set; }

        /// <summary>Último momento de la ventana de autofactura; null si la ventana no cierra. Solo lectura.</summary>
        public DateTime? EligibleUntil { get; set; }

        /// <summary>Factura individual del ticket (estado invoiced). Solo lectura.</summary>
        public string InvoiceId { get; set; }

        /// <summary>Factura global del ticket (estado globalized). Solo lectura.</summary>
        public string GlobalInvoiceId { get; set; }

        /// <summary>Fecha de alta del ticket. Solo lectura.</summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>Conceptos del ticket. Obligatorios al crear; en las consultas solo llegan con details=true.</summary>
        public List<TicketItem> Items { get; set; }

        /// <summary>Historia del ticket en orden. Solo lectura; solo llega con details=true.</summary>
        public List<TicketEvent> Events { get; set; }
    }

    /// <summary>Emisor de un ticket: una persona del tenant, por su Id.</summary>
    public class TicketIssuer
    {
        public string Id { get; set; }
    }

    /// <summary>Concepto de un ticket, con los nombres de los conceptos de las facturas (sin terceros, aduanas, predial ni partes).</summary>
    public class TicketItem
    {
        /// <summary>Clave de producto o servicio (c_ClaveProdServ).</summary>
        public string ItemCode { get; set; }

        /// <summary>Número de identificación (SKU) del producto en el punto de venta.</summary>
        public string ItemSku { get; set; }

        public decimal Quantity { get; set; }

        /// <summary>Clave de unidad (c_ClaveUnidad).</summary>
        public string UnitOfMeasurementCode { get; set; }

        public string Description { get; set; }

        public decimal UnitPrice { get; set; }

        /// <summary>Descuento del concepto. Opcional.</summary>
        public decimal? Discount { get; set; }

        /// <summary>Objeto de impuesto (c_ObjetoImp): 02 exige impuestos; 01, 03, 04 y 05 no admiten.</summary>
        public string TaxObjectCode { get; set; }

        public List<TicketItemTax> ItemTaxes { get; set; }
    }

    /// <summary>Impuesto de un concepto del ticket.</summary>
    public class TicketItemTax
    {
        /// <summary>Impuesto: 001 (ISR), 002 (IVA) o 003 (IEPS).</summary>
        public string TaxCode { get; set; }

        /// <summary>Tipo de factor: Tasa, Cuota o Exento.</summary>
        public string TaxTypeCode { get; set; }

        /// <summary>Tasa o cuota, por ejemplo 0.160000m. Se envía tal cual, sin redondear.</summary>
        public decimal TaxRate { get; set; }

        /// <summary>T (traslado) o R (retención).</summary>
        public string TaxFlagCode { get; set; }
    }

    /// <summary>Un hecho de la historia del ticket (solo lectura; llega con details=true).</summary>
    public class TicketEvent
    {
        /// <summary>Tipo de hecho en camelCase (ver <see cref="TicketEventType"/>); texto para conservar uno que el SDK no conoce.</summary>
        public string Type { get; set; }

        public DateTime OccurredAt { get; set; }

        /// <summary>Quién actuó, en camelCase (ver <see cref="TicketActorType"/>); texto para conservar uno que el SDK no conoce.</summary>
        public string ActorType { get; set; }

        /// <summary>Factura del hecho (la individual o la global que lo timbró, liberó o devolvió); null si no aplica.</summary>
        public string InvoiceId { get; set; }
    }

    /// <summary>
    /// Respuesta de <c>Tickets.CreateAsync</c>: la misma forma en el 201 de un ticket nuevo y en el 200 de una repetición
    /// con la misma Idempotency-Key y el mismo cuerpo.
    /// </summary>
    public class TicketIngestionResult
    {
        public string Id { get; set; }

        /// <summary>Código del ticket sin guiones, como lo lleva el QR; se imprime en grupos (XXXX-XXXX-XXXX).</summary>
        public string TicketCode { get; set; }

        /// <summary>URL del portal de autofactura que abre el ticket.</summary>
        public string QrUrl { get; set; }

        /// <summary>Estado actual en camelCase (al ingerir, <see cref="TicketStatus.Pending"/>).</summary>
        public string Status { get; set; }

        public bool SelfInvoiceEligible { get; set; }

        /// <summary>Último momento de la ventana de autofactura; null si la ventana no cierra.</summary>
        public DateTime? EligibleUntil { get; set; }

        public decimal Subtotal { get; set; }
        public decimal TotalTransferredTaxes { get; set; }
        public decimal TotalWithheldTaxes { get; set; }
        public decimal Total { get; set; }
    }

    /// <summary>Código de un ticket y la URL de su QR, para reimprimirlo (GET /api/v4/tickets/{id}/code).</summary>
    public class TicketCodeResult
    {
        /// <summary>Código del ticket sin guiones, el mismo de la respuesta de la ingesta.</summary>
        public string TicketCode { get; set; }

        public string QrUrl { get; set; }
    }

    /// <summary>
    /// Filtros de <c>Tickets.GetListAsync</c> (GET /api/v4/tickets). Cada filtro aplica solo si se asigna; los nulos no se
    /// envían. Los tickets llegan ordenados por fecha de pago.
    /// </summary>
    public class TicketListFilters
    {
        /// <summary>Uno o más estados (ver <see cref="TicketStatus"/>); se envían repitiendo la llave status.</summary>
        public List<string> Status { get; set; }

        /// <summary>Persona emisora.</summary>
        public string PersonId { get; set; }

        /// <summary>Primer día del pago (incluido), en fecha civil de México: solo cuenta la fecha (yyyy-MM-dd).</summary>
        public DateTime? From { get; set; }

        /// <summary>Último día del pago (incluido), en fecha civil de México: solo cuenta la fecha (yyyy-MM-dd).</summary>
        public DateTime? To { get; set; }

        /// <summary>Página, desde 1. Sin ella, el API devuelve la primera.</summary>
        public int? PageNumber { get; set; }

        /// <summary>Tamaño de página (1 a 50). Sin él, el API usa 10.</summary>
        public int? PageSize { get; set; }

        /// <summary>La query string de los filtros asignados, sin el signo ?; vacía si no hay ninguno.</summary>
        internal string ToQueryString()
        {
            var parameters = new List<KeyValuePair<string, string>>();
            if (Status != null)
                parameters.AddRange(Status.Select(status => new KeyValuePair<string, string>("status", status)));
            AddIfPresent(parameters, "personId", PersonId);
            AddIfPresent(parameters, "from", FormatDate(From));
            AddIfPresent(parameters, "to", FormatDate(To));
            AddIfPresent(parameters, "pageNumber", PageNumber?.ToString(CultureInfo.InvariantCulture));
            AddIfPresent(parameters, "pageSize", PageSize?.ToString(CultureInfo.InvariantCulture));

            return string.Join("&",
                parameters.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
        }

        private static string FormatDate(DateTime? date) =>
            date?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        private static void AddIfPresent(List<KeyValuePair<string, string>> parameters, string key, string value)
        {
            if (value != null)
                parameters.Add(new KeyValuePair<string, string>(key, value));
        }
    }

    /// <summary>Estados de un ticket, como llegan en <see cref="Ticket.Status"/>.</summary>
    public static class TicketStatus
    {
        /// <summary>Ingerido y sin factura: lo puede facturar el receptor o entrar a una global.</summary>
        public const string Pending = "pending";

        /// <summary>El receptor lo está facturando.</summary>
        public const string Invoicing = "invoicing";

        /// <summary>Tiene su factura individual (<see cref="Ticket.InvoiceId"/>).</summary>
        public const string Invoiced = "invoiced";

        /// <summary>Una factura global lo está timbrando.</summary>
        public const string Globalizing = "globalizing";

        /// <summary>Está en una factura global (<see cref="Ticket.GlobalInvoiceId"/>).</summary>
        public const string Globalized = "globalized";

        /// <summary>Cancelado por el tenant antes de facturarse.</summary>
        public const string Cancelled = "cancelled";
    }

    /// <summary>Tipos de hecho de la historia de un ticket, como llegan en <see cref="TicketEvent.Type"/>.</summary>
    public static class TicketEventType
    {
        public const string Ingested = "ingested";
        public const string InvoicingStarted = "invoicingStarted";
        public const string Invoiced = "invoiced";
        public const string InvoiceRejected = "invoiceRejected";
        public const string OutcomeUnknown = "outcomeUnknown";
        public const string LeaseExpired = "leaseExpired";
        public const string Globalizing = "globalizing";
        public const string Globalized = "globalized";
        public const string ReleasedFromGlobal = "releasedFromGlobal";
        public const string Cancelled = "cancelled";
        public const string RecoveryFailed = "recoveryFailed";
        public const string Recovered = "recovered";
        public const string Resent = "resent";
        public const string ReturnedToPending = "returnedToPending";
    }

    /// <summary>Quién actuó en un hecho del ticket, como llega en <see cref="TicketEvent.ActorType"/>.</summary>
    public static class TicketActorType
    {
        /// <summary>Una persona del tenant por la API o el dashboard.</summary>
        public const string Tenant = "tenant";

        /// <summary>El receptor en el portal de autofactura.</summary>
        public const string Portal = "portal";

        /// <summary>Un proceso en segundo plano de FiscalAPI.</summary>
        public const string System = "system";
    }
}
