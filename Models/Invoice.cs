using System;
using System.Collections.Generic;

namespace FiscalApi.Models
{
    public class Invoice : BaseDto
    {
        public string VersionCode { get; set; } = "4.0";
        public string Series { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string PaymentFormCode { get; set; }
        public string PaymentConditions { get; set; } = "Contado";
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
        public string PacConfirmation { get; set; }
        public string PaymentMethodCode { get; set; } = "PUE";
        public decimal ExchangeRate { get; set; } = 1;
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string IssuerId { get; set; }
        public string RecipientId { get; set; }
        public string IssuerTin { get; set; }
        public string IssuerLegalName { get; set; }
        public string RecipientTin { get; set; }
        public string RecipientLegalName { get; set; }
        public List<InvoiceResponse> InvoiceResponses { get; set; }
        public InvoicePayment Payments { get; set; }
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
        public decimal FileType { get; set; }
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
        public decimal? Year { get; set; }
    }

    public class Addendum
    {
        // Add properties as needed
    }

    public class InvoiceItem
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
        public DateTime PaymentDate { get; set; }
        public string PaymentFormCode { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ExchangeRate { get; set; }
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
        public decimal PartialityNumber { get; set; }
        public decimal SubTotal { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal RemainingBalance { get; set; }
        public string TaxObjectCode { get; set; }
        public decimal Equivalence { get; set; }
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
}