namespace Fiscalapi.Models.ForeignTrade
{
    public class Emisor
    {
        public string Curp { get; set; }
        public EmisorDomicilio Domicilio { get; set; }
    }

    public class EmisorDomicilio
    {
        public string Calle { get; set; }
        public string NumeroExterior { get; set; }
        public string NumeroInterior { get; set; }
        public string ColoniaId { get; set; }
        public string LocalidadId { get; set; }
        public string Referencia { get; set; }
        public string MunicipioId { get; set; }
        public string EstadoId { get; set; }
        public string PaisId { get; set; }
        public string CodigoPostalId { get; set; }
    }
}
