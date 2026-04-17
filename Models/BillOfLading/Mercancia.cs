using Fiscalapi.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fiscalapi.Models.BillOfLading
{
    public class Mercancia
    {
        public string BienesTranspId { get; set; }
        public string ClaveSTCCId { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public string ClaveUnidadId { get; set; }
        public string Unidad { get; set; }
        public string Dimensiones { get; set; }
        public string MaterialPeligrosoId { get; set; }
        public string CveMaterialPeligrosoId { get; set; }
        public string EmbalajeId { get; set; }
        public string DescripEmbalaje { get; set; }
        public string SectorCOFEPRISId { get; set; }
        public string NombreIngredienteActivo { get; set; }
        public string NomQuimico { get; set; }
        public string DenominacionGenericaProd { get; set; }
        public string DenominacionDistintivaProd { get; set; }
        public string Fabricante { get; set; }

        [JsonIgnore]
        public DateTime? FechaCaducidad { get; set; }

        [JsonProperty("FechaCaducidad")]
        public string FechaCaducidadString
        {
            get => FechaCaducidad?.ToString(SdkConstants.SatDateFormat);
            private set => FechaCaducidad = string.IsNullOrEmpty(value) ? (DateTime?)null : DateTime.Parse(value);
        }

        public string LoteMedicamento { get; set; }
        public string FormaFarmaceuticaId { get; set; }
        public string CondicionesEspTranspId { get; set; }
        public string RegistroSanitarioFolioAutorizacion { get; set; }
        public string PermisoImportacion { get; set; }
        public string FolioImpoVUCEM { get; set; }
        public string NumCAS { get; set; }
        public string RazonSocialEmpImp { get; set; }
        public string NumRegSanPlagCOFEPRIS { get; set; }
        public string DatosFabricante { get; set; }
        public string DatosFormulador { get; set; }
        public string DatosMaquilador { get; set; }
        public string UsoAutorizado { get; set; }
        public decimal PesoEnKg { get; set; }
        public decimal? ValorMercancia { get; set; }
        public string MonedaId { get; set; }
        public string FraccionArancelariaId { get; set; }
        public string UUIDComercioExt { get; set; }
        public string TipoMateriaId { get; set; }
        public string DescripcionMateria { get; set; }
        public List<DocumentoAduanero> DocumentacionAduanera { get; set; }
        public List<GuiaIdentificacion> GuiasIdentificacion { get; set; }
        public List<CantidadTransporta> CantidadTransporta { get; set; }
        public DetalleMercancia DetalleMercancia { get; set; }
    }

    public class CantidadTransporta
    {
        public decimal Cantidad { get; set; }
        public string IDOrigen { get; set; }
        public string IDDestino { get; set; }
        public string CvesTransporteId { get; set; }
    }

    public class DocumentoAduanero
    {
        public string TipoDocumentoId { get; set; }
        public string NumPedimento { get; set; }
        public string IdentDocAduanero { get; set; }
        public string RFCImpo { get; set; }
    }

    public class GuiaIdentificacion
    {
        public string NumeroGuiaIdentificacion { get; set; }
        public string DescripGuiaIdentificacion { get; set; }
        public decimal PesoGuiaIdentificacion { get; set; }
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
