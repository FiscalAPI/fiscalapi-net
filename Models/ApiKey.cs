using Fiscalapi.Common;

namespace Fiscalapi.Models
{
    public class ApiKey : BaseDto
    {
        public string Description { get; set; }
        public string Environment { get; set; }
        public string ApiKeyValue { get; set; }
        public string PersonId { get; set; }
        public string TenantId { get; set; }
        public ApiKeyStatus ApiKeyStatus { get; set; }
    }

    public enum ApiKeyStatus
    {
        Disabled = 0,
        Enabled = 1
    }
}