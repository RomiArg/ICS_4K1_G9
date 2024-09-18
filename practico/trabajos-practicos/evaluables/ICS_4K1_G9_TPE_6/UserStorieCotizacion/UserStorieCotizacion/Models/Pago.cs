using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserStorieCotizacion.Models
{
    public partial class Pago
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PagoId { get; set; }
        public long CotizacionId { get; set; }
        public long FormaPagoId { get; set; }
        public string EstadoPago { get; set; } = null!;
        public DateTime? FechaPago { get; set; }
        public long? NumeroDePagoDePasarelaDePago { get; set; }

        //public virtual Cotizacion Cotizacion { get; set; } = null!;
        //public virtual FormaPago FormaPago { get; set; } = null!;
    }
}
