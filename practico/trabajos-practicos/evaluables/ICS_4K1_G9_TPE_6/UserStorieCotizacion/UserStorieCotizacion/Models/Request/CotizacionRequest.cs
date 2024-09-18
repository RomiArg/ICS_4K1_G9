namespace UserStorieCotizacion.Models.Request
{
    public class CotizacionRequest
    {
        public long PersonaId { get; set; }
        public DateTime FechaRetiro { get; set; }
        public DateTime FechaEntrega { get; set; }
        public decimal Importe { get; set; }
        public string Estado { get; set; } = null!;
        public string? FormaPagoEstablecida { get; set; }

        public long PedidoId { get; set; }
    }
}
