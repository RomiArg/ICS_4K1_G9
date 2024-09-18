using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserStorieCotizacion.Models
{
    public partial class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; } // Primary Key

      
        public string Email { get; set; } = null!; // Email del usuario

      
        public string Password { get; set; } = null!; // Contraseña del usuario

       
        public string Nombre { get; set; } = null!; // Nombre del usuario
    }
}
