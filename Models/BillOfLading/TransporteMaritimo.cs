using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fiscalapi.Models.BillOfLading
{
    public class TransporteMaritimo
    {
        public string PermSCTId { get; set; }
        public string NumPermisoSCT { get; set; }
        public string NombreAseg { get; set; }
        public string NumPolizaSeguro { get; set; }
        public string TipoEmbarcacionId { get; set; }
        public string Matricula { get; set; }
        public string NumeroOMI { get; set; }
        public int? AnioEmbarcacion { get; set; }
        public string NombreEmbarc { get; set; }
        public string NacionalidadEmbarcId { get; set; }
        public decimal UnidadesDeArqBruto { get; set; }
        public string TipoCargaId { get; set; }
        public decimal? Eslora { get; set; }
        public decimal? Manga { get; set; }
        public decimal? Calado { get; set; }
        public decimal? Puntal { get; set; }
        public string LineaNaviera { get; set; }
        public string NombreAgenteNaviero { get; set; }
        public string NumAutorizacionNavieroId { get; set; }
        public string NumViaje { get; set; }
        public string NumConocEmbarc { get; set; }
        public string PermisoTempNavegacion { get; set; }
        public List<ContenedorMaritimo> Contenedores { get; set; }
    }

    public class ContenedorMaritimo
    {
        public string TipoContenedorId { get; set; }
        public string MatriculaContenedor { get; set; }
        public string NumPrecinto { get; set; }
        public string IdCCPRelacionado { get; set; }
        public string PlacaVMCCP { get; set; }

        [JsonIgnore]
        public DateTime? FechaCertificacionCCP { get; set; }

        [JsonProperty("FechaCertificacionCCP")]
        public string FechaCertificacionCCPString
        {
            get => FechaCertificacionCCP?.ToString(SdkConstants.SatDateFormat);
            private set => FechaCertificacionCCP = string.IsNullOrEmpty(value) ? (DateTime?)null : DateTime.Parse(value);
        }

        public List<RemolqueCCP> RemolquesCCP { get; set; }
    }

    public class RemolqueCCP
    {
        public string SubTipoRemCCPId { get; set; }
        public string PlacaCCP { get; set; }
    }
}
