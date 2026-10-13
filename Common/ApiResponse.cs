using Newtonsoft.Json.Linq;

namespace Fiscalapi.Common
{
    public class ApiResponse<T>
    {
        
        public T Data { get; set; }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public int HttpStatusCode { get; set; }

        /// <summary>
        /// Identificador de la petición en los registros de FiscalAPI. Viene en los errores (por ejemplo, un 500 o un 502):
        /// inclúyalo al escribir a soporte@fiscalapi.com.
        /// </summary>
        public string TraceIdentifier { get; set; }

        /// <summary>
        /// El data crudo de una respuesta de error, tal como lo envió el API (por ejemplo existingTicketId en el 409 de
        /// Tickets.CreateAsync, o expectedTotal y computedTotal en su 422). Null en las respuestas exitosas y en los
        /// errores sin data. <see cref="Data"/> no cambia: en un error sigue en su valor por omisión.
        /// </summary>
        public JToken ErrorData { get; set; }
    }


    public class ValidationFailure
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public object AttemptedValue { get; set; } // El valor de un secreto (contraseñas, códigos, tokens, archivos y contraseñas de CSD/FIEL) llega como "[masked: n]", o "[masked]" si es un objeto o lista que lo contiene
    }
}