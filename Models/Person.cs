using Fiscalapi.Common;
using System;
using System.Collections.Generic;

namespace Fiscalapi.Models
{
    public class Person : BaseDto
    {
        //Mandatory fields
        public string LegalName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Contraseña del dashboard: requerida al crear; en Update, null o vacía conserva la actual. El API nunca la devuelve

        //Optional fields
        public string PhoneNumber { get; set; }

        public string SatTaxRegimeId { get; set; }
        public CatalogDto SatTaxRegime { get; set; }
        public string SatCfdiUseId { get; set; }
        public CatalogDto SatCfdiUse { get; set; }
        public string UserTypeId { get; set; } // "C" (cliente, por omisión al crear) o "U" (usuario). "T" (tenant) solo llega en respuestas: el API lo rechaza al crear y en Update solo lo acepta si la persona ya es "T"
        public CatalogDto UserType { get; set; }
        public string Tin { get; set; } // RFC (Tax Identification Number)
        public string ZipCode { get; set; }
        public string Base64Photo { get; set; }
        public string TaxPassword { get; set; } // Contraseña de la .key que la persona guarda en su perfil; el API no la usa para sellar. Con valor solo para la propia persona y el owner (los demás reciben null). En Update, null la conserva y "" la borra
        public string Curp { get; set; }

        //Foreign person fields (receptor extranjero)
        public string CountryId { get; set; } // Residencia fiscal, catálogo c_Pais (ej. "USA"). Máximo 36 caracteres
        public CatalogDto Country { get; set; }
        public string ForeignTin { get; set; } // NumRegIdTrib. Máximo 40 caracteres

        //Carta manifiesto (solo lectura en la API, lo escribe Manifests.SignAsync)
        public string ManifestStatusId { get; set; } // Ver ManifestStatusIds
        public CatalogDto ManifestStatus { get; set; }

        public int AvailableBalance { get; set; } // Timbres disponibles (solo lectura en la API)
        public int CommittedBalance { get; set; } // Campo heredado: el API ya no lo calcula y siempre vale 0 (solo lectura en la API)
        public int AvailableValidationBalance { get; set; } // Créditos de validación SAT disponibles (solo lectura en la API)
        public List<CreditBalance> Balances { get; set; } // Saldos por tipo de crédito (solo lectura en la API)
        public string TenantId { get; set; }
        public bool IsOwner { get; set; } // true si la persona es el owner de su tenant (solo lectura en la API)
        public DateTime? ValidTo { get; set; } // Fin de vigencia de la persona; la asigna el API, casi siempre null e informativa (solo lectura en la API)
    }

    /// <summary>
    /// Saldo de una persona para un tipo de crédito (<see cref="Person.Balances"/>). Solo aparecen los tipos que la persona
    /// ha tenido; un tipo ausente tiene saldo 0. Un tipo que este SDK no conoce conserva su número en <see cref="CreditType"/>.
    /// </summary>
    public class CreditBalance
    {
        public CreditType CreditType { get; set; }
        public int Available { get; set; }
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