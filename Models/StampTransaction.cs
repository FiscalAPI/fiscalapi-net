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
    }

    public class StampTransactionParams
    {
        public string FromPersonId { get; set; }
        public string ToPersonId { get; set; }
        public int Amount { get; set; }
        public string Comments { get; set; }
    }
}
