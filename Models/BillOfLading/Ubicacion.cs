using Newtonsoft.Json;
using System;

namespace Fiscalapi.Models.BillOfLading
{
    public class Ubicacion
    {
        public string TipoUbicacion { get; set; }
        public string IDUbicacion { get; set; }
        public string RFCRemitenteDestinatario { get; set; }
        public string NumRegIdTrib { get; set; }
        public string ResidenciaFiscalId { get; set; }
        public string NombreRemitenteDestinatario { get; set; }
        public string NumEstacionId { get; set; }
        public string NombreEstacion { get; set; }
        public string NavegacionTraficoId { get; set; }
        public string TipoEstacionId { get; set; }

        [JsonIgnore]
        public DateTime FechaHoraSalidaLlegada { get; set; }

        [JsonProperty("FechaHoraSalidaLlegada")]
        public string FechaHoraSalidaLlegadaString
        {
            get => FechaHoraSalidaLlegada.ToString(SdkConstants.SatDateFormat);
            private set => FechaHoraSalidaLlegada = DateTime.Parse(value);
        }

        public decimal? DistanciaRecorrida { get; set; }
        public Domicilio Domicilio { get; set; }
    }
}
