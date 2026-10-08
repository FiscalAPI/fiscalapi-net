namespace Fiscalapi.Common
{
    public class ApiResponse<T>
    {
        
        public T Data { get; set; }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public int HttpStatusCode { get; set; }
    }


    public class ValidationFailure
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public object AttemptedValue { get; set; } // El valor de un secreto (contraseñas, códigos, tokens, archivos y contraseñas de CSD/FIEL) llega como "[masked: n]", o "[masked]" si es un objeto o lista que lo contiene
    }
}