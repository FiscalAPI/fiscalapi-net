using Fiscalapi.Common;
using System;
using System.Collections.Generic;

namespace Fiscalapi.Models
{
    public class DownloadRequest : BaseDto
    {
        public int Consecutive { get; set; }

        /// <summary>
        /// Sat Request ID used to track the request in the SAT system.
        /// </summary>
        public string SatRequestId { get; set; }

        /// <summary>
        /// RuleId associated with the request.
        /// </summary>
        public string DownloadRuleId { get; set; }

        public string DownloadTypeId { get; set; }
        public CatalogDto DownloadType { get; set; }

        public string DownloadRequestTypeId { get; set; }
        public CatalogDto DownloadRequestType { get; set; }


        /// <summary>
        /// RfcReceptor
        /// Specific CFDIs or metadata of the given recipient TIN (RFC).
        /// </summary>
        public string RecipientTin { get; set; }

        /// <summary>
        /// RfcEmisor
        /// Specific CFDIs or metadata of the given issuer TIN (RFC).
        /// </summary>
        public string IssuerTin { get; set; }

        /// <summary>
        /// RfcSolicitante
        /// RFC who is requesting the query.
        /// </summary>
        public string RequesterTin { get; set; }

        /// <summary>
        /// FechaInicial
        /// Start date for the associated request.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// FechaFinal
        /// End date for the associated request.
        /// </summary>
        public DateTime EndDate { get; set; }


        /// <summary>
        /// TipoSolicitud
        /// Request type for the request.
        /// CFDI or Metadata.
        /// </summary>
        public string SatQueryTypeId { get; set; }
        public CatalogDto SatQueryType { get; set; }


        /// <summary>
        /// TipoComprobante
        /// Specific invoice type to request.
        /// Ingreso
        /// Egreso
        /// Traslado
        /// Nómina
        /// Pago
        /// Todos
        /// </summary>

        public string SatInvoiceTypeId { get; set; }
        public CatalogDto SatInvoiceType { get; set; }


        /// <summary>
        /// EstadoComprobante
        /// CFDIs status to request.
        /// </summary>

        public string SatInvoiceStatusId { get; set; }
        public CatalogDto SatInvoiceStatus { get; set; }


        /// <summary>
        /// Complemento
        /// CFDIs complements for the request.
        /// </summary>

        public string SatInvoiceComplementId { get; set; }
        public CatalogDto SatInvoiceComplement { get; set; }


        /// <summary>
        /// Estado actual de la solicitud.
        /// DESCONOCIDO
        /// ACEPTADA
        /// EN PROCESO
        /// TERMINADA
        /// ERROR
        /// RECHAZADA
        /// VENCIDA
        /// </summary>

        public string SatRequestStatusId { get; set; }
        public CatalogDto SatRequestStatus { get; set; }


        /// <summary>
        /// Fiscalapi Request Status ID.
        /// </summary>
        public string DownloadRequestStatusId { get; set; }
        public CatalogDto DownloadRequestStatus { get; set; }


        /// <summary>
        /// FechaUltimoIntento
        /// Last attempt date for the associated request.
        /// </summary>
        public DateTime? LastAttemptDate { get; set; }

        /// <summary>
        /// FechaSiguienteIntento
        /// Next attempt date for the associated request.
        /// </summary>
        public DateTime? NextAttemptDate { get; set; }


        /// <summary>
        /// Number of CFDIs found for the request when request is terminated.
        /// </summary>
        public int InvoiceCount { get; set; }

        /// <summary>
        /// List of package IDs available for download when the request is terminated.
        /// </summary>
        public List<string> PackageIds { get; set; } = new List<string>();

        /// <summary>
        /// Indicates if the request is ready for download, becomes true when the request is terminated and packages are available.
        /// </summary>
        public bool IsReadyToDownload { get; set; }


        /// <summary>
        /// Number of total retries attempted for this request across all re-submissions.
        /// </summary>
        public int RetriesCount { get; set; }
    }
}