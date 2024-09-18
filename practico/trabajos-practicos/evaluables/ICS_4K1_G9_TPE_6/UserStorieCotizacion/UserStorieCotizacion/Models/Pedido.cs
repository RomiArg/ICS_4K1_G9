using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserStorieCotizacion.Models
{
    public partial class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PedidoId { get; set; }

     
        public long PersonaId { get; set; }


        public string EstadoPedido { get; set; } = null!;

      
        public string DomicilioRetiro { get; set; } = null!;

      
        public DateTime FechaRetiro { get; set; }

      
        public string DomicilioEntrega { get; set; } = null!;

       
        public DateTime FechaEntrega { get; set; }

        // Aquí podrías agregar relaciones, por ejemplo, si un Pedido está relacionado con una Persona
        // public virtual Persona Persona { get; set; } = null!;
    }
}
