using System.Collections.Generic;

namespace Fiscalapi.Models
{
    /// <summary>
    /// La persona de la credencial (<c>Me.GetAsync</c>, GET /api/v4/me): quién es, de qué tenant y ambiente, si es el owner,
    /// lo que puede hacer y sus saldos. Siempre la persona de la API key; no trae secretos.
    /// </summary>
    public class MeInfo
    {
        /// <summary>Id de la persona.</summary>
        public string Id { get; set; }

        /// <summary>Nombre o razón social.</summary>
        public string LegalName { get; set; }

        /// <summary>RFC.</summary>
        public string Tin { get; set; }

        /// <summary>Correo.</summary>
        public string Email { get; set; }

        /// <summary>Tenant de la persona (el del header X-TENANT-KEY).</summary>
        public string TenantId { get; set; }

        /// <summary>Ambiente que responde: Development, Test o Production.</summary>
        public string Environment { get; set; }

        /// <summary>true si la persona es el owner del tenant (el mismo valor que <see cref="PersonFeatures.IsOwner"/>).</summary>
        public bool IsOwner { get; set; }

        /// <summary>Lo que la persona puede hacer: roles, permisos concedidos y negados, features del tenant y suscripción.</summary>
        public PersonFeatures Features { get; set; }

        /// <summary>Saldos por tipo de crédito, como <see cref="Person.Balances"/>; un tipo ausente tiene saldo 0.</summary>
        public List<CreditBalance> Balances { get; set; }
    }

    /// <summary>
    /// Lo que la persona puede hacer (<see cref="MeInfo.Features"/>). Es informativo: cada endpoint vuelve a evaluar su
    /// permiso.
    /// </summary>
    public class PersonFeatures
    {
        /// <summary>true si la persona es el owner del tenant.</summary>
        public bool IsOwner { get; set; }

        /// <summary>Roles de la persona: Owner o Member (más Host para la persona del host).</summary>
        public List<string> Roles { get; set; }

        /// <summary>Permisos concedidos, con la forma Área.Acción (por ejemplo, Invoices.Issue).</summary>
        public List<string> Permissions { get; set; }

        /// <summary>
        /// Permisos que el rol de la persona tiene pero se le niegan porque al tenant le faltan features, por permiso.
        /// </summary>
        public Dictionary<string, DeniedPermission> Denied { get; set; }

        /// <summary>Features habilitados del tenant, por código (por ejemplo, emision_cfdi), con sus valores.</summary>
        public Dictionary<string, PersonFeature> Features { get; set; }

        /// <summary>La suscripción que otorga los features del tenant.</summary>
        public PersonSubscription Subscription { get; set; }

        /// <summary>Los asientos de la persona en los features por persona; vacío si no tiene ninguno.</summary>
        public List<PersonSeatAssignment> SeatAssignments { get; set; }
    }

    /// <summary>Un permiso negado (<see cref="PersonFeatures.Denied"/>): los features que le faltan al tenant.</summary>
    public class DeniedPermission
    {
        /// <summary>Códigos de los features que le faltan al tenant.</summary>
        public List<string> MissingFeatures { get; set; }
    }

    /// <summary>Un feature habilitado del tenant (<see cref="PersonFeatures.Features"/>).</summary>
    public class PersonFeature
    {
        /// <summary>Valores del feature por código (por ejemplo, seats); vacío si no tiene.</summary>
        public Dictionary<string, int> Values { get; set; }
    }

    /// <summary>La suscripción que otorga los features del tenant (<see cref="PersonFeatures.Subscription"/>).</summary>
    public class PersonSubscription
    {
        /// <summary>Estado de la suscripción (active, trialing o past_due), o null si ninguna otorga features.</summary>
        public string Status { get; set; }

        /// <summary>false si no se pudieron leer los features del tenant (o la persona no tiene roles).</summary>
        public bool IsAvailable { get; set; }
    }

    /// <summary>Un asiento de la persona en un feature por persona (<see cref="PersonFeatures.SeatAssignments"/>).</summary>
    public class PersonSeatAssignment
    {
        /// <summary>Código del feature (por ejemplo, descarga_cfdi).</summary>
        public string FeatureCode { get; set; }

        /// <summary>Unidades que ocupa de la cantidad contratada.</summary>
        public int Units { get; set; }

        /// <summary>Active si cabe en la cantidad contratada (la persona usa el feature) o Suspended si no cabe.</summary>
        public string Status { get; set; }
    }
}
