using System;
using System.Collections.Generic;
using Fiscalapi.Common;

namespace Fiscalapi.Models
{
    /// <summary>
    /// Representa el XML de un CFDI (Comprobante Fiscal Digital por Internet) descargado desde el SAT.
    /// </summary>
    public class Xml : BaseDto
    {
        // Regla de descarga
        public string DownloadRequestId { get; set; }

        // Version del CFDI
        public string Version { get; set; }

        // Serie
        public string Series { get; set; }

        // Folio
        public string Number { get; set; }

        // Fecha de emisión del CFDI
        public DateTime Date { get; set; }

        // Codigo de la forma de pago
        public string PaymentForm { get; set; }

        // Codigo del método de pago
        public string PaymentMethod { get; set; }

        // Numero de certificado del emisor
        public string CertificateNumber { get; set; }

        // Condiciones de pago
        public string PaymentConditions { get; set; }

        // Subtotal del CFDI
        public decimal SubTotal { get; set; }

        // Descuento aplicado al CFDI
        public decimal Discount { get; set; }

        // Codigo de la moneda del CFDI
        public string Currency { get; set; }

        // Tipo de cambio del CFDI (si aplica)
        public decimal ExchangeRate { get; set; }

        // Total del CFDI
        public decimal Total { get; set; }

        // Tipo de comprobante (I = Ingreso, E = Egreso, T = Traslado, N = Nómina, P = Pago)
        public string InvoiceType { get; set; }

        // Codigo de exportación (si aplica)
        public string Export { get; set; }

        // Lugar de expedición del CFDI
        public string ExpeditionZipCode { get; set; }

        // Confirmacion si aplica
        public string Confirmation { get; set; }


        // Total impuestos retenidos
        public decimal TotalWithheldTaxes { get; set; }

        // Total impuestos trasladados
        public decimal TotalTransferredTaxes { get; set; }

        // Información global del CFDI (para CFDI globales)
        public XmlGlobalInformation XmlGlobalInformation { get; set; }

        // Información de impuestos del CFDI
        public List<XmlTax> Taxes { get; set; } = new List<XmlTax>();

        // Información sobre facturas relacionada del CFDI (CFDI relacionados)
        public List<XmlRelated> XmlRelated { get; set; } = new List<XmlRelated>();

        // Información del emisor del CFDI
        public XmlIssuer XmlIssuer { get; set; }

        // Información del receptor del CFDI
        public XmlRecipient XmlRecipient { get; set; }

        // Información de los conceptos del CFDI
        public List<XmlItem> XmlItems { get; set; } = new List<XmlItem>();

        // Información de los complementos del CFDI
        public List<XmlComplement> XmlComplements { get; set; } = new List<XmlComplement>();

        // Xml crudo en base64
        public string Base64Content { get; set; }
    }

    public class XmlGlobalInformation : BaseDto
    {
        public string Periodicity { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
    }

    public class XmlIssuer : BaseDto
    {
        public string Tin { get; set; }
        public string LegalName { get; set; }
        public string TaxRegime { get; set; }
    }

    public class XmlItemCustomsInformation : BaseDto
    {
        public string XmlItemId { get; set; }
        public string CustomsDocumentNumber { get; set; }
    }

    public class XmlItem : BaseDto
    {
        public string XmlId { get; set; }
        public string ItemCode { get; set; }
        public string Sku { get; set; }
        public decimal Quantity { get; set; }
        public string UnitMeasurement { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public string TaxObject { get; set; }
        public string ThirdPartyAccount { get; set; }

        public List<XmlItemCustomsInformation> XmlItemCustomsInformation { get; set; } =
            new List<XmlItemCustomsInformation>();

        public List<XmlItemPropertyAccount> XmlItemPropertyAccounts { get; set; } = new List<XmlItemPropertyAccount>();
        public List<XmlItemTax> Taxes { get; set; } = new List<XmlItemTax>();
    }

    public class XmlItemPropertyAccount : BaseDto
    {
        public string XmlItemId { get; set; }
        public string PropertyAccountNumber { get; set; }
    }

    public class XmlItemTax : BaseDto
    {
        public decimal Base { get; set; }
        public string Tax { get; set; }
        public string TaxType { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string TaxFlag { get; set; }
        public string XmlItemId { get; set; }
    }

    public class XmlRecipient : BaseDto
    {
        public string Tin { get; set; }
        public string LegalName { get; set; }
        public string ZipCode { get; set; }
        public string TaxRegime { get; set; }
        public string CfdiUse { get; set; }
        public string ForeignTaxId { get; set; }
        public string FiscalResidence { get; set; }
    }

    public class XmlRelated : BaseDto
    {
        public string XmlId { get; set; }
        public string RelationshipType { get; set; }
        public string CfdiUuid { get; set; }
    }

    public class XmlTax : BaseDto
    {
        public decimal Base { get; set; }
        public string Tax { get; set; }
        public string TaxType { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string TaxFlag { get; set; }
        public string XmlId { get; set; }
    }

    public class XmlComplement : BaseDto
    {
        public string ComplementName { get; set; }
        public string Base64ComplementValue { get; set; }
        public string XmlId { get; set; }
    }

    public class MetadataItem : BaseDto
    {
        /// <summary>
        /// Folio de la factura - UUID
        /// </summary>
        public string InvoiceUuid { get; set; }

        /// <summary>
        /// RFC del emisor del comprobante - RfcEmisor
        /// </summary>
        public string IssuerTin { get; set; }

        /// <summary>
        /// Nombre o razón social del emisor - NombreEmisor
        /// </summary>
        public string IssuerName { get; set; }

        /// <summary>
        /// RFC del receptor del comprobante - RfcReceptor
        /// </summary>
        public string RecipientTin { get; set; }

        /// <summary>
        /// Nombre o razón social del receptor - NombreReceptor
        /// </summary>
        public string RecipientName { get; set; }

        /// <summary>
        /// RFC del Proveedor Autorizado de Certificación (PAC) - RfcPac
        /// </summary>
        public string PacTin { get; set; }

        /// <summary>
        /// Fecha y hora de emisión del comprobante - FechaEmision
        /// </summary>
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// Fecha y hora de certificación por el SAT - FechaCertificacionSat
        /// </summary>
        public DateTime SatCertificationDate { get; set; }

        /// <summary>
        /// Monto total del comprobante - Monto
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Tipo de comprobante (I = Ingreso, E = Egreso, T = Traslado, N = Nómina, P = Pago) - EfectoComprobante
        /// </summary>
        public string InvoiceType { get; set; }

        /// <summary>
        /// Estatus del comprobante (1 = Vigente, 0 = Cancelado) - Estatus
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Fecha de cancelación del comprobante (si aplica) - FechaCancelacion
        /// </summary>
        public DateTime CancellationDate { get; set; }

        public string DownloadPackageId { get; set; }
        public string DownloadRequestId { get; set; }
    }
}