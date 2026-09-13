namespace Fiscalapi.Models
{
    /// <summary>Solicitud de firma de la carta manifiesto. Requiere la e.firma (FIEL) del contribuyente, no el CSD de timbrado.</summary>
    public class SignManifestRequest
    {
        /// <summary>Certificado .cer de la e.firma (FIEL) en base64. Máximo 24576 caracteres.</summary>
        public string Base64Cer { get; set; }

        /// <summary>Llave privada .key de la e.firma (FIEL) en base64. Máximo 24576 caracteres.</summary>
        public string Base64Key { get; set; }

        /// <summary>Contraseña de la llave privada. Máximo 256 caracteres.</summary>
        public string Password { get; set; }
    }

    /// <summary>Ids del estatus de la carta manifiesto de una persona (Person.ManifestStatusId).</summary>
    public static class ManifestStatusIds
    {
        /// <summary>La persona aún no ha sido invitada a firmar la carta manifiesto.</summary>
        public const string NotInvited = "NotInvited";

        /// <summary>La persona fue invitada pero todavía no firma.</summary>
        public const string Unsigned = "Unsigned";

        /// <summary>La carta manifiesto ya fue firmada.</summary>
        public const string Signed = "Signed";
    }
}
