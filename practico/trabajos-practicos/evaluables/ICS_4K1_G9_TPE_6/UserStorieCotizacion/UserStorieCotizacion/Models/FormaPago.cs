using System;
using System.Collections.Generic;

namespace UserStorieCotizacion.Models
{
    public partial class FormaPago
    {
        public FormaPago()
        {
            Pagos = new HashSet<Pago>();
        }

        public long FormaPagoId { get; set; }
        public string? Descripcion { get; set; }

        public virtual ICollection<Pago> Pagos { get; set; }
    }
}
