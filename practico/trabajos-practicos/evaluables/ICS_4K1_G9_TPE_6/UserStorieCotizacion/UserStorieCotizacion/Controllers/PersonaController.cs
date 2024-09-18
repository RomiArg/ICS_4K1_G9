using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using UserStorieCotizacion.Hubs;
using UserStorieCotizacion.Models;
using UserStorieCotizacion.Models.Request;
using UserStorieCotizacion.Models.Response;
using UserStorieCotizacion.Services;

namespace UserStorieCotizacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly PersonaService _personaService;
        private readonly IHubContext<NotificacionesHub> _hubContext;
        public PersonaController(PersonaService personaService, IHubContext<NotificacionesHub> hubContext)
        {
            _personaService = personaService;
            _hubContext = hubContext;
        }




        [HttpGet("test-signalr")]
        public async Task<IActionResult> TestSignalR()
        {
            await _hubContext.Clients.All.SendAsync("RecibirNotificacion", "SignalR está funcionando desde gerpersona.");
            return Ok("Test de SignalR enviado.");
        }

        [HttpGet]
        public IActionResult Get()
        {
            Respuesta respuesta = new Respuesta();

            try
            {


                var personas = _personaService.GetAllPersonas();
                respuesta.Exito = 1;
                respuesta.Data = personas;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var persona = _personaService.GetPersonaById(id);
            if (persona == null)
                return NotFound();

            return Ok(persona);
        }

        [HttpPost]
        public IActionResult Post(PersonaRequest request)
        {
            var nuevaPersona = new Persona
            {
                Nombre = request.Nombre,
                Email = request.Email,
                Rol = request.Rol,
                Calificacion = request.Calificacion,
                OtrosCampos = request.OtrosCampos
            };

            Respuesta respuesta = new Respuesta();
            try
            {
                var creada = _personaService.CreatePersona(nuevaPersona);
                respuesta.Exito = 1;
                respuesta.Data = creada;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpPut("{id}")]
        public IActionResult Put(long id, PersonaRequest request)
        {
            var personaExistente = _personaService.GetPersonaById(id);
            if (personaExistente == null)
                return NotFound();

            personaExistente.Nombre = request.Nombre;
            personaExistente.Email = request.Email;
            personaExistente.Rol = request.Rol;
            personaExistente.Calificacion = request.Calificacion;
            personaExistente.OtrosCampos = request.OtrosCampos;

            Respuesta respuesta = new Respuesta();
            try
            {
                _personaService.UpdatePersona(personaExistente);
                respuesta.Exito = 1;
                respuesta.Data = personaExistente;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                _personaService.DeletePersona(id);
                respuesta.Exito = 1;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }
    }
}

