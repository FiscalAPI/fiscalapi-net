using FiscalApi.Common;
using System.Collections.Generic;

namespace FiscalApi.Models
{
    public class Product : BaseDto
    {
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public string SatUnitMeasurementId { get; set; } //= "H87"; // Código del SAT (Unidad de medida)
        public CatalogDto SatUnitMeasurement { get; set; } 
        public string SatTaxObjectId { get; set; } //= "02"; //Código del SAT (objeto de impuesto)
        public CatalogDto SatTaxObject { get; set; }  
        public string SatProductCodeId { get; set; } //= "01010101"; // Código del SAT (Clave producto/servicio)
        public CatalogDto SatProductCode { get; set; } 
        public List<ProductTax> ProductTaxes { get; set; } // Impuestos del producto
    }

    public class ProductTax : BaseDto
    {
        /// <summary>
        /// Tasa del impuesto. 
        /// El valor debe estar entre 0.00000 y 1 (e.g., 0.160000 para un 16% de impuesto).
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// Impuesto.
        /// Valores posibles:
        /// * "001" = ISR
        /// * "002" = IVA
        /// * "003" = IEPS
        /// </summary>
        public string TaxId { get; set; }

        public CatalogDto Tax { get; set; }

        /// <summary>
        /// Naturaleza del impuesto.
        /// Valores posibles:
        /// * "T" = Traslado
        /// * "R" = Retención
        /// </summary>
        public string TaxFlagId { get; set; }

        public CatalogDto TaxFlag { get; set; }

        /// <summary>
        /// Tipo de impuesto.
        /// Valores posibles:
        /// * "Tasa"
        /// * "Cuota"
        /// * "Exento"
        /// </summary>
        public string TaxTypeId { get; set; }

        public CatalogDto TaxType { get; set; }
    }
}