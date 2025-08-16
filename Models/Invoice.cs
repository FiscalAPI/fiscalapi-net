using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Fiscalapi.Common;

namespace Fiscalapi.Models
{
    public class Invoice : BaseDto
    {
        public string VersionCode { get; set; } = "4.0";
        public string Series { get; set; }
        public string Number { get; set; }

        [JsonIgnore] public DateTime Date { get; set; }

        [JsonProperty("Date")]
        public string InvoiceDate
        {
            get => Date.ToString(SdkConstants.SatDateFormat);
            private set => Date = DateTime.Parse(value);
        }

        public string PaymentFormCode { get; set; }
        public string CurrencyCode { get; set; } = "MXN";
        public string TypeCode { get; set; } = "I";
        public string ExpeditionZipCode { get; set; }
        public string ExportCode { get; set; } = "01";
        public InvoiceIssuer Issuer { get; set; }
        public InvoiceRecipient Recipient { get; set; }
        public List<RelatedInvoice> RelatedInvoices { get; set; }
        public List<InvoiceItem> Items { get; set; }
        public GlobalInformation GlobalInformation { get; set; }
        public Addendum Addendum { get; set; }

        // Nullable or optional properties in some invoices
        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal ExchangeRate { get; set; } = 1;

        public string PaymentConditions { get; set; }
        public string PacConfirmation { get; set; }
        public string PaymentMethodCode { get; set; } = "PUE";
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public List<InvoiceResponse> Responses { get; set; }
        public List<InvoicePayment> Payments { get; set; }
    }

    public class InvoiceIssuer : BaseDto
    {
        public string Tin { get; set; }
        public string LegalName { get; set; }
        public string TaxRegimeCode { get; set; }
        public string OperationNumber { get; set; }
        public List<TaxCredential> TaxCredentials { get; set; }
    }

    public class TaxCredential
    {
        public string Base64File { get; set; }
        public FileType FileType { get; set; }
        public string Password { get; set; }
    }

    public class InvoiceRecipient : BaseDto
    {
        public string Tin { get; set; }
        public string LegalName { get; set; }
        public string ZipCode { get; set; }
        public string ForeignCountryCode { get; set; }
        public string ForeignTin { get; set; }
        public string TaxRegimeCode { get; set; }
        public string CfdiUseCode { get; set; }
        public string Email { get; set; }
    }

    public class RelatedInvoice
    {
        public string RelationshipTypeCode { get; set; }
        public string Uuid { get; set; }
    }

    public class GlobalInformation
    {
        public string PeriodicityCode { get; set; }
        public string MonthCode { get; set; }
        public int Year { get; set; }
    }

    public class Addendum
    {
        public string Any { get; set; }
    }

    public class InvoiceItem : BaseDto
    {
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public string UnitOfMeasurementCode { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public string TaxObjectCode { get; set; }

        // Nullable or optional properties
        public string ItemSku { get; set; }
        public string UnitOfMeasurement { get; set; }
        public decimal Discount { get; set; }
        public List<InvoiceItemTax> ItemTaxes { get; set; }
    }

    public class InvoiceItemTax
    {
        public string TaxCode { get; set; }
        public string TaxTypeCode { get; set; }
        public decimal TaxRate { get; set; }
        public string TaxFlagCode { get; set; }
    }

    public class InvoicePayment
    {
        [JsonIgnore] public DateTime PaymentDate { get; set; }

        [JsonProperty("PaymentDate")]
        public string InvoiceDate
        {
            get => PaymentDate.ToString(SdkConstants.SatDateFormat);
            private set => PaymentDate = DateTime.Parse(value);
        }

        public string PaymentFormCode { get; set; }
        public string CurrencyCode { get; set; }

        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal ExchangeRate { get; set; } = 1;

        public decimal Amount { get; set; }
        public string OperationNumber { get; set; }
        public string SourceBankTin { get; set; }
        public string SourceBankAccount { get; set; }
        public string TargetBankTin { get; set; }
        public string TargetBankAccount { get; set; }
        public string ForeignBankName { get; set; }
        public string PaymentTypeCode { get; set; }
        public string Base64PaymentCertificate { get; set; }
        public string PaymentOriginalString { get; set; }
        public string SignatureValue { get; set; }
        public List<PaidInvoice> PaidInvoices { get; set; }
    }

    public class PaidInvoice
    {
        public string Uuid { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public string CurrencyCode { get; set; }
        public int PartialityNumber { get; set; }
        public decimal SubTotal { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal RemainingBalance { get; set; }
        public string TaxObjectCode { get; set; }

        [JsonConverter(typeof(DecimalJsonConverter))]
        public decimal Equivalence { get; set; } = 1;

        public decimal ExchangeRate { get; set; }
        public List<PaidInvoiceTax> PaidInvoiceTaxes { get; set; }
    }

    public class PaidInvoiceTax
    {
        public decimal Base { get; set; }
        public string TaxCode { get; set; }
        public string TaxTypeCode { get; set; }
        public decimal TaxRate { get; set; }
        public string TaxFlagCode { get; set; }
    }

    public class InvoiceResponse : BaseDto
    {
        public string InvoiceId { get; set; }
        public string InvoiceUuid { get; set; }
        public string InvoiceCertificateNumber { get; set; }
        public string InvoiceBase64Sello { get; set; }
        public DateTime InvoiceSignatureDate { get; set; }
        public string InvoiceBase64QrCode { get; set; }
        public string InvoiceBase64 { get; set; }
        public string SatBase64Sello { get; set; }
        public string SatBase64OriginalString { get; set; }
        public string SatCertificateNumber { get; set; }
    }


    public class CancelInvoiceRequest
    {
        /// <summary>
        /// Invoice id to cancel.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Invoice uuid to cancel.
        /// </summary>
        public string InvoiceUuid { get; set; }

        /// <summary>
        /// Taxpayer RFC.
        /// </summary>
        [JsonProperty("tin")]
        public string Rfc { get; set; }

        /// <summary>
        /// Cancellation reason code.
        /// </summary>
        public string CancellationReasonCode { get; set; }

        /// <summary>
        /// Replacement uuid.
        /// </summary>
        public string ReplacementUuid { get; set; }

        /// <summary>
        /// Tax credentials.
        /// </summary>
        public List<TaxCredential> TaxCredentials { get; set; }
    }


    public class CancelInvoiceResponse
    {
        /// <summary>
        /// XML Acknowledgement of cancellation encoded to base 64.
        /// To retrieve the raw XML you must decode using the DecodeFromBase64() extension method.
        /// </summary>
        public string Base64CancellationAcknowledgement { get; set; }

        /// <summary>
        /// List of canceled invoice uuids. Or list of  invoice uuids in process to be canceled.
        /// </summary>
        public Dictionary<string, string> InvoiceUuids { get; set; }
    }


    public class CreatePdfRequest
    {
        public string InvoiceId { get; set; }
        public string Base64Logo { get; set; }
        public string BandColor { get; set; }
        public string FontColor { get; set; }
    }

    public class SendInvoiceRequest
    {
        public string InvoiceId { get; set; }
        public string Base64Logo { get; set; }
        public string BandColor { get; set; }
        public string FontColor { get; set; }
        public string ToEmail { get; set; }
    }

    public enum FileType
    {
        CertificateCsd,
        PrivateKeyCsd,
    }


    /// <summary>
    /// Modelo para consultar el estado de facturas (CFDI)
    /// Permite consultar por referencias (usando id) o por valores (usando los demás campos)
    /// </summary>
    public class InvoiceStatusRequest
    {
        /// <summary>
        /// Id de la factura a consultar. Obligatorio cuando se consulta por referencias.
        /// </summary>

        public string Id { get; set; }

        /// <summary>
        /// RFC Emisor de la factura. Obligatorio cuando se consulta por valores.
        /// </summary>

        public string IssuerTin { get; set; }

        /// <summary>
        /// RFC Receptor de la factura. Obligatorio cuando se consulta por valores.
        /// </summary>

        public string RecipientTin { get; set; }

        /// <summary>
        /// Total de la factura. Obligatorio cuando se consulta por valores.
        /// </summary>

        public decimal InvoiceTotal { get; set; }

        /// <summary>
        /// Folio fiscal de la factura a consultar. Obligatorio tanto para consultas por referencias como por valores.
        /// </summary>

        public string InvoiceUuid { get; set; }

        /// <summary>
        /// Últimos ocho caracteres del sello digital del emisor. Obligatorio cuando se consulta por valores.
        /// </summary>

        public string Last8DigitsIssuerSignature { get; set; }
    }

    /// <summary>
    /// Modelo de respuesta de consulta de estado de facturas
    /// Contiene la información del estado de una factura consultada
    /// </summary>
    public class InvoiceStatusResponse
    {
        /// <summary>
        /// Código de estatus retornado por el SAT.
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// Estado actual de la factura.
        /// Posibles valores: 'Vigente' | 'Cancelado' | 'No Encontrado'
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Indica si la factura es cancelable.
        /// Posibles valores: 'Cancelable con aceptación' | 'No cancelable' | 'Cancelable sin aceptación'
        /// </summary>
        public string CancelableStatus { get; set; }

        /// <summary>
        /// Detalle del estatus de cancelación.
        /// Posibles valores: null | 'En proceso' | 'Plazo vencido' | 'Solicitud rechazada' | 
        /// 'Cancelado sin aceptación' | 'Cancelado con aceptación'
        /// </summary>

        public string CancellationStatus { get; set; }

        /// <summary>
        /// Codigo que indica si el RFC Emisor se encuentra dentro de la lista negra de 
        /// Empresas que Facturan Operaciones Simuladas (EFOS).
        /// Posible valores: 
        /// 100: El RFC Emisor se encuentra dentro de la lista de EFOS.
        /// 200: El RFC Emisor no se encuentra dentro de la lista de EFOS.
        /// </summary>
        public string EfosValidation { get; set; }
    }
}