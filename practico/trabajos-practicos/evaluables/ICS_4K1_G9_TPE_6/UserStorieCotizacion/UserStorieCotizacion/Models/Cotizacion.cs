using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserStorieCotizacion.Models
{
    public partial class Cotizacion
    {
        public Cotizacion()
        {
            //Pagos = new HashSet<Pago>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Asegúrate de que esto esté presente
        public long CotizacionId { get; set; }
        public long PersonaId { get; set; }
        public DateTime FechaRetiro { get; set; }
        public DateTime FechaEntrega { get; set; }
        public decimal Importe { get; set; }
        public string Estado { get; set; } = null!;
        public string? FormaPagoEstablecida { get; set; }

        public long PedidoId { get; set; }

        public long CalificacionTransportista { get; set; }

        public string NombreCotizador { get; set; }

        //public virtual Persona Persona { get; set; } = null!;
        //public virtual ICollection<Pago> Pagos { get; set; }
    }
}
