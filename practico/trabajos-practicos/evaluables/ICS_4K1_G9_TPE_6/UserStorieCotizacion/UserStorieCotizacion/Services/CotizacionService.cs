using Microsoft.EntityFrameworkCore;
using UserStorieCotizacion.Models;

namespace UserStorieCotizacion.Services
{
    public class CotizacionService
    {
        private readonly ISWContext _context;

        public CotizacionService(ISWContext context)
        {
            _context = context;
        }

        public IEnumerable<Cotizacion> GetAllCotizaciones()
        {
            return _context.Cotizaciones.ToList();
        }

        public Cotizacion GetCotizacionById(long id)
        {
            return _context.Cotizaciones.Find(id);

        }

        public Cotizacion CreateCotizacion(Cotizacion nuevaCotizacion)
        {
            _context.Cotizaciones.Add(nuevaCotizacion);
            _context.SaveChanges();
            return nuevaCotizacion;
        }

        public void UpdateCotizacion(Cotizacion cotizacion)
        {
            _context.Cotizaciones.Update(cotizacion);
            _context.SaveChanges();
        }

        public void DeleteCotizacion(long id)
        {
            var cotizacion = _context.Cotizaciones.Find(id);
            if (cotizacion != null)
            {
                _context.Cotizaciones.Remove(cotizacion);
                _context.SaveChanges();
            }


        }



        public IEnumerable<Cotizacion> GetCotizacionesByPedidoId(long pedidoId)
        {
            return _context.Cotizaciones.Where(c => c.PedidoId == pedidoId).ToList();
        }



        public List<Cotizacion> GetCotizacionesByPersonaId(long personaId)
        {
            return _context.Cotizaciones.Where(c => c.PersonaId == personaId).ToList();
        }

        public decimal? GetCalificacionByPersonaId(long personaId)
        {
            // Aquí asumimos que tienes una tabla `Persona` con el campo `Calificacion`
            var persona = _context.Personas.FirstOrDefault(p => p.PersonaId == personaId);
            return persona?.Calificacion;
        }



    }
}
