using System.Collections.Generic;

namespace Fiscalapi.Models.BillOfLading
{
    public class TransporteFerroviario
    {
        public string TipoDeServicioId { get; set; }
        public string TipoDeTraficoId { get; set; }
        public string NombreAseg { get; set; }
        public string NumPolizaSeguro { get; set; }
        public List<DerechoDePaso> DerechosDePaso { get; set; }
        public List<Carro> Carros { get; set; }
    }

    public class DerechoDePaso
    {
        public string TipoDerechoDePasoId { get; set; }
        public decimal KilometrajePagado { get; set; }
    }

    public class Carro
    {
        public string TipoCarroId { get; set; }
        public string MatriculaCarro { get; set; }
        public string GuiaCarro { get; set; }
        public decimal ToneladasNetasCarro { get; set; }
        public List<CarroContenedor> Contenedores { get; set; }
    }

    public class CarroContenedor
    {
        public string TipoContenedorId { get; set; }
        public decimal PesoContenedorVacio { get; set; }
        public decimal PesoNetoMercancia { get; set; }
    }
}
