using System;

namespace Fiscalapi.Webhooks
{
    /// <summary>
    /// La entrega no se puede tratar como auténtica: falta un header de Standard Webhooks, ninguna firma coincide, el
    /// timestamp está fuera de la tolerancia o el cuerpo firmado no es un evento. El receptor la rechaza (por ejemplo con
    /// un 400) y no la procesa.
    /// </summary>
    public class WebhookVerificationException : Exception
    {
        public WebhookVerificationException(string message) : base(message)
        {
        }

        public WebhookVerificationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
