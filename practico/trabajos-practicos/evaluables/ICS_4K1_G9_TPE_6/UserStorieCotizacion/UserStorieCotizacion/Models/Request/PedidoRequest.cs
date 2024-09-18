namespace UserStorieCotizacion.Models.Request
{
    public class PedidoRequest
    {
        public long PersonaId { get; set; } // ID de la persona que realizó el pedido
        public string EstadoPedido { get; set; } = null!; // Estado del pedido (Pendiente, En preparación, Entregado, etc.)
        public string DomicilioRetiro { get; set; } = null!; // Dirección donde se retirará el pedido
        public DateTime FechaRetiro { get; set; } // Fecha de retiro del pedido
        public string DomicilioEntrega { get; set; } = null!; // Dirección donde se entregará el pedido
        public DateTime FechaEntrega { get; set; } // Fecha de entrega del pedido
    }
}
