using Fiscalapi.Common;

namespace Fiscalapi.Models
{
    /// <summary>
    /// Representa una plantilla para crear solicitudes de descarga de CFDI o metadatos.
    /// </summary>
    public class DownloadRule : BaseDto
    {
        public string PersonId { get; set; }
        public Person Person { get; set; }
        public string Tin { get; set; }
        public string Description { get; set; }

        // 1 Pendiente, 2 Aprobada, 3 Rechazada, 4 Abandonada
        public string DownloadRuleStatusId { get; set; }
        public CatalogDto DownloadRuleStatus { get; set; }

        //CFDI, Metadata.
        public string SatQueryTypeId { get; set; }
        public CatalogDto SatQueryType { get; set; }

        // Emitidos, Recibidos
        public string DownloadTypeId { get; set; }
        public CatalogDto DownloadType { get; set; }

        //Vigente, Cancelado
        public string SatInvoiceStatusId { get; set; }
        public CatalogDto SatInvoiceStatus { get; set; }
    }
}