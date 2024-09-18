namespace UserStorieCotizacion.Models.Response
{
    public class UserResponse
    {
        public long Id { get; set; }  // Agrega el campo Id
        public string Email { get; set; }
        public string Token { get; set; }

        public string Nombre { get; set; }
    }
}
