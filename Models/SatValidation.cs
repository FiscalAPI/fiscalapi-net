using System.Collections.Generic;

namespace Fiscalapi.Models
{
    /// <summary>
    /// Solicitud de validaciones SAT sobre un CFDI (POST /api/v4/sat-validations).
    /// Envía <see cref="Xml"/> o <see cref="Tin"/>, nunca ambos: con Xml puedes solicitar cualquier tipo de validación;
    /// con Tin (RFC) únicamente listas negras (sat.blacklist.69b y sat.blacklist.69bbis).
    /// Cada tipo solicitado consume un crédito de validación; el cobro es todo o nada (402 si el saldo no alcanza).
    /// </summary>
    public class SatValidationRequest
    {
        /// <summary>CFDI completo (XML timbrado) codificado en Base64. Opcional, excluyente con <see cref="Tin"/>.</summary>
        public string Xml { get; set; }

        /// <summary>RFC a consultar en listas negras. Opcional, excluyente con <see cref="Xml"/>.</summary>
        public string Tin { get; set; }

        /// <summary>Ids de los tipos de validación a ejecutar (ver <see cref="SatValidationTypeIds"/>). Obligatorio, sin vacíos ni duplicados.</summary>
        public List<string> ValidationTypes { get; set; } = new List<string>();
    }

    /// <summary>Tipo de validación SAT del catálogo.</summary>
    public class SatValidationType
    {
        /// <summary>Id del tipo, por ejemplo sat.cfdi.status (ver <see cref="SatValidationTypeIds"/>).</summary>
        public string Id { get; set; }

        /// <summary>Descripción de lo que verifica el tipo.</summary>
        public string Description { get; set; }
    }

    /// <summary>Estatus de una validación SAT.</summary>
    public class SatValidationStatus
    {
        /// <summary>Id del estatus, por ejemplo Valido, Vigente, NoListado (ver <see cref="SatValidationStatusIds"/>).</summary>
        public string Id { get; set; }

        /// <summary>Descripción del estatus según el catálogo.</summary>
        public string Description { get; set; }

        /// <summary>
        /// Hechos del caso en texto libre (RFC y corte del listado, número de certificado, estado en el SAT).
        /// Es null en el catálogo de estatus y puede ser null en una ejecución.
        /// </summary>
        public string Details { get; set; }
    }

    /// <summary>
    /// Un tipo de validación con un estatus y su veredicto. Es el elemento de GET /sat-validations/{id}/statuses
    /// (estatus posibles del tipo) y de POST /sat-validations (estatus obtenido, en el orden del catálogo).
    /// </summary>
    public class SatValidationResult
    {
        /// <summary>Tipo de validación evaluado.</summary>
        public SatValidationType Type { get; set; }

        /// <summary>Estatus obtenido (o posible, en el catálogo).</summary>
        public SatValidationStatus Status { get; set; }

        /// <summary>True cuando el estatus se considera aprobado para ese tipo.</summary>
        public bool Passed { get; set; }
    }

    /// <summary>Ids de los tipos de validación SAT, en el orden del catálogo.</summary>
    public static class SatValidationTypeIds
    {
        /// <summary>Estructura del XML contra el Anexo 20 y sus complementos. Requiere Xml.</summary>
        public const string XmlStructure = "sat.xml.structure";

        /// <summary>Vigencia del certificado del emisor a la fecha de emisión. Requiere Xml.</summary>
        public const string CertificateValidity = "sat.certificate.validity";

        /// <summary>Sello del CFDI contra la cadena original y el certificado del emisor. Requiere Xml.</summary>
        public const string CfdiSello = "sat.cfdi.sello";

        /// <summary>Sello del SAT en el TimbreFiscalDigital. Requiere Xml.</summary>
        public const string TfdSello = "sat.tfd.sello";

        /// <summary>Estado actual del comprobante en el SAT (vigente, cancelado, no encontrado). Requiere Xml.</summary>
        public const string CfdiStatus = "sat.cfdi.status";

        /// <summary>Lista negra del artículo 69-B CFF. Admite Xml o Tin.</summary>
        public const string Blacklist69B = "sat.blacklist.69b";

        /// <summary>Lista negra del artículo 69-B Bis CFF. Admite Xml o Tin.</summary>
        public const string Blacklist69BBis = "sat.blacklist.69bbis";
    }

    /// <summary>Ids de los estatus de validación SAT. Los estatus posibles de cada tipo se consultan con GET /sat-validations/{id}/statuses.</summary>
    public static class SatValidationStatusIds
    {
        public const string Valido = "Valido";
        public const string Invalido = "Invalido";
        public const string Vigente = "Vigente";
        public const string Expirado = "Expirado";
        public const string NoVigenteAun = "NoVigenteAun";
        public const string Cancelado = "Cancelado";
        public const string NoEncontrado = "NoEncontrado";
        public const string NoListado = "NoListado";
        public const string Presunto = "Presunto";
        public const string Desvirtuado = "Desvirtuado";
        public const string Definitivo = "Definitivo";
        public const string SentenciaFavorable = "SentenciaFavorable";

        /// <summary>El servicio externo no respondió o no hay corte de lista importado; la validación consume crédito y debe reintentarse.</summary>
        public const string NoDisponible = "NoDisponible";

        /// <summary>No se evaluó porque la entrada no lo permite (XML inválido, sin TimbreFiscalDigital, sin certificado legible).</summary>
        public const string Omitido = "Omitido";
    }
}
