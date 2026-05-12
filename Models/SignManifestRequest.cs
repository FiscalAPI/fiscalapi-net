namespace Fiscalapi.Models
{
    public class SignManifestRequest
    {
        public string Base64Cer { get; set; }
        public string Base64Key { get; set; }
        public string Password { get; set; }
    }
}
