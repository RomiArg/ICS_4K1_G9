namespace UserStorieCotizacion.Models.Request
{
    public class PagoRequest
    {
        public long CotizacionId { get; set; } // ID de la cotización a la que pertenece el pago
        public long FormaPagoId { get; set; } // ID de la forma de pago seleccionada
        public string EstadoPago { get; set; } = null!; // Estado del pago (Pendiente, Confirmado, etc.)
        public DateTime? FechaPago { get; set; } // Fecha en la que se realiza el pago, puede ser nula si aún no se realizó
        public long? NumeroDePagoDePasarelaDePago { get; set; } // Número de referencia devuelto por la pasarela de pago (opcional)
    }

}
