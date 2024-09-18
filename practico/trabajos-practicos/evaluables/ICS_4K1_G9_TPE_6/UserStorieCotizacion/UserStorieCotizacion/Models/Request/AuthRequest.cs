using System.ComponentModel.DataAnnotations;

namespace UserStorieCotizacion.Models.Request
{
    public class AuthRequest
    {

        //solo pongo los datos que necesito para autentificar

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
