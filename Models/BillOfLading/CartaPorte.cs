using System.Collections.Generic;

namespace Fiscalapi.Models.BillOfLading
{
    public class CartaPorte
    {
        public string TranspInternacId { get; set; }
        public string EntradaSalidaMercId { get; set; }
        public string PaisOrigenDestinoId { get; set; }
        public string ViaEntradaSalidaId { get; set; }
        public decimal TotalDistRec { get; set; }
        public decimal PesoNetoTotal { get; set; }
        public string RegistroISTMOId { get; set; }
        public string UbicacionPoloOrigenId { get; set; }
        public string UbicacionPoloDestinoId { get; set; }
        public string UnidadPesoId { get; set; }
        public string LogisticaInversaRecoleccionDevolucionId { get; set; }
        public List<RegimenAduanero> RegimenAduaneros { get; set; }
        public List<Ubicacion> Ubicaciones { get; set; }
        public List<Mercancia> Mercancias { get; set; }
        public Autotransporte Autotransporte { get; set; }
        public TransporteMaritimo TransporteMaritimo { get; set; }
        public TransporteAereo TransporteAereo { get; set; }
        public TransporteFerroviario TransporteFerroviario { get; set; }
        public List<TipoFigura> TiposFigura { get; set; }
    }
}
