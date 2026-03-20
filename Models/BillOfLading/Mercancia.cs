using Fiscalapi.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fiscalapi.Models.BillOfLading
{
    public class Mercancia
    {
        public string BienesTranspId { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public string ClaveUnidadId { get; set; }
        public string MaterialPeligrosoId { get; set; }
        public string DenominacionGenericaProd { get; set; }
        public string DenominacionDistintivaProd { get; set; }
        public string Fabricante { get; set; }

        [JsonIgnore]
        public DateTime FechaCaducidad { get; set; }

        [JsonProperty("FechaCaducidad")]
        public string FechaCaducidadString
        {
            get => FechaCaducidad.ToString(SdkConstants.SatDateFormat);
            private set => FechaCaducidad = DateTime.Parse(value);
        }

        public string LoteMedicamento { get; set; }
        public string FormaFarmaceuticaId { get; set; }
        public string CondicionesEspTranspId { get; set; }
        public string RegistroSanitarioFolioAutorizacion { get; set; }
        public decimal PesoEnKg { get; set; }
        public string FraccionArancelariaId { get; set; }
        public string TipoMateriaId { get; set; }
        public string DescripcionMateria { get; set; }
        public decimal? ValorMercancia { get; set; }
        public string MonedaId { get; set; }
        public List<DocumentoAduanero> DocumentacionAduanera { get; set; }
        public List<CantidadTransporta> CantidadTransporta { get; set; }
        public DetalleMercancia DetalleMercancia { get; set; }
    }

    public class CantidadTransporta
    {
        public decimal Cantidad { get; set; }
        public string IdOrigen { get; set; }
        public string IdDestino { get; set; }
        public string CvesTransporteId { get; set; }
    }

    public class DocumentoAduanero
    {
        public string TipoDocumentoId { get; set; }
        public string NumPedimento { get; set; }
        public string RfcImpo { get; set; }
    }

    public class DetalleMercancia
    {
        public string UnidadPesoMercId { get; set; }
        public decimal PesoBruto { get; set; }
        public decimal PesoNeto { get; set; }
        public decimal PesoTara { get; set; }
        public int? NumPiezas { get; set; }
    }
}
