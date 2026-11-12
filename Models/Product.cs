using System.Collections.Generic;
using Fiscalapi.Common;

namespace Fiscalapi.Models
{
    public class Product : BaseDto
    {
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public string SatUnitMeasurementId { get; set; } // Default "H87"
        public CatalogDto SatUnitMeasurement { get; set; } 
        public string SatTaxObjectId { get; set; } //Default "02"
        public CatalogDto SatTaxObject { get; set; }  
        public string SatProductCodeId { get; set; } //Default "01010101"
        public CatalogDto SatProductCode { get; set; } 
        public List<ProductTax> ProductTaxes { get; set; } // Default "IVA 16%"
    }

    /// <summary>
    /// Filtros de <c>Products.GetListAsync</c> (GET /api/v4/products). Cada filtro asignado se aplica y los que quedan en
    /// null no se envían; sin ninguno, la lista es la de siempre (primera página).
    /// </summary>
    public class ProductListFilters
    {
        /// <summary>
        /// Texto a buscar, de 2 a 100 caracteres y sin distinguir mayúsculas: los productos cuya descripción o clave de
        /// producto o servicio del SAT lo contiene.
        /// </summary>
        public string Search { get; set; }

        /// <summary>Página, desde 1. Sin ella, el API devuelve la primera.</summary>
        public int? PageNumber { get; set; }

        /// <summary>Tamaño de página (1 a 50). Sin él, el API usa 10.</summary>
        public int? PageSize { get; set; }

        /// <summary>La query string de los filtros asignados, sin el signo ?; vacía si no hay ninguno.</summary>
        internal string ToQueryString() =>
            new QueryStringBuilder()
                .Add("search", Search)
                .Add("pageNumber", PageNumber)
                .Add("pageSize", PageSize)
                .ToString();
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