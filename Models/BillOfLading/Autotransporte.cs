using System.Collections.Generic;

namespace Fiscalapi.Models.BillOfLading
{
    public class Autotransporte
    {
        public string PermSCTId { get; set; }
        public string NumPermisoSCT { get; set; }
        public string ConfigVehicularId { get; set; }
        public decimal PesoBrutoVehicular { get; set; }
        public string PlacaVM { get; set; }
        public int AnioModeloVM { get; set; }
        public string AseguraRespCivil { get; set; }
        public string PolizaRespCivil { get; set; }
        public List<Remolque> Remolques { get; set; }
    }

    public class Remolque
    {
        public string SubTipoRemId { get; set; }
        public string Placa { get; set; }
    }
}
