using System;
using Fiscalapi.Common;
using Newtonsoft.Json;

namespace Fiscalapi.Models
{
    public class TaxFile : BaseDto
    {
        /// <summary>
        /// PersonId who owns the tax file
        /// </summary>
        [JsonProperty("userId")]
        public string PersonId { get; set; }

        public string Tin { get; set; } // RFC (Tax Identification Number)
        public string Base64File { get; set; }
        public FileType FileType { get; set; }
        public string Password { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int Sequence { get; set; }
    }
}