using UserStorieCotizacion.Models;

namespace UserStorieCotizacion.Services
{
    public class PedidoService
    {
        private readonly ISWContext _context;

        public PedidoService(ISWContext context)
        {
            _context = context;
        }

        public IEnumerable<Pedido> GetAllPedidos()
        {
            return _context.Pedidos.ToList();
        }

        public Pedido GetPedidoById(long id)
        {
            return _context.Pedidos.Find(id);
        }

        public IEnumerable<Pedido> GetPedidosByPersonaId(long personaId)
        {
            return _context.Pedidos.Where(p => p.PersonaId == personaId).ToList();
        }

        public Pedido CreatePedido(Pedido nuevoPedido)
        {
            _context.Pedidos.Add(nuevoPedido);

            // Actualización adicional si es necesario (p.ej., cambiar estado de otra entidad relacionada)
            _context.SaveChanges();
            return nuevoPedido;
        }

        public void UpdatePedido(Pedido pedidoActualizado)
        {
            _context.Pedidos.Update(pedidoActualizado);
            _context.SaveChanges();
        }

        public void DeletePedido(long id)
        {
            var pedido = _context.Pedidos.Find(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                _context.SaveChanges();
            }
        }

        

    }
}
