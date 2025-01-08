using Fiscalapi.Common;

namespace Fiscalapi.Models
{
    public class Person : BaseDto
    {
        //Mandatory fields
        public string LegalName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Password to access to the dashboard 

        //Optional fields
        public string CapitalRegime { get; set; }
        public string SatTaxRegimeId { get; set; }
        public CatalogDto SatTaxRegime { get; set; }
        public string SatCfdiUseId { get; set; }
        public CatalogDto SatCfdiUse { get; set; }
        public string UserTypeId { get; set; }
        public CatalogDto UserType { get; set; }
        public string Tin { get; set; } // RFC (Tax Identification Number)
        public string ZipCode { get; set; }
        public string Base64Photo { get; set; }
        public string TaxPassword { get; set; }
        public int AvailableBalance { get; }
        public int CommittedBalance { get; }
        public string TenantId { get; set; }
    }
}