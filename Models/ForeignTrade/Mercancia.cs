using System.Collections.Generic;

namespace Fiscalapi.Models.ForeignTrade
{
    public class Mercancia
    {
        public string NoIdentificacion { get; set; }
        public string FraccionArancelariaId { get; set; }
        public decimal? CantidadAduana { get; set; }
        public string UnidadAduanaId { get; set; }
        public decimal? ValorUnitarioAduana { get; set; }
        public decimal ValorDolares { get; set; }
        public List<DescripcionEspecifica> DescripcionesEspecificas { get; set; }
    }

    public class DescripcionEspecifica
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string SubModelo { get; set; }
        public string NumeroSerie { get; set; }
    }
}
