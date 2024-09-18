using UserStorieCotizacion.Models;

namespace UserStorieCotizacion.Services
{
    public class PersonaService
    {
        private readonly ISWContext _context;

        public PersonaService(ISWContext context)
        {
            _context = context;
        }

        public IEnumerable<Persona> GetAllPersonas()
        {
            return _context.Personas.ToList();
        }

        public Persona GetPersonaById(long id)
        {
            return _context.Personas.Find(id);
        }

        public Persona CreatePersona(Persona nuevaPersona)
        {
            _context.Personas.Add(nuevaPersona);
            _context.SaveChanges();
            return nuevaPersona;
        }

        public void UpdatePersona(Persona persona)
        {
            _context.Personas.Update(persona);
            _context.SaveChanges();
        }

        public void DeletePersona(long id)
        {
            var persona = _context.Personas.Find(id);
            if (persona != null)
            {
                _context.Personas.Remove(persona);
                _context.SaveChanges();
            }
        }
    }
}