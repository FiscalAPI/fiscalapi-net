using Fiscalapi.Common;

namespace Fiscalapi.Models
{
    public class User : BaseDto
    {
    }

    public class UserLookupDto : BaseDto
    {
        public string Tin { get; set; }
        public string LegalName { get; set; }
    }
}
