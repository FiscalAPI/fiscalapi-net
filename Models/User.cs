using Fiscalapi.Common;
using System;
using System.Collections.Generic;
using System.Text;

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
