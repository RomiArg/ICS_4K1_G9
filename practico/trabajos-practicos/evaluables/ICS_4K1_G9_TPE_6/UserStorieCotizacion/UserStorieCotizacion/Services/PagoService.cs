using UserStorieCotizacion.Models;

namespace UserStorieCotizacion.Services
{
    public class PagoService
    {
        private readonly ISWContext _context;

        public PagoService(ISWContext context)
        {
            _context = context;
        }

        public IEnumerable<Pago> GetAllPagos()
        {
            return _context.Pagos.ToList();
        }

        public Pago GetPagoById(long id)
        {
            return _context.Pagos.Find(id);
        }

        public Pago CreatePago(Pago nuevoPago)
        {
            _context.Pagos.Add(nuevoPago);

            // Actualizar la cotización asociada
            var cotizacion = _context.Cotizaciones.Find(nuevoPago.CotizacionId);
            if (cotizacion != null)
            {
                cotizacion.Estado = "Confirmada"; // O el estado que sea apropiado
                _context.Cotizaciones.Update(cotizacion);
            }

            _context.SaveChanges();
            return nuevoPago;
        }

        public void UpdatePago(Pago pago)
        {
            _context.Pagos.Update(pago);
            _context.SaveChanges();
        }

        public void DeletePago(long id)
        {
            var pago = _context.Pagos.Find(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
                _context.SaveChanges();
            }
        }
    }
}
