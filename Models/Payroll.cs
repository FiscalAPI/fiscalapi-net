using Fiscalapi.Common;
using System;
using System.Collections.Generic;

namespace Fiscalapi.Models
{

    public class Payroll
    {
        public string Version { get; set; }
        public string PayrollTypeCode { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime InitialPaymentDate { get; set; }
        public DateTime FinalPaymentDate { get; set; }
        public int DaysPaid { get; set; }
        public PayrollEarnings Earnings { get; set; }
        public List<PayrollDeduction> Deductions { get; set; }
        public List<PayrollDisability> Disabilities { get; set; }
    }

    public class PayrollDisability
    {
        public int DisabilityDays { get; set; }

        public string DisabilityTypeCode { get; set; }

        public decimal MonetaryAmount { get; set; }
    }

    public class PayrollEarnings
    {
        public List<PayrollEarning> Earnings { get; set; }
        public List<PayrollEarningOtherPayment> OtherPayments { get; set; }
        public PayrollRetirement Retirement { get; set; }
        public PayrollSeverance Severance { get; set; }
    }

    public class PayrollSeverance
    {
        public decimal TotalPaid { get; set; }

        public int YearsOfService { get; set; }

        public decimal LastMonthlySalary { get; set; }

        public decimal AccumulableIncome { get; set; }

        public decimal NonAccumulableIncome { get; set; }
    }

    public class PayrollRetirement
    {
        public decimal TotalOneTime { get; set; }

        public decimal TotalInstallments { get; set; }

        public decimal DailyAmount { get; set; }

        public decimal AccumulableIncome { get; set; }

        public decimal NonAccumulableIncome { get; set; }
    }

    public class PayrollEarning
    {
        public string EarningTypeCode { get; set; }
        public string Code { get; set; }
        public string Concept { get; set; }
        public decimal TaxedAmount { get; set; }
        public decimal ExemptAmount { get; set; }
        public List<PayrollEarningOvertime> Overtime { get; set; }
    }

    public class PayrollEarningOvertime
    {
        public int Days { get; set; }
        public string HoursTypeCode { get; set; }
        public int ExtraHours { get; set; }
        public decimal AmountPaid { get; set; }
    }

    public class PayrollEarningOtherPayment
    {
        public string OtherPaymentTypeCode { get; set; }
        public string Code { get; set; }
        public string Concept { get; set; }
        public decimal Amount { get; set; }
        public decimal SubsidyCaused { get; set; }

        public BalanceCompensation BalanceCompensation {  get; set; }
    }

    public class BalanceCompensation : CatalogDto
    {
        public decimal FavorableBalance { get; set; }
        public int Year {  get; set; }
        public decimal RemainingFavorableBalance { get; set; }
    }

    public class PayrollDeduction
    {
        public string DeductionTypeCode { get; set; }
        public string Code { get; set; }
        public string Concept { get; set; }
        public decimal Amount { get; set; }
    }

}
