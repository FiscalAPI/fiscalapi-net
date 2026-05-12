using System.Collections.Generic;

namespace Fiscalapi.Models.ForeignTrade
{
    public class ComercioExterior
    {
        public string MotivoTrasladoId { get; set; }
        public string ClaveDePedimentoId { get; set; }
        public int CertificadoOrigen { get; set; }
        public string NumCertificadoOrigen { get; set; }
        public string NumeroExportadorConfiable { get; set; }
        public string IncotermId { get; set; }
        public string Observaciones { get; set; }
        public decimal TipoCambioUSD { get; set; }
        public Emisor Emisor { get; set; }
        public Receptor Receptor { get; set; }
        public List<Propietario> Propietarios { get; set; }
        public List<Destinatario> Destinatarios { get; set; }
        public List<Mercancia> Mercancias { get; set; }
    }
}
