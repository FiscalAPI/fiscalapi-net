using System.Collections.Generic;

namespace Fiscalapi.Models.BillOfLading
{
    public class TipoFigura
    {
        public string TipoFiguraId { get; set; }
        public string RfcFigura { get; set; }
        public string NumLicencia { get; set; }
        public string NombreFigura { get; set; }
        public List<ParteTransporte> PartesTransporte { get; set; }
        public Domicilio Domicilio { get; set; }
    }

    public class ParteTransporte
    {
        public string ParteTransporteId { get; set; }
    }
}
