using System;
using Fiscalapi.Common;

namespace Fiscalapi.Models
{
    public class TaxFile : BaseDto
    {
        /// <summary>
        /// PersonId who owns the tax file
        /// </summary>
        public string PersonId { get; set; }
        /// <summary>
        /// RFC de la persona. Opcional en CreateAsync: si se omite (null o vacío), el API usa el RFC de la persona; si se
        /// envía, debe ser el RFC de la persona (sin distinguir mayúsculas) o el API responde 400 con la falla en Tin. El
        /// API no guarda el valor enviado: siempre devuelve el RFC de la persona.
        /// </summary>
        public string Tin { get; set; }
        public string Base64File { get; set; }
        public FileType FileType { get; set; }
        /// <summary>
        /// Contraseña de la llave privada (.key). Solo se envía al subir con CreateAsync (en el .cer y en la .key). En las
        /// consultas (GetListAsync, GetByIdAsync y la respuesta de CreateAsync) el API la devuelve vacía (""); solo
        /// GetDefaultValuesAsync la devuelve.
        /// </summary>
        public string Password { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int Sequence { get; set; }
    }
}