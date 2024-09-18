using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserStorieCotizacion.Models
{
    public partial class Persona
    {
        public Persona()
        {
           // Cotizaciones = new HashSet<Cotizacion>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PersonaId { get; set; }
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Rol { get; set; }
        public long? Calificacion { get; set; }
        public string? OtrosCampos { get; set; }

       // public virtual ICollection<Cotizacion> Cotizacions { get; set; }
    }
}
