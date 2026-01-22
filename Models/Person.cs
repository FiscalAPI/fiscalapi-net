using Fiscalapi.Common;
using System;

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

    public class EmployeeData : BaseDto
    {
        public string Curp { get; set; }
        public string EmployerPersonId { get; set; }
        public string EmployeePersonId { get; set; }
        public string EmployeeNumber { get; set; }
        public string SocialSecurityNumber { get; set; }
        public DateTime LaborRelationStartDate { get; set; }
        public CatalogDto SatContractType { get; set; }
        public CatalogDto SatTaxRegimeType { get; set; }
        public CatalogDto SatWorkdayType { get; set; }
        public CatalogDto SatJobRisk { get; set; }
        public CatalogDto SatPaymentPeriodicity { get; set; }
        public CatalogDto SatBank { get; set; }
        public CatalogDto SatPayrollState { get; set; }
        public CatalogDto SatUnionizedStatus { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Seniority { get; set; }
        public string SatUnionizedStatusId { get; set; }
        public string SatContractTypeId { get; set; }
        public string SatWorkdayTypeId { get; set; }
        public string SatTaxRegimeTypeId { get; set; }
        public string SatJobRiskId { get; set; }
        public string SatPaymentPeriodicityId { get; set; }
        public string SatBankId { get; set; }
        public string SatPayrollStateId { get; set; }
        public string BankAccount { get; set; }
        public decimal BaseSalaryForContributions { get; set; }
        public decimal IntegratedDailySalary { get; set; }
        public string SubcontractorRfc { get; set; }
        public decimal TimePercentage { get; set; }
    }

    public class EmployerData : BaseDto
    {
        public string PersonId { get; set; }
        public string EmployerRegistration { get; set; }
        public string OriginEmployerTin { get; set; }
        public CatalogDto SatFundSource { get; set; } // Rename to SatFundingSource
        public string SatFundSourceId { get; set; }
        public decimal OwnResourceAmount { get; set; }
    }
}