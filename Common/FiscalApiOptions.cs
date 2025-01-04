namespace FiscalApi.Models.Common
{
    public class FiscalApiOptions
    {
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public string ApiVersion { get; set; }
        public string Tenant { get; set; }
        public string TimeZone { get; set; }
    }
}