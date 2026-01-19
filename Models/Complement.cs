using Fiscalapi.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fiscalapi.Models
{
    public class Complement : BaseDto
    {
        public Payroll Payroll { get; set; }
        public InvoicePayment Payment { get; set; }
        public LocalTaxes LocalTaxes { get; set; }
    }

    public class LocalTaxes
    {
        public List<LocalTax> Taxes { get; set; }
    }

    public class LocalTax
    {
        
        public string TaxName { get; set; }

        public decimal TaxRate { get; set; }

        public decimal TaxAmount { get; set; }

        public string TaxFlagCode { get; set; }
    }
}
