namespace Fiscalapi.Common
{
    public class FiscalapiSettings
    {
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public string ApiVersion { get; set; }= "v4";
        public string Tenant { get; set; }
        public string TimeZone { get; set; } = "America/Mexico_City"; // (iana time-zones https://en.wikipedia.org/wiki/List_of_tz_database_time_zones)
    }
}