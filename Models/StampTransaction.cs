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
        /// Tipo de crédito a transferir: Stamp (timbres, por defecto), Validation (créditos de validación SAT) o Ticket (créditos de ticket).
        /// </summary>
        public CreditType CreditType { get; set; } = CreditType.Stamp;
    }

    /// <summary>
    /// Tipo de crédito de una transacción. Los saldos nunca se mezclan: Stamp afecta AvailableBalance,
    /// Validation afecta AvailableValidationBalance y Ticket afecta el saldo de tipo Ticket en Person.Balances.
    /// Un tipo que este SDK no conoce llega como un valor sin nombre que conserva su número.
    /// </summary>
    public enum CreditType
    {
        Stamp = 1,
        Validation = 2,
        Ticket = 3
    }
}
