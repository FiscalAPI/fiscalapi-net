using System.Collections.Generic;

namespace Fiscalapi.Models.ForeignTrade
{
    public class Destinatario
    {
        public string NumRegIdTrib { get; set; }
        public string Nombre { get; set; }
        public List<DestinatarioDomicilio> Domicilios { get; set; }
    }

    public class DestinatarioDomicilio
    {
        public string Calle { get; set; }
        public string NumeroExterior { get; set; }
        public string NumeroInterior { get; set; }
        public string Colonia { get; set; }
        public string Localidad { get; set; }
        public string Referencia { get; set; }
        public string Municipio { get; set; }
        public string Estado { get; set; }
        public string PaisId { get; set; }
        public string CodigoPostal { get; set; }
    }
}
