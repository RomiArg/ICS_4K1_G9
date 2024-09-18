namespace UserStorieCotizacion.Models.Request
{
    public class UsuarioRequest
    {
        public string Email { get; set; } = null!; // Email del usuario
        public string Password { get; set; } = null!; // Contraseña del usuario
        public string Nombre { get; set; } = null!; // Nombre del usuario
    }
}
