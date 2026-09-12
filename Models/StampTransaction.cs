using Fiscalapi.Common;

namespace Fiscalapi.Models
{
    public class StampTransaction : BaseDto
    {
        public int Consecutive { get; set; }
        public UserLookupDto FromPerson { get; set; }
        public UserLookupDto ToPerson { get; set; }
        public int Amount { get; set; }
        public int TransactionType { get; set; }
        public int TransactionStatus { get; set; }
        public string ReferenceId { get; set; }
        public string Comments { get; set; }
        public CreditType CreditType { get; set; }
    }

    public class StampTransactionParams
    {
        public string FromPersonId { get; set; }
        public string ToPersonId { get; set; }
        public int Amount { get; set; }
        public string Comments { get; set; }

        /// <summary>
        /// Tipo de crédito a transferir: Stamp (timbres, por defecto) o Validation (créditos de validación SAT).
        /// </summary>
        public CreditType CreditType { get; set; } = CreditType.Stamp;
    }

    /// <summary>
    /// Tipo de crédito de una transacción. Los saldos nunca se mezclan: Stamp afecta AvailableBalance
    /// y Validation afecta AvailableValidationBalance.
    /// </summary>
    public enum CreditType
    {
        Stamp = 1,
        Validation = 2
    }
}
