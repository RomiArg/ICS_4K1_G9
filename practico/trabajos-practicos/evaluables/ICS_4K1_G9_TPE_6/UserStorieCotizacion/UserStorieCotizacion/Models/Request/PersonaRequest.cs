namespace UserStorieCotizacion.Models.Request
{
    public class PersonaRequest
    {
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Rol { get; set; }
        public long? Calificacion { get; set; }
        public string? OtrosCampos { get; set; }
    }
}
