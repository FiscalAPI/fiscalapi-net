
using FiscalApi.Common;
using Newtonsoft.Json;

namespace FiscalApi.Models
{
    public class ApiKey : BaseDto
    {
        public string Environment { get; set; }
        public string ApiKeyValue { get; set; }
        [JsonProperty("userId")] 
        public string PersonId { get; set; }
        public ApiKeyStatus ApiKeyStatus { get; set; }
    }

    public enum ApiKeyStatus
    {
        Disabled = 0,
        Enabled = 1
    }
}